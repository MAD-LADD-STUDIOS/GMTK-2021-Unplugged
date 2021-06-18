using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInput : MonoBehaviour
{
    // TEMP SCRIPT FOR TESTING INPUT SYSTEM.

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        var x = FindObjectOfType<GameManager>().settings._controllerSens;
        transform.GetChild(1).transform.localScale = new Vector3(x, x, x);

    }
    void Update()
    {
        transform.GetChild(0).transform.localPosition = new Vector2(Input.GetAxisRaw("ControllerLeftX")/2, -Input.GetAxisRaw("ControllerLeftY")/2);

        var a = Input.GetAxisRaw("ControllerAButton");
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color((1-a), a, 0);
    }
}
