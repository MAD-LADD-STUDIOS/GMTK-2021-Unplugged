using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject playerLeft;
    [SerializeField] GameObject playerRight;
    [SerializeField] Vector2Int posLeft;
    [SerializeField] Vector2Int posRight;


    [SerializeField] Vector2 clampX; // min, max
    [SerializeField] Vector2 clampY; // min, max
    // scale: 7x7 per grid (atm)
    // middle: 4, 4

    void Start()
    {
        posLeft = new Vector2Int(4, 4);
        posRight = new Vector2Int(4, 4);
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
        
        /* NOTE
        refer to https://answers.unity.com/questions/383671/find-gameobject-at-position.html
        for solid walls in the future
        */

        // left player check
        if(!(posLeft.x + horizontal >= clampX.y || posLeft.x + horizontal < clampX.x))
        {
            playerLeft.transform.position += Vector3.right * horizontal;
            posLeft += Vector2Int.right * horizontal;
        }

        if(!(posLeft.y + vertical >= clampY.y || posLeft.y + vertical <= clampY.x))
        {
            playerLeft.transform.position += Vector3.up * vertical;
            posLeft += Vector2Int.up * vertical;
        }

        // right player check
        if(!(posRight.x + horizontal >= clampX.y || posRight.x + horizontal < clampX.x))
        {
            playerRight.transform.position += Vector3.right * horizontal;
            posRight += Vector2Int.right * horizontal;
        }

        if(!(posRight.y + vertical >= clampY.y || posRight.y + vertical <= clampY.x))
        {
            playerRight.transform.position += Vector3.up * vertical;
            posRight += Vector2Int.up * vertical;
        }

        // not very flexible grid size, TODO fix
        playerLeft.transform.position = (posLeft - (8 * Vector2.right) - (4* Vector2.up));
        playerRight.transform.position = (posRight - (4*Vector2.up));

    }
}
