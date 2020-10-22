using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Vehicle
{
    Vector3 ultimate;
    Vector3 disTotal;
    Vector3 fowTotal;
    Vector3 posTotal;
    Vector3 randomSpot;

    bool draw;

    public Vector3 DisTotal
    {
        set { disTotal = value; }
    }

    public Vector3 PosTotal
    {
        set { posTotal = value; }
    }

    public Vector3 FowTotal
    {
        set { fowTotal = value; }
    }

    public Vector3 RandomSpot
    {
        set { randomSpot = value; }
    }

    public override void CalcSteeringForce()
    {
        ultimate = Vector3.zero;
        //Keeps objects in bounds
        ultimate += InBounds() * 10;
        //Seeks random point
        ultimate += Seek(randomSpot) * .7f;
        //Aligns all objects
        ultimate += Algin(fowTotal) * 10;
        //All objects seek center pos
        ultimate += Seek(posTotal) * .5f;
        if(disTotal != Vector3.zero)
        {
            ultimate += Seek(disTotal + gameObject.transform.position)* 10;
        }
        foreach (GameObject o in obstacles)
        {
            ultimate += ObstacleAvoidance(o) * 1000;
        }
        //Applys the ultimate force
        ApplyForce(ultimate.normalized * maxSpeed);
        if (Input.GetKeyDown("space"))
        {
            draw = !draw;
        }
    }

    private void OnRenderObject()
    {
        if (draw)
        {
            //Line for future position
            centerLine.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(posTotal);
            GL.Vertex(posTotal + fowTotal);
            GL.End();
        }

    }
}
