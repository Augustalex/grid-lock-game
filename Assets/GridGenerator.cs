using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public EndGameManager endGameManager;
    public GridRepository gridRepository;
    public GridSettings gridSettings;

    private Cell[][] _cells = new Cell[][]
    {
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = false},
            new Cell {blocker = true},
        },
        new[]
        {
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = true},
            new Cell {blocker = false},
            new Cell {blocker = true},
            new Cell {blocker = true},
        }
    };

    private bool _hasGrid;
    private bool _generatingGrid;

    public struct Cell
    {
        public bool blocker;
    }

    void Start()
    {
        if (HasNoGrid())
        {
            Generate();
        }
    }

    public void Generate()
    {
        if (_generatingGrid) return;

        _generatingGrid = true;
        StartCoroutine(GenerateRoutine());
    }

    public IEnumerator GenerateRoutine()
    {
        for (int y = 0; y < gridSettings.gridSize; y++)
        {
            for (int x = 0; x < gridSettings.gridSize; x++)
            {
                var position = new Vector2(x, y) - Vector2.one * gridSettings.gridSize * .5f;
                var gridItemGO = Instantiate(gridSettings.gridTemplate, position, Quaternion.identity, transform);
                var itemComponent = gridItemGO.GetComponent<GridItem>();
                itemComponent.endGameManager = endGameManager;
                itemComponent.SetPosition(position);

                var cell = _cells[y][x];
                if (cell.blocker)
                {
                    itemComponent.AddBlocker();
                }

                gridRepository.Register(itemComponent, position);

                yield return new WaitForFixedUpdate();
            }
        }

        gridRepository.Lock();

        _generatingGrid = false;
        _hasGrid = true;
    }

    public bool HasNoGrid()
    {
        return !_hasGrid;
    }

    public void ZeroGrid()
    {
        if (_generatingGrid) return;

        gridRepository.Clean();
        _hasGrid = false;
    }
}