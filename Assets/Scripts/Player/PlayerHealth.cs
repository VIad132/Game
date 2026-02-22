using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("PLAYER HP: " + currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
    }
}