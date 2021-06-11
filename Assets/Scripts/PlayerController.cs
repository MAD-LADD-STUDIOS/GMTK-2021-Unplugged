using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerLeft;
    [SerializeField] GameObject playerRight;
    [SerializeField] LineRenderer Cord;
    [SerializeField] Vector2 pos;

    [SerializeField] Vector2 clampX; // min, max
    [SerializeField] Vector2 clampY; // min, max
    // scale: 7x7 per grid (atm)
    // middle: 4, 4

    void Start()
    {
        pos = new Vector2(4, 4);
    }
    void Update()
    {
        var horizontal = Input.GetKeyDown(KeyCode.A) ? -1 : Input.GetKeyDown(KeyCode.D) ? 1 : 0;
        var vertical = Input.GetKeyDown(KeyCode.S) ? -1 : Input.GetKeyDown(KeyCode.W) ? 1 : 0;

        if(Mathf.Abs(horizontal) < 0.5f)
            horizontal = 0;
        else
            horizontal = horizontal / Mathf.Abs(horizontal); // math magic
        
        if(Mathf.Abs(vertical) < 0.5f)
            vertical = 0;
        else
            vertical = vertical / Mathf.Abs(vertical); // math magic
        
        if(!(pos.x + horizontal >= clampX.y || pos.x + horizontal < clampX.x))
        {
            playerLeft.transform.position += Vector3.right * horizontal;
            playerRight.transform.position += Vector3.right * horizontal;
            pos += Vector2.right * horizontal;
            UpdateCord();
        }

        if(!(pos.y + vertical >= clampY.y || pos.y + vertical <= clampY.x))
        {
            playerLeft.transform.position += Vector3.up * vertical;
            playerRight.transform.position += Vector3.up * vertical;
            pos += Vector2.up * vertical;
            UpdateCord();
        }
    }


    void UpdateCord()
    {
        Cord.SetPosition(0, playerLeft.transform.position);
        Cord.SetPosition(1, playerRight.transform.position);
    }
}
