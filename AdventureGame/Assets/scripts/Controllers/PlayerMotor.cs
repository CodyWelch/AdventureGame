using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    public bool bMainPlayer;
    public GameObject mainPlayer;

    Transform target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            if (bMainPlayer)
            {

                if (target != null)
                {
                    agent.stoppingDistance = 1f;
                    agent.SetDestination(target.position);
                    FaceTarget();
                }
                else
                {
                    agent.stoppingDistance = 1f;

                }
            }
            else
            {
                if (target != null)
                {
                    agent.stoppingDistance = 1f;
                    agent.SetDestination(target.position);
                    FaceTarget();
                }
                else
                {
                    // follow player
                    agent.stoppingDistance = 4f;
                    agent.SetDestination(mainPlayer.transform.position);
                    //FaceTarget();
                    // detect target

                }
            }
 


        }
    }

    public void SetMainPlayer(bool setActive,GameObject newMainPlayer)
    {
        bMainPlayer = setActive;
        mainPlayer = newMainPlayer;
    }

    public void MoveToPoint (Vector3 point)
    {
        agent.SetDestination(point);
    }
    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * .8f;
        agent.updateRotation = false;
        //target = newTarget.transform;
        target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;

        target = null;
    }

   void  FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
