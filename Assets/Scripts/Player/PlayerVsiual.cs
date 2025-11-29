using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_RUNNING = "IsRunning";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (animator == null || Player.Instance == null) return;
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        RotateToMouse();
    }

    private void RotateToMouse()
    {
        if (GameInput.Instance == null || Camera.main == null) return;
        
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(GameInput.Instance.GetMousePosition());
        mouseWorldPos.z = 0; 

        
        Vector3 direction = mouseWorldPos - transform.position;

        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
