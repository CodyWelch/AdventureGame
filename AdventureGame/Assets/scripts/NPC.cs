using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class NPC : Interactable
{
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interacting with NPC");
    }

}
