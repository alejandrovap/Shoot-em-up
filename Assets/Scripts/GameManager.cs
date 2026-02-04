using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    const int INITIAL_LIVES = 3;
    const string PREF_MAX_SCORE = "MaxScoreKey";

    [SerializeField] int scoreToGainLife = 300;
    private int nextLifeThreshold;

    int lives = INITIAL_LIVES;
    int score = 0;
    int maxScore = 0;

    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] TextMeshProUGUI txtMaxScore;
    [SerializeField] TextMeshProUGUI txtMessage;

    // El tamaño de este array define el máximo de vidas
    [SerializeField] GameObject[] imgLives;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        maxScore = PlayerPrefs.GetInt(PREF_MAX_SCORE, 0);

        nextLifeThreshold = scoreToGainLife;
    }

    void Start()
    {
        if (txtMessage != null) txtMessage.gameObject.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (lives <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space)) RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMaxScore();
        }
    }

    public static GameManager GetInstance() => instance;

    public void LoseLife()
    {
        if (lives > 0)
        {
            lives--;
            UpdateUI();
            if (lives <= 0) GameOver();
        }
    }

    public void GainLife()
    {
        // Si lives es 3, 3 < 3 es falso, por lo tanto NO entra y no suma vida.
        // Si lives es 2, 2 < 3 es verdadero, entra y suma vida.
        if (imgLives != null && lives < imgLives.Length)
        {
            lives++;
            UpdateUI();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        if (score >= nextLifeThreshold)
        {
            GainLife();

            nextLifeThreshold += scoreToGainLife;
        }

        if (score > maxScore)
        {
            maxScore = score;
            PlayerPrefs.SetInt(PREF_MAX_SCORE, maxScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (txtScore != null) txtScore.text = string.Format("{0,4:D4}", score);
        if (txtMaxScore != null) txtMaxScore.text = string.Format("{0,4:D4}", maxScore);

        if (imgLives != null)
        {
            for (int i = 0; i < imgLives.Length; i++)
            {
                if (imgLives[i] != null) imgLives[i].SetActive(i < lives);
            }
        }
    }

    void GameOver()
    {
        if (txtMessage != null) txtMessage.gameObject.SetActive(true);
        Time.timeScale = 0f;
        DestroyAllWithTag("Enemy");
        DestroyAllWithTag("Shoot");
        DestroyAllWithTag("Asteroid");
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

    void ResetMaxScore()
    {
        maxScore = 0;
        PlayerPrefs.DeleteKey(PREF_MAX_SCORE);
        PlayerPrefs.Save();
        UpdateUI();
    }
}