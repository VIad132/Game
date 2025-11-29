using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public float score = 0f;
    public float scorePerSecond = 1f; // скільки балів додається за секунду

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        AddScore(Time.deltaTime * scorePerSecond); // бали за час
    }

    // Метод нарахування балів
    public void AddScore(float amount)
    {
        score += amount;
    }

    // Метод для округленого значення (для UI)
    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }
}
