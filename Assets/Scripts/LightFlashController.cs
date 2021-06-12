using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal; // what

public class LightFlashController : MonoBehaviour
{
    public bool flashEnabled;
    public float flashMinLength;
    public float flashMaxLength;
    public float pauseMinLength;
    public float pauseMaxLength;
    public float intensity;

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
        yield return new WaitForSeconds(Random.Range(flashMinLength, flashMaxLength));
        StartCoroutine(DoRandomPause());

    }

    IEnumerator DoRandomPause()
    {
        childLight.GetComponent<Light2D>().intensity = 0;
        yield return new WaitForSeconds(Random.Range(pauseMinLength, pauseMaxLength));
        StartCoroutine(DoRandomLight());
    }
}
