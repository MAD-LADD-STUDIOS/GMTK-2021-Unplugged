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

    void Start()
    {

    }


    void Update()
    {
        if (moving)
        {
            this.transform.position = Vector3.LerpUnclamped(currentPos, newPos, currentTime/moveTime);
            currentTime += Time.deltaTime;

            if (currentTime >= moveTime) moving = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W) && !Physics2D.OverlapCircle(this.transform.position + Vector3.up, .2f))
            {
                newPos = this.transform.position + Vector3.up;
                move();

            }

            if (Input.GetKeyDown(KeyCode.A) && !Physics2D.OverlapCircle(this.transform.position + Vector3.left, .2f))
            {
                newPos = this.transform.position + Vector3.left;
                move();
            }

            if (Input.GetKeyDown(KeyCode.S) && !Physics2D.OverlapCircle(this.transform.position + Vector3.down, .2f))
            {
                newPos = this.transform.position + Vector3.down;
                move();
            }

            if (Input.GetKeyDown(KeyCode.D) && !Physics2D.OverlapCircle(this.transform.position + Vector3.right, .2f))
            {
                newPos = this.transform.position + Vector3.right;
                move();
            }
        }
    }

    void move()
    {
        currentPos = this.transform.position;
        currentTime = 0.0f;
        moving = true;
    }

}
