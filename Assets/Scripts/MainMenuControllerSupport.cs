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
    }
    [SerializeField] buttonArray[] buttons;

    Vector2 highlightedButton;
    [SerializeField] Sprite highlightedButtonSprite;
    [SerializeField] Sprite notHighlightedButtonSprite;

    void Update()
    {

    }
}
