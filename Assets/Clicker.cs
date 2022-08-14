using UnityEngine;

public class Clicker : MonoBehaviour
{
    private Camera _camera;
    public FactoryCreator factoryCreator;

    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    public void Select(Vector2 down, Vector2 up)
    {
        var ray = _camera.ScreenPointToRay(down);
        if (Physics.Raycast(ray, out var hit, 100f))
        {
            var gridItem = hit.collider.GetComponent<GridItem>();
            if (gridItem)
            {
                var direction = (up - down).normalized;
                factoryCreator.TryCreate(gridItem, direction);
            }
        }
    }
}