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
    bool isPluggedIn;
    int currentMovementDir; // UDLR (0123)
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
                float controllerSensitivity = 0.6f; // todo move to settings
                if (Input.GetAxisRaw("Vertical") > controllerSensitivity && isPluggedDirectionAvailable(0))
                {
                    newPos = this.transform.position + Vector3.up;
                    move();
                    if(!isPluggedIn)
                    {
                        selfSpriteRenderer.sprite = directionalSprites[0];
                        currentMovementDir = 0;
                    }
                    else
                        isPluggedIn = false;
                }

                if (Input.GetAxisRaw("Horizontal") < -controllerSensitivity && isPluggedDirectionAvailable(2))
                {
                    newPos = this.transform.position + Vector3.left;
                    move();
                    if(!isPluggedIn)
                    {
                        selfSpriteRenderer.sprite = directionalSprites[2];
                        currentMovementDir = 2;
                    }
                    else
                        isPluggedIn = false;
                }

                if (Input.GetAxisRaw("Vertical") < -controllerSensitivity && isPluggedDirectionAvailable(1))
                {
                    newPos = this.transform.position + Vector3.down;
                    move();
                    if(!isPluggedIn)
                    {
                        selfSpriteRenderer.sprite = directionalSprites[1];
                        currentMovementDir = 1;
                    }
                    else
                        isPluggedIn = false;
                }

                if (Input.GetAxisRaw("Horizontal") > controllerSensitivity && isPluggedDirectionAvailable(3))
                {
                    newPos = this.transform.position + Vector3.right;
                    move();
                    if(!isPluggedIn)
                    {
                        selfSpriteRenderer.sprite = directionalSprites[3];
                        currentMovementDir = 3;
                    }
                    else
                        isPluggedIn = false;
                }
            
        }
    }

    void move()
    {
        currentPos = this.transform.position;
        currentTime = 0.0f;
        moving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.isTrigger || collision.CompareTag("Player"))
        {
            moving = false;
            this.transform.position = currentPos;
        }
    }

    bool isPluggedDirectionAvailable(int dir)
    {
        if(isPluggedIn)
            return (currentMovementDir == 0 && dir == 1) || (currentMovementDir == 1 && dir == 0) || (currentMovementDir == 2 && dir == 3) || (currentMovementDir == 3 && dir == 2);
        else 
            return true;
    }

    public void OnPlug()
    {
        moving = false;
        // transform.position = newPos;
        if(currentMovementDir == 0)
        {
            transform.position += Vector3.down * 0.7f;
            currentPos = transform.position + Vector3.down;
            
        }
        else if(currentMovementDir == 1)
        {
            transform.position += Vector3.up * 0.7f;
            currentPos = transform.position + Vector3.up;
        }
        else if(currentMovementDir == 2)
        {
            transform.position += Vector3.right * 0.7f;
            currentPos = transform.position + Vector3.right;
        }
        else if(currentMovementDir == 3)
        {
            transform.position += Vector3.left * 0.7f;
            currentPos = transform.position + Vector3.left;
        }
        
        isPluggedIn = true;
    }

        
    
}
