using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum buttonType {hold, press, timed}; 
// hold:    once pressed, it will stay pressed
// press:   once pressed, will turn off once all objects are lifted
// timed:   once pressed, will remain on for a certain amount of time
public class DoorAndButton_Button : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Sprite unpressed;
    [SerializeField] Sprite pressed;
    [SerializeField] SpriteRenderer selfRenderer;
    [SerializeField] float timedHoldTime;
    [SerializeField] buttonType type;
    [SerializeField] bool isPressed;

    void Start()
    {
        selfRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // update own looks
        if(isPressed)
            selfRenderer.sprite = pressed;
        else
            selfRenderer.sprite = unpressed;
        

        // update door
        door.GetComponent<DoorAndButton_Door>().isOpened = isPressed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPressed = true;
    }


    void OnTriggerExit2D(Collider2D collider)
    {
        if(type == buttonType.press)
            isPressed = false;
        else if(type == buttonType.timed)
            StartCoroutine(TimedDoorControl());
        
    }

    IEnumerator TimedDoorControl()
    {
        yield return new WaitForSeconds(timedHoldTime);
        isPressed = false;
    }

    
}
