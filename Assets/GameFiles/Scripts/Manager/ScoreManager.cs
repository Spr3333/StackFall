using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        AddScore(0);
    }

    private void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            scoreText.text = score.ToString();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", score);
        scoreText.text = score.ToString();

    }

    public void ResetScore()
    {
        score = 0;
    }
}
