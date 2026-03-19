using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class GameUI : MonoBehaviour
{
    [Header("UI Panel References")]
    [SerializeField] private GameObject restartButton;

    [Header("Spear Icon Settings")]
    [SerializeField] private GameObject panelSpears;
    [SerializeField] private GameObject iconSpear;
    [SerializeField] private Color usedSpearIconColor;

    [Header("Stage Display")]
    [SerializeField] private TextMeshProUGUI stageText;

    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Home Page Stats")]
    
    [SerializeField] private TextMeshProUGUI homeHighScoreText;
    [SerializeField] private TextMeshProUGUI homeHighStageText;

    private int SpearIconIndexToChange = 0;

    private void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "Home Scene")
        {
            LoadHomeStats();
        }
    }

    private void LoadHomeStats()
    {
        
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int highStage = PlayerPrefs.GetInt("HighStage", 1);

       
        if (homeHighScoreText != null) homeHighScoreText.text = "SCORE " + highScore;
        if (homeHighStageText != null) homeHighStageText.text = "STAGE " + highStage;
    }

    public void GoToHome()
    {
        
        Time.timeScale = 1;

       
        SceneManager.LoadScene("Home Scene");
    }





    public void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void showrestartButton()
    {
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Restart Button (Panel) is not assigned!");
        }
    }

    public void setInitialDisplaySpearCount(int count)
    {
        foreach (Transform child in panelSpears.transform)
        {
            Destroy(child.gameObject);
        }

        SpearIconIndexToChange = 0;

        for (int i = 0; i < count; i++)
        {
            Instantiate(iconSpear, panelSpears.transform);
        }
    }

    public void DecrementDisplaySpearCount()
    {
        if (SpearIconIndexToChange < panelSpears.transform.childCount)
        {
            panelSpears.transform.GetChild(SpearIconIndexToChange++)
                .GetComponent<Image>().color = usedSpearIconColor;
        }
    }

    public void UpdateStageText(int stageNumber)
    {
        if (stageText != null)
        {
            stageText.text = "STAGE " + stageNumber;
        }
    }
}