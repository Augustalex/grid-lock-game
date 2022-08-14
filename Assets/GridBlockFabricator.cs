using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class GridBlockFabricator : MonoBehaviour
{
    public EndGameManager endGameManager;
    public GridSettings gridSettings;

    private GridRepository _gridRepository;
    private Coroutine _routine;

    private const float Delta = .02f;

    void Awake()
    {
        _gridRepository = GetComponent<GridRepository>();
    }

    private void OnEnable()
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(Cycle());
    }

    private void OnDisable()
    {
        StopCoroutine(_routine);
        _routine = null;
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Delta);
            if (endGameManager.gameOver) yield break;

            foreach (var gridItem in _gridRepository.gridItems)
            {
                if (gridItem.Busy()) continue;

                if (gridItem.Activated() && !gridItem.Occupied() && gridItem.ReadyToFabricate())
                {
                    gridItem.ResetFabricationCooldown();
                    var block = Instantiate(gridSettings.basicBlock, gridItem.holdPosition, Quaternion.identity,
                        null);
                    gridItem.SetBlock(block);
                }

                if (gridItem.Occupied())
                {
                    var direction = gridItem.Facing();
                    var next = _gridRepository.GetInDirection(gridItem.gridPosition, direction);
                    if (next)
                    {
                        if (next.CanBeTargeted() && !next.Occupied())
                        {
                            if (next.Busy())
                            {
                                if (!next.AlreadyInQueue(gridItem))
                                {
                                    next.AddToQueue(gridItem);
                                }
                                else
                                {
                                    gridItem.StandStill(Delta);
                                }
                            }
                            else
                            {
                                if (next.HasQueue())
                                {
                                    if (next.IsNextInQueue(gridItem))
                                    {
                                        next.DequeueNext();
                                        gridItem.MoveTo(next);
                                    }
                                }
                                else
                                {
                                    gridItem.MoveTo(next);
                                }
                            }
                        }
                        else
                        {
                            gridItem.StandStill(Delta);
                        }
                    }
                    else
                    {
                        gridItem.DestroyOccupant();
                    }
                }

                // var neighbours = _gridRepository.neighbourMap[gridItem.gridPosition];
            }
        }
    }
}