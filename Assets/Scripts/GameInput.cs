using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        // Singleton safety
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    public Vector2 GetMovementVector()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector3 GetMousePosition()
    {
        return Mouse.current.position.ReadValue();
    }
}
