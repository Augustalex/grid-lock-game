using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class GridRepository : MonoBehaviour
{
    public GridSettings gridSettings;
    public Dictionary<Vector2, GridItem> grid = new Dictionary<Vector2, GridItem>();
    public GridItem[] gridItems = new GridItem[] { };
    public readonly Dictionary<Vector2, GridItem[]> neighbourMap = new Dictionary<Vector2, GridItem[]>();

    public void Register(GridItem gridItem, Vector2 gridPosition)
    {
        grid.Add(gridPosition, gridItem);
    }

    public void Lock()
    {
        gridItems = grid.Values.ToArray();

        for (int x = 0; x < gridSettings.gridSize; x++)
        {
            for (int y = 0; y < gridSettings.gridSize; y++)
            {
                var position = new Vector2(x, y);
                var offsets = Neighbours(position);

                var neighbours = new List<GridItem>();
                foreach (var offset in offsets)
                {
                    if (grid.ContainsKey(offset))
                    {
                        neighbours.Add(grid[offset]);
                    }
                }

                neighbourMap[position] = neighbours.ToArray();
            }
        }
    }

    public GridItem GetInDirection(Vector2 center, Vector2 direction)
    {
        var key = center + direction;
        if (grid.ContainsKey(key)) return grid[key];
        return null;
    }

    public GridItem GetRight(Vector2 center)
    {
        var key = center + Vector2.right;
        if (grid.ContainsKey(key)) return grid[key];
        return null;
    }

    public GridItem GetDown(Vector2 center)
    {
        var key = center + Vector2.down;
        if (grid.ContainsKey(key)) return grid[key];
        return null;
    }

    private Vector2[] Neighbours(Vector2 origin)
    {
        return new[]
        {
            origin + new Vector2(-1f, -1f),
            origin + new Vector2(0f, -1f),
            origin + new Vector2(1f, -1f),
            origin + new Vector2(-1f, 0f),
            origin + new Vector2(1f, 0f),
            origin + new Vector2(-1f, 1f),
            origin + new Vector2(0f, 1f),
            origin + new Vector2(1f, 1f),
        };
    }

    public bool TryGet(Vector2 gridPosition, out GridItem gridItem)
    {
        return grid.TryGetValue(gridPosition, out gridItem);
    }

    public void Clean()
    {
        grid.Clear();
        gridItems = new GridItem[] { };
        neighbourMap.Clear();
    }
}