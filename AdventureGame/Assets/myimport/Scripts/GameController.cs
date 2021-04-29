using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject[] waterBoats;

    public Vector3 setMovePoint;
    public GameObject target;

    void Start()
    {


    }

    void Update()
    {
        foreach (GameObject waterBoat in waterBoats)
        {
            waterBoat.GetComponent<WaterBoat>().SetMoveTo(target.transform.position);
        }


    }
}
