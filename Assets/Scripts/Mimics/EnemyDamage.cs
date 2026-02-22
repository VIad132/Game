using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1f;
    private float nextAttackTime;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < nextAttackTime) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
