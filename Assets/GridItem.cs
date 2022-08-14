using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    public EndGameManager endGameManager;
    public GridSettings gridSettings;
    public Vector2 gridPosition;
    public Vector3 holdPosition;

    private GameObject _occupant;
    private bool _targeted;
    private float _progress = 0f;
    private GridItem _target;
    private Vector2 _facing;
    private bool _activated;
    private bool _busyThisFrame;
    private bool _freeNextFrame;
    private bool _blocked;

    private float _spawnCooldown = 0f;

    private readonly Queue<GridItem> _queue = new Queue<GridItem>();
    private bool _shrink;
    private float _standingTime;
    private float _busyCooldown;
    private GameObject _factory;

    public void SetPosition(Vector2 newGridPosition)
    {
        gridPosition = newGridPosition;
        holdPosition = ((Vector3) gridPosition) - Vector3.forward;
    }

    public bool ReadyToFabricate()
    {
        return _spawnCooldown >= gridSettings.spawnTime;
    }

    void FixedUpdate()
    {
        _busyCooldown = Mathf.Max(0f, _busyCooldown - Time.fixedDeltaTime);
        if (_shrink)
        {
            transform.localScale *= .8f;
            if (transform.localScale.magnitude < .001f)
            {
                gameObject.SetActive(false);
            }
        }

        if (endGameManager.gameOver) return;

        if (_target)
        {
            _progress = Mathf.Min(1f,
                _progress + Time.fixedDeltaTime * gridSettings.moveSpeed * gridSettings.timeScale);

            var curvedProgress = gridSettings.basicMovement.Evaluate(_progress);
            var newPosition = Vector3.Lerp(holdPosition, _target.holdPosition,
                curvedProgress);

            _occupant.transform.position = newPosition;

            if (_progress >= 1f)
            {
                FinishMove();
            }
        }

        if (_freeNextFrame)
        {
            _busyThisFrame = false;
            _freeNextFrame = false;
        }
        else if (_busyThisFrame)
        {
            _freeNextFrame = true;
        }

        if (!ReadyToFabricate())
        {
            _spawnCooldown += Time.fixedDeltaTime * gridSettings.timeScale;
        }
    }

    private void FinishMove()
    {
        _occupant.GetComponentInChildren<BasicBlockController>()?.DecreaseValue();
        _target.SetBlock(ReleaseOccupant());
        _target.Arrived();
        _target = null;
    }

    public void SetBlock(GameObject newOccupant)
    {
        _occupant = newOccupant;
    }

    private GameObject ReleaseOccupant()
    {
        var o = _occupant;
        _occupant = null;
        return o;
    }

    public void MarkAsFactory(Vector2 direction)
    {
        Debug.Log("MARK: " + direction);
        _facing = direction;
        if (!_occupant)
        {
            var position = transform.position - Vector3.forward;
            _occupant = Instantiate(gridSettings.basicBlock, position, Quaternion.identity, null);
            _activated = true;
            _spawnCooldown = 0f;
        }

        AddFactory();
    }

    public bool Busy()
    {
        return _target || _targeted || _busyThisFrame || _blocked || _busyCooldown > 0f;
    }

    public bool CanBeTargeted()
    {
        return !Busy() && _factory == null;
    }

    public bool Occupied()
    {
        return _occupant != null;
    }

    private void Targeted(Vector2 direction)
    {
        if (_facing == Vector2.zero)
        {
            _facing = direction;
        }

        _targeted = true;
    }

    private void Arrived()
    {
        _targeted = false;
        _busyCooldown = 1f;
    }

    public void MoveTo(GridItem target)
    {
        _target = target;
        _progress = 0f;
        _target.Targeted(_facing);
    }

    public Vector2 Facing()
    {
        return _facing;
    }

    public bool Activated()
    {
        return _activated;
    }

    public void DestroyOccupant()
    {
        Destroy(_occupant);
        _occupant = null;
        _busyThisFrame = true;
    }

    public void AddToQueue(GridItem next)
    {
        _queue.Enqueue(next);
    }

    public bool HasQueue()
    {
        return _queue.Count > 0;
    }

    public bool IsNextInQueue(GridItem gridItem)
    {
        return _queue.Peek() == gridItem;
    }

    public void DequeueNext()
    {
        _queue.Dequeue();
    }

    public bool AlreadyInQueue(GridItem gridItem)
    {
        return _queue.Contains(gridItem);
    }

    public bool GridLocked()
    {
        if (!_occupant) return false;

        var basicBlockController = _occupant.GetComponentInChildren<BasicBlockController>();
        return basicBlockController && basicBlockController.GetNumber() > 3;
    }

    public void StandStill(float delta)
    {
        _standingTime += delta;
        if (_standingTime >= 2f)
        {
            _standingTime -= 2f;
            _occupant.GetComponentInChildren<BasicBlockController>()?.IncreaseValue();
        }
    }

    public void Shrink()
    {
        _shrink = true;
    }

    public void ResetFabricationCooldown()
    {
        _spawnCooldown = 0f;
    }

    public void AddBlocker()
    {
        _blocked = true;
        _occupant = Instantiate(gridSettings.blockerBlock, holdPosition, Quaternion.identity, null);
    }

    private void AddFactory()
    {
        _factory = Instantiate(gridSettings.factoryBlock, holdPosition, Quaternion.identity, null);
    }
}