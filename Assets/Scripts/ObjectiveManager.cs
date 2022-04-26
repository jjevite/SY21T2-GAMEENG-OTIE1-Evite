using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private Queue<string> sentences;
    [SerializeField] private TextMeshProUGUI objective;

    private void Start()
    {
        sentences = new();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log(dialogue.sentences[0]);
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplaySentence();
    }

    private void DisplaySentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence =  sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        objective.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            objective.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void EndDialogue()
    {
        Debug.Log("End Of Text");
    }
}
