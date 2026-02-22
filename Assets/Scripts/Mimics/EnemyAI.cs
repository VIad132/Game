using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    private void OnEnable()
    {
        if (Player.Instance != null)
            player = Player.Instance.transform;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    PlayerHealth player = other.GetComponent<PlayerHealth>();
       if (player != null)
        {
           Debug.Log("Enemy hit player");
           player.TakeDamage(10);
        }
    }

}
