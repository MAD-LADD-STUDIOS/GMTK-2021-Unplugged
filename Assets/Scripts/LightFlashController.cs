using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal; // what

public class LightFlashController : MonoBehaviour
{
    [SerializeField] bool flashEnabled;
    [SerializeField] float flashMinLength;
    [SerializeField] float flashMaxLength;
    [SerializeField] float pauseMinLength;
    [SerializeField] float pauseMaxLength;
    [SerializeField] float intensity;

    [SerializeField] GameObject childLight;

    void Start()
    {
        childLight = transform.GetChild(1).gameObject;
        if(flashEnabled)
            StartCoroutine(DoRandomPause());
    }

    IEnumerator DoRandomLight()
    {
        childLight.GetComponent<Light2D>().intensity = intensity;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(Random.Range(flashMinLength, flashMaxLength-1) + Random.value);
        StartCoroutine(DoRandomPause());

    }

    IEnumerator DoRandomPause()
    {
        childLight.GetComponent<Light2D>().intensity = 0;
        GetComponent<AudioSource>().Pause();
        yield return new WaitForSeconds(Random.Range(pauseMinLength, pauseMaxLength-1) + Random.value);
        StartCoroutine(DoRandomLight());
    }


    // we might need it /shrug
    public void TurnOnLight()
    {
        childLight.GetComponent<Light2D>().intensity = intensity;
    }

    public void TurnOfflight()
    {
        childLight.GetComponent<Light2D>().intensity = 0;
    }
}
