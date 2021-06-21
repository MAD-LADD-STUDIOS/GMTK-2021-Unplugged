using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header ("Scene Transition")]
        [SerializeField] Animator leftSideCoverAnimator;
        [SerializeField] Animator rightSideCoverAnimator;
    [Header ("Death Screen")]
    [SerializeField] Sprite[] bluePlayerDiedSprites;

    [SerializeField] Sprite[] redPlayerDiedSprites;
    bool isOriginalGameManager;
    int lastDeadPlayer = -1;
    [HideInInspector] public int playersAtEnd = 0;
    [Header ("SFX")]
    public AudioClip[] clickPositiveSounds;
    public AudioClip[] clickNegativeSounds;
    [Header ("Settings")]
    public Settings settings;
    [SerializeField] GameObject bg;
    [Header ("Timer")]
    [SerializeField] float time;
    

    void Awake()
    {
        if(FindObjectsOfType<GameManager>().Length > 1 && !isOriginalGameManager)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            isOriginalGameManager = true;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        bg = GameObject.Find("ComputerBackground");
        string settingsPath = Application.persistentDataPath + "/settings.json";

        if(File.Exists(settingsPath))
        {
            settings = JsonUtility.FromJson<Settings>(File.ReadAllText(settingsPath));
            if(settings == null)
            {
                File.WriteAllText(settingsPath, JsonUtility.ToJson(new Settings()));
                settings = JsonUtility.FromJson<Settings>(File.ReadAllText(settingsPath));
            }
        }
        else
            settings = new Settings();
        
        UpdateScreenFromSettings(settings);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene(0);
        }

        if(SceneManager.GetActiveScene().name.StartsWith("Level"))
        {
            float seconds = (int)time;
            float minutes = (int)(seconds / 60);
            float milliseconds = time - (int)time;
            GameObject.Find("Timer").GetComponent<TMPro.TMP_Text>().text = $"{minutes}:{seconds};{milliseconds}";

            time += Time.deltaTime;
        }

        
    }

    public void UpdateScreenFromSettings(Settings newSettings)
    {
        settings = newSettings;
        
        bg.SetActive(settings._showBackground);


        foreach (var item in FindObjectsOfType<LightFlashController>())
        {
            item.flashEnabled = settings._enableLightFlashing;
        }
        Camera.main.GetComponent<Volume>().enabled = settings._enablePostProcess;

        foreach (var item in FindObjectsOfType<AudioSource>())
        {        
            item.volume = settings._SFXLevel;
        }


        transform.Find("Music").GetComponent<AudioSource>().volume = settings._musicLevel;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode != LoadSceneMode.Additive)
        {
            bg = GameObject.Find("ComputerBackground");
            UpdateScreenFromSettings(settings);
        }

        if(scene.name == "Level01")
            time = 0;
        
        leftSideCoverAnimator = Camera.main.transform.Find("ScreenCoverLeft").GetComponent<Animator>();
        rightSideCoverAnimator = Camera.main.transform.Find("ScreenCoverRight").GetComponent<Animator>();
        
        foreach (var item in FindObjectsOfType<PlayerDiedObjectScript>())
        {            
            if(item.playerDiedIndex == 0)
                item.objectLocCopy = leftSideCoverAnimator.transform;
            else if(item.playerDiedIndex == 1)
                item.objectLocCopy = rightSideCoverAnimator.transform;
        }
        
        GameObject.Find("Timer").SetActive(settings._showTimer);

        foreach (var item in FindObjectsOfType<PlayerController>())
        {
            item.moveTime = settings._playerMoveTime;
        }

        StartCoroutine(destroyPlayerDiedObject());
    }
    public void LoadScene(int buildIndex)
    {
        leftSideCoverAnimator.SetBool("isLoading", true);
        rightSideCoverAnimator.SetBool("isLoading", true);
        playersAtEnd = 0;
        StartCoroutine(loadSceneAfterSeconds(1, buildIndex));
    }

    public void ResetCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// param player:
    ///     0 - Blue
    ///     1 - Red
    /// </summary>
    public void PlayerDied(int player)
    {
        lastDeadPlayer = player;
        PlayerDiedAnimation(player);
        Invoke("ResetCurrentScene", 1);         
    }

    void PlayerDiedAnimation(int playerIndex)
    {
        foreach (var item in FindObjectsOfType<PlayerController>())
        {
            item.enabled = false;
        }
        var PlayerDiedObject = new GameObject();

        foreach (var item in (playerIndex == 0 ? bluePlayerDiedSprites : redPlayerDiedSprites))
        {
            var brokenDownSprite = new GameObject();
            brokenDownSprite.AddComponent<SpriteRenderer>();
            brokenDownSprite.GetComponent<SpriteRenderer>().sprite = item;
            brokenDownSprite.GetComponent<SpriteRenderer>().sortingOrder = 20;
            brokenDownSprite.transform.position = (Vector2.up * (Random.Range(-5, 5) + Random.value)) + ((playerIndex == 0 ? Vector2.left : Vector2.right) * (Random.Range(0, 5) + Random.value));
            brokenDownSprite.transform.eulerAngles = Vector3.forward * (Random.Range(0, 360));
            brokenDownSprite.transform.localScale = Vector3.one * 3;

            brokenDownSprite.transform.SetParent(PlayerDiedObject.transform, false);
        }


        PlayerDiedObject.transform.position = Vector3.right * (playerIndex == 0 ? -16 : 16);
        PlayerDiedObject.AddComponent<PlayerDiedObjectScript>();
        PlayerDiedObject.GetComponent<PlayerDiedObjectScript>().objectLocCopy = (playerIndex == 0 ? leftSideCoverAnimator.transform : rightSideCoverAnimator.transform);
        PlayerDiedObject.GetComponent<PlayerDiedObjectScript>().playerDiedIndex = playerIndex;

        DontDestroyOnLoad(PlayerDiedObject);

        if(playerIndex == 0)
            leftSideCoverAnimator.SetBool("isLoading", true);
        else
            rightSideCoverAnimator.SetBool("isLoading", true);
    }

    public AudioClip RandomFromArray(AudioClip[] array) // dont have time to make it more flexible
    {
        return array[Mathf.RoundToInt(Random.Range(0, array.Length))];
    }

    IEnumerator loadSceneAfterSeconds(float secs, int index)
    {
        yield return new WaitForSeconds(secs);
        SceneManager.LoadScene(index);
    }

    IEnumerator destroyPlayerDiedObject()
    {
        yield return new WaitForSeconds(0.6f);
        foreach (var item in FindObjectsOfType<PlayerDiedObjectScript>())
        {
            Destroy(item.gameObject);
        }
    }

}
