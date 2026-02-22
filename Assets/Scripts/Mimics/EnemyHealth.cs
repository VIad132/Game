using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHP = 30;
    public int scoreForKill = 5; 
    private int hp;

    private void OnEnable()
    {
        hp = maxHP;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("Enemy HP: " + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Звертаємося до синглтона ScoreManager і додаємо очки
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreForKill);
        }

        gameObject.SetActive(false); // Повернення в пул
    }
}