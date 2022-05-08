using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{

    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject Char;
    [SerializeField] private Texture2D texture2D;
    [SerializeField] private Slider backVolSlider, effectVolSlider;
    private Vector3 spawnPos;


    public void chardied(Vector3 checkPos)
    {
        spawnPos = checkPos;
        StartCoroutine("movechar");
        
    }

    

    private void Awake()
    {
       Time.timeScale = 1;
       Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void Start()
    {
        Vector2 cursorHotspot = new Vector2(texture2D.width / 2, texture2D.height / 2);
        // Cursor.SetCursor(texture2D,Vector2.zero,CursorMode.ForceSoftware);
        Cursor.SetCursor(texture2D, cursorHotspot, CursorMode.ForceSoftware);

        backVolSlider.value = PlayerPrefs.GetFloat("BackVol",.7f);
        effectVolSlider.value = PlayerPrefs.GetFloat("EffectVol",1f);


    }

    IEnumerator movechar()
    {       
        Char.SetActive(false);
        yield return new WaitForSeconds(.5f);
        Char.transform.position = spawnPos;
        Char.SetActive(true);
    }


    public void OpenMenu()
    {
        if (MenuPanel.activeSelf || SettingsPanel.activeSelf)
        {
            MenuPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            Time.timeScale = 1;

        }
        else if(!MenuPanel.activeSelf && !SettingsPanel.activeSelf)
        {
            MenuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void toggleSettings()
    {
        if (MenuPanel.activeSelf && !SettingsPanel.activeSelf)
        {
            MenuPanel.SetActive(false);
            SettingsPanel.SetActive(true);
            Time.timeScale =0;
        }
        else if(!MenuPanel.activeSelf && SettingsPanel.activeSelf)
        {
            MenuPanel.SetActive(true);
            SettingsPanel.SetActive(false);
            Time.timeScale = 0;
        }
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void exit()
    {
        SceneManager.LoadScene(0);
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
