using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiedObjectScript : MonoBehaviour
{
    public Transform objectLocCopy;
    public int playerDiedIndex;

    // Update is called once per frame
    void Update()
    {
        transform.position = objectLocCopy.position;
    }

}
