using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Interactable
{
    public override void Interact()
    {
        base.Interact();
        // attack the enemy
    }

    public NavMeshAgent agent;

    public bool isActive;
    public float agroRange;

    public GameObject player;

    public void Awake()
    {
        isActive = false;
        agroRange = 10.0f;
    }

    void Update()
    {
        
            
        if (agroRange >= Vector3.Distance(player.GetComponent<Transform>().position, this.GetComponent<Transform>().position))
         { isActive = true;
        }
        else
        {
            isActive = false;
        }

        if (isActive)
        {
            agent.SetDestination(player.GetComponent<Transform>().position);
        }
    }
}
