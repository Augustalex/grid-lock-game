using System;
using UnityEngine;

public class FactoryCreator : MonoBehaviour
{
    private int _blocks = 0;

    public event Action<int> ConsumedBlock;

    public void TryCreate(GridItem gridItem, Vector2 direction)
    {
        var fourWayDirection = RoundVector(direction);
        if (fourWayDirection != Vector2.zero)
        {
            gridItem.MarkAsFactory(fourWayDirection);

            IncreaseCounter();
        }
    }

    private void IncreaseCounter()
    {
        _blocks += 1;
        ConsumedBlock?.Invoke(_blocks);
    }

    private Vector2 RoundVector(Vector2 vector)
    {
        return new Vector2(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y)
        );
    }
}