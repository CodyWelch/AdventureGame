using UnityEngine;

public class DialogueTrigger : Interactable
{
    public Dialogue dialogue;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Tried to open dialogue");
        TriggerDialoge();
    }

    void TriggerDialoge()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
