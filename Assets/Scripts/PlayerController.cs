using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveTime = 1.0f;
    float currentTime = 0.0f;
    Vector3 currentPos;
    Vector3 newPos;
    bool moving = false;

    [SerializeField] Sprite[] directionalSprites; // UDLR
    SpriteRenderer selfSpriteRenderer;
    void Start()
    {
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (moving)
        {
            
            this.transform.position = Vector3.LerpUnclamped(currentPos, newPos, currentTime/moveTime);
            currentTime += Time.deltaTime;

            if (currentTime >= moveTime) 
            {
                moving = false;
                this.transform.position = new Vector3(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y), 0);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) && isWalkable(this.transform.position + Vector3.up))
            {
                newPos = this.transform.position + Vector3.up;
                move();
                selfSpriteRenderer.sprite = directionalSprites[0];
            }

            if (Input.GetKeyDown(KeyCode.A) && isWalkable(this.transform.position + Vector3.left))
            {
                newPos = this.transform.position + Vector3.left;
                move();
                selfSpriteRenderer.sprite = directionalSprites[2];
            }

            if (Input.GetKeyDown(KeyCode.S) && isWalkable(this.transform.position + Vector3.down))
            {
                newPos = this.transform.position + Vector3.down;
                move();
                selfSpriteRenderer.sprite = directionalSprites[1];
            }

            if (Input.GetKeyDown(KeyCode.D) && isWalkable(this.transform.position + Vector3.right))
            {
                newPos = this.transform.position + Vector3.right;
                move();
                selfSpriteRenderer.sprite = directionalSprites[3];
            }
        }
    }

    void move()
    {
        currentPos = this.transform.position;
        currentTime = 0.0f;
        moving = true;
    }


    bool isWalkable(Vector3 area) // yes you can walk into the void mhm makes sense
    {
        var res = Physics2D.OverlapCircle(area, 0.2f);
        if(res == null)
            return true;
        else if(res.CompareTag("Void"))
            return true;
        else
            return false;
    }

}
