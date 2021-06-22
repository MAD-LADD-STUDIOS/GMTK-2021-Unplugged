using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<ConstantForce2D>().force = (Vector2.up * Random.Range(-10, 10)) + (Vector2.right * Random.Range(-10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).localEulerAngles += Vector3.forward * 0.05f;
    }
}
