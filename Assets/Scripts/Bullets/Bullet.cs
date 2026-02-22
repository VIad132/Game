using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 15;
    public float lifeTime = 2f;

    private Vector2 direction;

    private void OnEnable()
    {
        Invoke(nameof(Disable), lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction; // щоб пуля дивилась куди летить
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
        if (enemy != null)
        {
            Debug.Log("Bullet hit enemy");
            enemy.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    void Disable()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }
}
