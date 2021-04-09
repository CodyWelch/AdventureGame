using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerMotor))]
public class Player : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;
    PlayerMotor motor;
    public LayerMask movementMask;
    public bool isActive;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        cam = Camera.main;
        isActive = false;
    }

    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
            {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);
//                agent.SetDestination(hit.point);

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                // check if interactable

            }
        }
    }
}
