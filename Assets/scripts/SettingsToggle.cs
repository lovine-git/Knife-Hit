using UnityEngine;
using UnityEngine.UI;

public class SettingsToggle : MonoBehaviour
{
    [Header("UI Button Objects")]
    public GameObject onButton;
    public GameObject offButton;

    private bool isSoundOn = true;

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;

        onButton.SetActive(isSoundOn);
        offButton.SetActive(!isSoundOn);


        AudioListener.pause = !isSoundOn;

        Debug.Log("Sound is now: " + (isSoundOn ? "ON" : "OFF"));
    }
}