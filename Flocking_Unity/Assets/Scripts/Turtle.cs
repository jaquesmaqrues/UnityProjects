using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Vehicle
{
    Vector3 ultimate;

    public List<GameObject> feathers;
    int count;
    bool draw;


    public override void CalcSteeringForce()
    {
        //Checks if tutle is close enough to move to next feather
        if((transform.position - feathers[count].transform.position).sqrMagnitude < 4)
        {
            if(count == feathers.Count - 1)
            {
                count = 0;
            }
            else
            {
                count++;
            }
        }
        ultimate = Vector3.zero;
        ultimate += Seek(feathers[count]);
        ApplyForce(ultimate.normalized * maxSpeed);
        if (Input.GetKeyDown("space"))
        {
            draw =! draw;
        }

    }

    private void OnRenderObject()
    {
        //Only draws debug line if space is pressed
        if (draw)
        {
            //Line for future position
            centerLine.SetPass(0);
            GL.Begin(GL.LINES);
            for (int x = 1; x < feathers.Count; x++)
            {
                GL.Vertex(feathers[x - 1].transform.position);
                GL.Vertex(feathers[x].transform.position);
            }
            GL.Vertex(feathers[feathers.Count - 1].transform.position);
            GL.Vertex(feathers[0].transform.position);
            GL.End();
        }

    }
}
