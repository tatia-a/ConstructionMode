using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CursorController : MonoBehaviour
{
    private PlayerInputActions input; 

    [Inject]
    public void Construct(PlayerInputActions input)
    {
        this.input = input;
    }
    private void Awake()
    {
        input.Player.UnlockCursor.started += OnAltPressed; 
        input.Player.UnlockCursor.canceled += OnAltReleased; 
    }

    private void Start()
    {
        LockCursor(); 
    }

    private void OnAltPressed(InputAction.CallbackContext context) => UnlockCursor();

    private void OnAltReleased(InputAction.CallbackContext context) => LockCursor();

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        input.Player.UnlockCursor.started -= OnAltPressed;
        input.Player.UnlockCursor.canceled -= OnAltReleased;
    }
}
