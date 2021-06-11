using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VoidScript : MonoBehaviour
{
    void Awake()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, 1); // BECAUSE FUCKING UNITY SAYS THAT IF THERE'S LITERALLY LESS THEN AN ATOM TOUCHING THEN THEY'RE TOUCHING
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Player"))
        {
            Debug.Log($"{this.gameObject.name} collided with {collider2D.gameObject.name}");
            SceneManager.LoadScene(0);
        }
    }
}
