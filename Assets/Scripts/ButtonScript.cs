using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public enum buttonMode {mainMenu, settingsMenu};
public class ButtonScript : MonoBehaviour
{
    public buttonMode mode;
    [Header ("Main Menu")]
    public GameObject credits;

    [Header ("Settings")]
    [SerializeField] Toggle lightFlash;
    [SerializeField] Toggle postProcess;
    [SerializeField] Toggle showBakground;
    [SerializeField] Slider musicLevel;
    [SerializeField] Slider SFXLevel;

    void Start()
    {
        // settings scene loaded
        if(mode == buttonMode.settingsMenu)
        {
            string settingsPath = $"{Application.persistentDataPath}/settings.json";
            if(File.Exists(settingsPath))
            {
                var savedSettings = JsonUtility.FromJson<Settings>(File.ReadAllText(settingsPath));
                lightFlash.isOn = savedSettings._enableLightFlashing;
                postProcess.isOn = savedSettings._enablePostProcess;
                showBakground.isOn = savedSettings._showBackground;

                musicLevel.value = savedSettings._musicLevel;
                SFXLevel.value = savedSettings._SFXLevel;
            }
            else
            {
                var t = File.Create(settingsPath);
                t.Close();  
            }
        }
    }

    public void LoadScene(int id)
    {
        FindObjectOfType<GameManager>().LoadScene(id);
    }

    public void toggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }

    public void LoadSceneAdditive(int ID)
    {
        SceneManager.LoadScene(ID, LoadSceneMode.Additive);
    }
    
    public void CloseSettingsPanel()
    {
        GetComponent<Animator>().SetBool("goOffScreen", true);
        StartCoroutine(WaitHalfASecondThenCloseSettingsPanel());
    }

    public void SaveAndQuit()
    {
        // check if settings.json is already there
        string settingsPath = $"{Application.persistentDataPath}/settings.json";
        if(!File.Exists(settingsPath))
        {
            var t = File.Create(settingsPath);
            t.Close();
        }
        
        var settingsData = new Settings();
        settingsData._enableLightFlashing = lightFlash.isOn;
        settingsData._enablePostProcess = postProcess.isOn;
        settingsData._showBackground = showBakground.isOn;

        settingsData._musicLevel = musicLevel.value;
        settingsData._SFXLevel = SFXLevel.value;

        File.WriteAllText(settingsPath, JsonUtility.ToJson(settingsData));

        CloseSettingsPanel();

        FindObjectOfType<GameManager>().UpdateScreenFromSettings(settingsData);
    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator WaitHalfASecondThenCloseSettingsPanel() // I have no clue how to do this in any other way ok
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.UnloadSceneAsync(8);
    }
    
}
