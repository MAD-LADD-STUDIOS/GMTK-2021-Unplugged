using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;

public enum doorType {forward, side};
public class DoorAndButton_Door : MonoBehaviour
{
    // visuals
    [SerializeField] doorType type;
    [SerializeField] Sprite[] forwardOnSprites;
    [SerializeField] Sprite[] sideOnSprites;
    [SerializeField] Sprite forwardOffSprite;
    [SerializeField] Sprite sideOffSprite;
    [SerializeField] SpriteRenderer selfSpriteRenderer;
    [SerializeField] Light2D childLight;
    public bool flipFunction;

    // visuals control
    int randomElecIndex;
    bool isCoroutineRunning;
    // actual door stuff
    public bool isOpened;
    
    void Start()
    {
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
        if(type == doorType.forward)
            randomElecIndex = Mathf.RoundToInt(Random.Range(0, forwardOnSprites.Length));
        else
            randomElecIndex = Mathf.RoundToInt(Random.Range(0, sideOnSprites.Length));
        

        childLight = transform.GetChild(0).GetComponent<Light2D>();

        if(type == doorType.side)
        {
            transform.eulerAngles += Vector3.up * 0.1f;
        }
    }


    void Update()
    {
        if(isOpened)
        {
            if(type == doorType.forward)
                selfSpriteRenderer.sprite = forwardOffSprite;
            else
                selfSpriteRenderer.sprite = sideOffSprite;
            
            childLight.intensity = 0;
        }
        else
        {
            if(type == doorType.forward)
                selfSpriteRenderer.sprite = forwardOnSprites[randomElecIndex];
            else
                selfSpriteRenderer.sprite = sideOnSprites[randomElecIndex];

            childLight.intensity = 2;
        }


        if(Random.value < 0.05f && !isCoroutineRunning)
        {
            StartCoroutine(RandomlyChangeElecLook());
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(!isOpened)
                FindObjectOfType<GameManager>().PlayerDied(collider.gameObject.name == "PlayerLeft" ? 0 : 1);
        }
    }

    IEnumerator RandomlyChangeElecLook()
    {
        isCoroutineRunning = true;
        bool switchElec = Random.Range(0, 1) == 1;

        int newElecIndex = (type == doorType.forward) ? Random.Range(0, forwardOnSprites.Length) : Random.Range(0, sideOnSprites.Length);

        if(switchElec)
            randomElecIndex = newElecIndex;
        else
        {
            int oldIndex = randomElecIndex;
            randomElecIndex = newElecIndex;
            yield return new WaitForSeconds(Random.value);
            randomElecIndex = oldIndex;
        }

        isCoroutineRunning = false;        
    }

}
