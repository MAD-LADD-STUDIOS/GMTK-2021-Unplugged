using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public int nextLevelID;
    GameManager manager;
    [Header ("0 = red")]
    [Header ("1 = blue")]
    public int type;

    void Start()
    { 
        manager = FindObjectOfType<GameManager>();
        manager.playersAtEnd = 0;

        AudioSource tried;
        if(!TryGetComponent<AudioSource>(out tried))
            this.gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && ((type == 0 && collider.gameObject.name == "PlayerRight") || (type == 1 && collider.gameObject.name == "PlayerLeft")))
        {
            GetComponent<AudioSource>().clip = manager.RandomFromArray(manager.clickPositiveSounds);
            GetComponent<AudioSource>().Play();
            collider.transform.position = transform.position;
            collider.GetComponent<PlayerController>().OnPlug();
            manager.playersAtEnd ++;
            if(manager.playersAtEnd == 2)
                manager.LoadScene(nextLevelID);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<AudioSource>().clip = manager.RandomFromArray(manager.clickNegativeSounds);
            GetComponent<AudioSource>().Play();
            manager.playersAtEnd--;
        }
    }
}
