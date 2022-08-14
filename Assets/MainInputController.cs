using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainInputController : MonoBehaviour
{
    public Clicker clicker;
    public GridGenerator gridGenerator;
    public UIController uiController;

    private Vector2 _position;
    private Vector2 _down;

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (gridGenerator.HasNoGrid())
        {
            uiController.Reset();
            gridGenerator.Generate();
        }
        else
        {
            if (context.performed)
            {
                _down = _position;
            }
            else if (context.canceled)
            {
                clicker.Select(_down, _position);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _position = context.ReadValue<Vector2>();
    }
}