using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    Vector3 ultimate;
    Vector3 seperate;
    public Vector3 humanPosition;
    public Vector3 humanVelocity;
    public Material humanLine;

    public Vector3 HumanPosition
    {
        set { humanPosition = value; }
    }

    public Vector3 HumanVelocity
    {
        set { humanVelocity = value; }
    }

    public Vector3 Seperate
    {
        set { seperate = value; }
    }

    public override void CalcSteeringForce()
    {
        ultimate = Vector3.zero;
        //Seperates zombies
        ultimate += seperate * 20;
        //Forces zombies in bounds
        ultimate += InBounds();
        //Checks if there are any zombies left
        if (humanPosition != Vector3.zero)
        {
            ultimate += Pursue(humanPosition, humanVelocity);
        }
        //If not wanders
        else
        {
            ultimate += Wander();
        }
        
        foreach (GameObject o in obstacles)
        {
            ultimate += ObstacleAvoidance(o) * 30;
        }

        seperate = Vector3.zero;
        //Applies ultimate force
        ApplyForce(ultimate.normalized * maxSpeed);

    }
    //Gl Lines
    private void OnRenderObject()
    {
        //Line for whichever human a zombie is seeking
        humanLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(humanPosition);
        GL.End();
    }
}
