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
    [SerializeField] GameManager manager;

    void Start()
    {
        selfRenderer = GetComponent<SpriteRenderer>();
        manager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // update own looks
        if(isPressed)
            selfRenderer.sprite = pressed;
        else
            selfRenderer.sprite = unpressed;
        

        // update door
        DoorAndButton_Door doorDoor = door.GetComponent<DoorAndButton_Door>(); // I have the best variable naming skills
        if(doorDoor.flipFunction)
            door.GetComponent<DoorAndButton_Door>().isOpened = !isPressed;
        else
            door.GetComponent<DoorAndButton_Door>().isOpened = isPressed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isPressed = true;
        GetComponent<AudioSource>().clip = manager.RandomFromArray(manager.clickPositiveSounds);
        GetComponent<AudioSource>().Play();
    }


    void OnTriggerExit2D(Collider2D collider)
    {
        if(type == buttonType.press)
        {
            isPressed = false;
            manager.RandomFromArray(manager.clickPositiveSounds);
            GetComponent<AudioSource>().Play();
        }
        else if(type == buttonType.timed)
            StartCoroutine(TimedDoorControl());
        
    }

    IEnumerator TimedDoorControl()
    {
        yield return new WaitForSeconds(timedHoldTime);
        isPressed = false;
        manager.RandomFromArray(manager.clickPositiveSounds);
        GetComponent<AudioSource>().Play();
    }

    
}
