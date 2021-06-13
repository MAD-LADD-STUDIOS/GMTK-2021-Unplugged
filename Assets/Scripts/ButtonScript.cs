using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void LoadScene(int id)
    {
        FindObjectOfType<GameManager>().LoadScene(id);
    }
}
