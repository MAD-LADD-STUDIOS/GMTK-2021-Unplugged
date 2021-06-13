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
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && ((type == 0 && collider.gameObject.name == "PlayerRight") || (type == 1 && collider.gameObject.name == "PlayerLeft")))
        {
            manager.playersAtEnd ++;
            if(manager.playersAtEnd == 2)
                manager.LoadScene(nextLevelID);
            
        }
    }

    void OnTiggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            manager.playersAtEnd --;
        }
    }
}
