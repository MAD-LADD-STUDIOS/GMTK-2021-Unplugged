using TMPro;
using UnityEngine;
using System.Collections;

public class Typing : MonoBehaviour
{
    [SerializeField] string text;

    [SerializeField] TMP_Text textMeshProTextItem; // best naming in the west

    [SerializeField] bool typeOnStart;
    [SerializeField] float minTypeTime;
    [SerializeField] float maxTypeTime;
    int charDone;

    void Start()
    {
        if(typeOnStart)
            StartTyping();
    }
    public void StartTyping()
    {
        StartCoroutine(typeLetterAndWait());
    }

    IEnumerator typeLetterAndWait()
    {
        while(charDone != text.Length)
        {
            charDone ++;
            textMeshProTextItem.text = startToIndex(text, charDone);
            yield return new WaitForSeconds(Random.Range(minTypeTime, maxTypeTime));
        }
    }

    string startToIndex(string item, int index)
    {
        string newString = "";

        for (int i = 0; i < index; i++)
        {
            newString += item[i].ToString();
        }

        return newString;
    }
}
