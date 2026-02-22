using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 inputVector;

    private bool isRunning;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        if (GameInput.Instance == null) return;
        inputVector = GameInput.Instance.GetMovementVector().normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = inputVector * movingSpeed;
        isRunning = inputVector.sqrMagnitude > 0.01f;
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
