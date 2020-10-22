using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    Vector3 ultimate;
    Vector3 zombiePosition;
    Vector3 psgPosition;
    public Material fowardLine;
    public Material rightLine;
    public Material psgLine;

    public Vector3 ZombiePosition
    {
        set { zombiePosition = value; }
    }

    public Vector3 PSGPosition
    {
        set { psgPosition = value; }
    }

    public override void CalcSteeringForce()
    {
        if ((gameObject.transform.position - zombiePosition).sqrMagnitude < 81)
        {
            ultimate += Flee(zombiePosition);
        }
        else
        {
            ultimate += Seek(psgPosition);
        }
        ApplyForce(ultimate.normalized * maxSpeed);
    }

    private void OnRenderObject()
    {
        fowardLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + gameObject.transform.forward * 5);
        GL.End();

        rightLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + gameObject.transform.right * 5);
        GL.End();

        psgLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(transform.position);
        GL.Vertex(psgPosition);
        GL.End();
    }
}
