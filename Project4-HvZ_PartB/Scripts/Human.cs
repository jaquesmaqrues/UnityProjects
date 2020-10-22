using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    Vector3 ultimate;
    Vector3 zombiePosition;
    Vector3 zombieVelocity;
    Vector3 seperate;

    public Vector3 ZombiePosition
    {
        set { zombiePosition = value; }
    }

    public Vector3 ZombieVelocity
    {
        set { zombieVelocity = value; }
    }

    public Vector3 Seperate
    {
        set { seperate = value; }
    }

    public override void CalcSteeringForce()
    {
        ultimate = Vector3.zero;
        ultimate += seperate * 20;
        ultimate += InBounds();
        //Checks if closest zombie is close enough to flee from
        if ((gameObject.transform.position - zombiePosition).sqrMagnitude < 100)
        {
            ultimate += Evade(zombiePosition, zombieVelocity);
        }
        //If not just wanders
        else
        {
            ultimate += Wander();
        }
        //Checks each obstacle and avoids it
        foreach(GameObject o in obstacles)
        {
            ultimate += ObstacleAvoidance(o) * 30;
        }
        //Applys the ultimate force
        ApplyForce(ultimate.normalized * maxSpeed);
        seperate = Vector3.zero;
    }
}
