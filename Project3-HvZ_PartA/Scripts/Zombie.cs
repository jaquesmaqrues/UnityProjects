using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    Vector3 ultimate;
    Vector3 humanPosition;
    public Material humanLine;
    public Material fowardLine;
    public Material rightLine;

    public Vector3 HumanPosition
    {
        set { humanPosition = value; }
    }

    public override void CalcSteeringForce()
    {
        ultimate += Seek(humanPosition);
        ApplyForce(ultimate.normalized * maxSpeed);

    }

    private void OnRenderObject()
    {
        humanLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(humanPosition);
        GL.End();

        fowardLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + gameObject.transform.forward*5);
        GL.End();

        rightLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + gameObject.transform.right*5);
        GL.End();
    }
}
