using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField] GameManager manager;

    void Start()
    {
        manager = transform.parent.GetComponent<GameManager>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var audio = manager.RandomFromArray(manager.clickPositiveSounds);
            GetComponent<AudioSource>().clip = audio;
            GetComponent<AudioSource>().Play();
        }
    }
}
