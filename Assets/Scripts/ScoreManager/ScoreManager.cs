using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Settings")]
    public TextMeshProUGUI scoreText; // Сюди перетягніть об'єкт тексту в Unity
    private int score = 0;
    private float timer = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Додаємо 1 очко щосекунди
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            AddScore(1);
            timer = 0f;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public int GetScore()
    {
        return score;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}