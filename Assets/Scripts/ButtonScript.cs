using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject credits;

    public void LoadScene(int id)
    {
        FindObjectOfType<GameManager>().LoadScene(id);
    }

    public void toggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }
}
