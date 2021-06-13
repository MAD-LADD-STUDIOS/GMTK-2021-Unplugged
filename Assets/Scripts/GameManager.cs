using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator leftSideCoverAnimator;
    [SerializeField] Animator rightSideCoverAnimator;
    [Space]
    [SerializeField] Sprite[] bluePlayerDiedSprites;

    [SerializeField] Sprite[] redPlayerDiedSprites;
    bool isOriginalGameManager;
    int lastDeadPlayer = -1;
    [HideInInspector] public int playersAtEnd = 0;
    void Start()
    {
        if(FindObjectsOfType<GameManager>().Length > 1 && !isOriginalGameManager)
            Destroy(this.gameObject);
        else
        {
            isOriginalGameManager = true;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        leftSideCoverAnimator = Camera.main.transform.Find("ScreenCoverLeft").GetComponent<Animator>();
        rightSideCoverAnimator = Camera.main.transform.Find("ScreenCoverRight").GetComponent<Animator>();
        
        foreach (var item in FindObjectsOfType<PlayerDiedObjectScript>())
        {            
            if(item.playerDiedIndex == 0)
                item.objectLocCopy = leftSideCoverAnimator.transform;
            else if(item.playerDiedIndex == 1)
                item.objectLocCopy = rightSideCoverAnimator.transform;
        }
        

        StartCoroutine(destroyPlayerDiedObject());
    }
    public void LoadScene(int buildIndex)
    {
        leftSideCoverAnimator.SetBool("isLoading", true);
        rightSideCoverAnimator.SetBool("isLoading", true);
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
            brokenDownSprite.GetComponent<SpriteRenderer>().sortingOrder = 10;
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
