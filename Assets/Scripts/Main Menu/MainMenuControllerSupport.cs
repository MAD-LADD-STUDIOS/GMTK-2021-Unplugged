using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControllerSupport : MonoBehaviour
{
    /*
        buttons[x][y]
    */
    [System.Serializable]
    public class buttonArray
    {
        public Button[] buttons;

        public Button this[int index]
        {
            get => buttons[index];
            set => buttons[index] = value;
        }

        public int Length {get => buttons.Length;}
    }
    [SerializeField] buttonArray[] buttons;

    Vector2Int highlightedButton;
    [SerializeField] Sprite highlightedButtonSprite;
    [SerializeField] Sprite notHighlightedButtonSprite;
    [SerializeField] Settings settings;
    bool isUsingController;
    [SerializeField] bool hasPressed; // prevent lightning speed scrolling and impossible selection problems
    void Start()
    {
        settings = FindObjectOfType<GameManager>().settings;
    }

    void Update()
    {
        bool oldPress = hasPressed;

        if(Input.GetAxisRaw("ControllerLeftX") < settings._controllerSens && 
           Input.GetAxisRaw("ControllerLeftY") < settings._controllerSens &&
           Input.GetAxisRaw("ControllerLeftX") > -settings._controllerSens &&
           Input.GetAxisRaw("ControllerLeftY") > -settings._controllerSens)
            hasPressed = false;
        else
            hasPressed = true;

        if(oldPress && hasPressed)
            return;

        var oldPos = highlightedButton;
        if(Input.GetAxisRaw("ControllerLeftX") > settings._controllerSens)
            highlightedButton += Vector2Int.right;
        else if(Input.GetAxisRaw("ControllerLeftX") < -settings._controllerSens)
            highlightedButton += Vector2Int.left;
        
        if(Input.GetAxisRaw("ControllerLeftY") > settings._controllerSens)
            highlightedButton += Vector2Int.up;
        else if(Input.GetAxisRaw("ControllerLeftY") < -settings._controllerSens)
            highlightedButton += Vector2Int.down;
        
        // wrap around
        if(highlightedButton.x > buttons.Length-1)
            highlightedButton = new Vector2Int(0, highlightedButton.y);
        else if(highlightedButton.x < 0)
            highlightedButton = new Vector2Int(buttons.Length-1, highlightedButton.y);
        
        if(highlightedButton.y > buttons[highlightedButton.x].Length-1)
            highlightedButton = new Vector2Int(highlightedButton.x, 0);
        else if(highlightedButton.y < 0)
            highlightedButton = new Vector2Int(highlightedButton.x, buttons[highlightedButton.x].Length-1);
        

        if(highlightedButton != Vector2Int.zero)
            isUsingController = true;
        


        if(isUsingController && oldPos != highlightedButton)
        {
            buttons[oldPos.x][oldPos.y].GetComponent<Image>().sprite = notHighlightedButtonSprite;
            buttons[highlightedButton.x][highlightedButton.y].GetComponent<Image>().sprite = highlightedButtonSprite;

        }
        if(Input.GetAxisRaw("ControllerAButton") > 0 && isUsingController)
        {
            buttons[highlightedButton.x][highlightedButton.y].onClick.Invoke();
            this.GetComponent<MainMenuControllerSupport>().enabled = false;
        }
        else if(Input.GetAxisRaw("ControllerAButton") > 0)
            isUsingController = true;
        
        


    }
}
