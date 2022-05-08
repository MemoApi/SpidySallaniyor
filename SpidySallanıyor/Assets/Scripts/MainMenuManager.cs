using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{


    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private Texture2D texture2D;
    [SerializeField] private Slider backVolSlider, effectVolSlider;


    private void Awake()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        Vector2 cursorHotspot = new Vector2(texture2D.width / 2, texture2D.height / 2);
        // Cursor.SetCursor(texture2D,Vector2.zero,CursorMode.ForceSoftware);
        Cursor.SetCursor(texture2D, cursorHotspot, CursorMode.ForceSoftware);

        backVolSlider.value = PlayerPrefs.GetFloat("BackVol", .7f);
        effectVolSlider.value = PlayerPrefs.GetFloat("EffectVol", 1f);


    }


    public void toggleSettings()
    {
        if (MenuPanel.activeSelf && !SettingsPanel.activeSelf)
        {
            MenuPanel.SetActive(false);
            SettingsPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (!MenuPanel.activeSelf && SettingsPanel.activeSelf)
        {
            MenuPanel.SetActive(true);
            SettingsPanel.SetActive(false);
            Time.timeScale = 0;
        }
    }


    public void exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void backVol(float vol)
    {
        FindObjectOfType<SoundManagerScript>().backVolume(vol);
    }
    public void effectVol(float vol)
    {
        FindObjectOfType<SoundManagerScript>().effectVolume(vol);
    }


}
