using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] ObjectiveManager objectiveManager;
    public Dialogue dialogue;

    [SerializeField] GameObject ObjectiveCanvas;


    private void Start()
    {
        triggerDialogue();
    }
    public void triggerDialogue()
    {
        objectiveManager.StartDialogue(dialogue);
    }

    public void CloseObjectiveCanvas()
    {
        ObjectiveCanvas.SetActive(false);
    }
}
