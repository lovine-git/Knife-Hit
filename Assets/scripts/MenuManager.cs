using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;

    void Start()
    {
        if (playButton != null)
            playButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        
        SceneManager.LoadScene("Game");
    }

 
    public void QuitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
            // This runs in your actual WebGL/itch.io build
            Application.Quit();
#endif
    }
}