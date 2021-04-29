using Ditzelgames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaterFloat))]
public class WaterBoat : MonoBehaviour
{
    //visible Properties
    public Transform Motor;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 100f;
    public float Drag = 0.1f;
    public Vector3 moveToPoint;
    public Vector3 curSpeed;

    //public GameObject dirCapsule;
    //used Components
    protected Rigidbody Rigidbody;
    protected Quaternion StartRotation;
    protected ParticleSystem ParticleSystem;
    protected Camera Camera;

    //public Vector3 checkMovement;
    float checkDistance = 0.0f;
    float currentDistance = 0.0f;
    bool bMovingTowardsPoint = false;
    float steer = 0;
    public float distanceCheck;
    float distanceToPoint = 20.0f;
    bool bDebug = false;

    //public Transform target;
    public float dirNum;

    protected bool bMoving;

    //internal Properties
    protected Vector3 CamVel;

    protected GameObject target;


    public void Awake()
    {
        ParticleSystem = GetComponentInChildren<ParticleSystem>();
        Rigidbody = GetComponent<Rigidbody>();
        StartRotation = Motor.localRotation;
        Camera = Camera.main;
        moveToPoint = new Vector3(0.0f, 0.0f, 0.0f);
        SetMoveToGameObject(target);
    }

    public void FixedUpdate()
    {
        if (bMoving)
        {
            AIMovement();
            ParticleSystem.Play();
        }
        else
        {
            Rigidbody.velocity = Rigidbody.velocity*0.9f;

            //Rigidbody.velocity = new Vector3(Rigidbody.velocity.x - 0.1f, Rigidbody.velocity.y - 0.1f, Rigidbody.velocity.z - 0.1f);

            //if(Rigidbody.velocity.x < 0.0f && Rigidbody.velocity.y < 0.0f && Rigidbody.velocity.z < 0.0f)
            //{
            //    Rigidbody.velocity *= 0.0f;
            //}
            ParticleSystem.Pause();


        }
        curSpeed = Rigidbody.velocity;

    }

    void AIMovement()
    {
        //moveToPoint = target.transform.position;
        //Vector3 newVector = moveToPoint - this.GetComponent<Transform>().position;

        // Unity's MoveTo
        // NativeMoveTo();


        //steer direction [-1,0,1]

        // ref a = left, d = right
        //        if (Input.GetKey(KeyCode.A))
        //          steer = 1;
        //    if (Input.GetKey(KeyCode.D))
        //      steer = -1;

        float steer;
        
        // Check if moving closer to point
        steer = CheckMovement();

        //Rotational Force
        Rigidbody.AddForceAtPosition(steer * transform.right * SteerPower / 100f, Motor.position);

        //compute forward vector
        var forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        //forward
        PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * MaxSpeed, Power);

        // back power
        // PhysicsHelper.ApplyForceToReachVelocity(Rigidbody, forward * -MaxSpeed, Power);

        //Motor Animation // Particle system
        Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(0, 30f * steer, 0));

        //moving forward
        var movingForward = Vector3.Cross(transform.forward, Rigidbody.velocity).y < 0;

        //move in direction
        Rigidbody.velocity = Quaternion.AngleAxis(Vector3.SignedAngle(Rigidbody.velocity, (movingForward ? 1f : 0f) * transform.forward, Vector3.up) * Drag, Vector3.up) * Rigidbody.velocity;

        //camera position
        //Camera.transform.LookAt(transform.position + transform.forward * 6f + transform.up * 2f);
        //Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, transform.position + transform.forward * -8f + transform.up * 2f, ref CamVel, 0.05f);
               
        AtPosition();
    }

    // Calculates if target heading is to the right or left of current heading
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(up, fwd);        // right vector
        float dir = Vector3.Dot(right, targetDir);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    float CheckMovement()
    {
        Vector3 heading = (moveToPoint - transform.position).normalized;
        dirNum = AngleDir(transform.forward, heading, transform.up);
        if (bDebug)
        {
            Debug.DrawLine(transform.position, transform.position + heading * 10, Color.red, 1.0f);
        }

        if (dirNum < 0)
        {
            //steering right
            steer = 1f;
            
        }
        else if (dirNum > 0)
        {
            // steering left

            steer = -1f;
        }
        else
        {
            steer = 0f;
        }
        return steer;

        //check if moving closer target
        //float distanceTemp = Vector3.Distance(moveToPoint, transform.position);
        //if (distanceTemp < distanceCheck)
        //{
        //    distanceCheck = distanceTemp;
        //    return true;
        //}
        //else if (distanceTemp > distanceCheck)
        //{ 
        //    distanceCheck = distanceTemp;
        //    return false;
        //}
        //return false;
    }

    void NativeMoveTo()
    {
        float speed = 10;

        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        Vector3 removeY = Vector3.MoveTowards(transform.position, moveToPoint, step);
        transform.position = new Vector3(removeY.x, 0.0f, removeY.z);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, moveToPoint) < 0.001f)
        {
            // Swap the position of the cylinder.
            //moveToPoint *= -1.0f;
        }
    }

    public void SetMoveToGameObject(GameObject target)
    {

    }
    public void SetMoveTo(Vector3 newPoint)
    {
        moveToPoint = newPoint;
        bMoving = true;
    }

    // Check if at Destination
    void AtPosition()
    {
        float distanceCheck = Vector3.Distance(moveToPoint, transform.position);
        if (distanceCheck < distanceToPoint)
        {
            bMoving = false;
            Motor.SetPositionAndRotation(Motor.position, transform.rotation * StartRotation * Quaternion.Euler(0, 0, 0));

        }
    }
}