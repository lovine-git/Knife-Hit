using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int[] spearsPerLevel;
    private int currentLevelIndex = 0;
    private int spearCount;

    [Header("Spear Spawning")]
    [SerializeField] private Vector2 spearSpawnPosition;
    [SerializeField] private GameObject spearObject;

    [Header("UI References")]
    public Button playButton;
    public int currentScore = 0;

    public GameUI GameUI { get; private set; }
    public bool IsGameActive { get; private set; } = true;

    private void Awake()
    {
        Instance = this;
        GameUI = GetComponent<GameUI>();
    }

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Current Scene: " + sceneName);

        if (sceneName == "Home Scene")
        {
            if (playButton != null)
            {
                playButton.onClick.RemoveAllListeners();
                playButton.onClick.AddListener(StartGame);
            }
            else
            {
                Debug.LogWarning("Play Button not assigned in Home Scene!");
            }
        }
        else if (sceneName == "Game")
        {
            IsGameActive = true;
            Time.timeScale = 1;
            SetupLevel();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void SetupLevel()
    {
        if (currentLevelIndex >= spearsPerLevel.Length) return;

        spearCount = spearsPerLevel[currentLevelIndex];

        if (GameUI != null)
        {
            GameUI.setInitialDisplaySpearCount(spearCount);
            GameUI.UpdateStageText(currentLevelIndex + 1);
        }

   

        SpawnSpear();
    }

    public void OnSuccessfulSpearHit()
    {
        currentScore += 1;

        if (GameUI != null)
        {
            GameUI.UpdateScoreUI(currentScore);
        }

        if (spearCount > 0)
        {
            SpawnSpear();
        }
        else
        {
            StartCoroutine(HandleLevelTransition());
        }
    }

    private IEnumerator HandleLevelTransition()
    {
        yield return new WaitForSeconds(1.5f);
        currentLevelIndex++;

        if (currentLevelIndex < spearsPerLevel.Length)
        {
            TargetRotator target = Object.FindAnyObjectByType<TargetRotator>();
            if (target != null) target.NextLevel();

            GameUI.UpdateStageText(currentLevelIndex + 1);
            SetupLevel();
        }
        else
        {
            StartGameOverSequence(true);
        }
    }

    private void SpawnSpear()
    {
        if (!IsGameActive) return;
        spearCount--;
        Instantiate(spearObject, spearSpawnPosition, Quaternion.identity);
    }

    public void StartGameOverSequence(bool win) { StartCoroutine(GameOverSequenceCoroutine(win)); }

    private IEnumerator GameOverSequenceCoroutine(bool win)
    {
        IsGameActive = false;

       
        int savedScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > savedScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

       
        int currentStage = currentLevelIndex + 1;
        int savedStage = PlayerPrefs.GetInt("HighStage", 1);
        if (currentStage > savedStage)
        {
            PlayerPrefs.SetInt("HighStage", currentStage);
        }

        PlayerPrefs.Save(); 

        if (win)
        {
            yield return new WaitForSeconds(0.3f);
            restartGame();
        }
        else
        {
            GameUI.showrestartButton();
        }
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}