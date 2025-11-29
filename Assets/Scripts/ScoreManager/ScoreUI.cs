using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (scoreText == null || ScoreManager.Instance == null) return;
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore();
    }
}
