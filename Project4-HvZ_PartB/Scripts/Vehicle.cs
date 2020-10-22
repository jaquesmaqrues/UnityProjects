using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    private Vector3 vehiclePosition;
    public Vector3 acceleration;
    public Vector3 direction;
    public Vector3 velocity;
    public float mass;
    public float maxSpeed;
    public float safeDistance;
    public List<GameObject> obstacles;

    float dotF;
    float dotR;
    float sum;
    Vector3 distance;
    Vector3 desiredVelocity;

    public Material fowardLine;
    public Material rightLine;
    public Material futureLine;


    public void Start()
    {
        obstacles = GameObject.Find("Manager").GetComponent<Manager>().obstacles; 
        vehiclePosition = transform.position;
    }

    void Update()
    {
        velocity += acceleration * Time.deltaTime;
        vehiclePosition += velocity * Time.deltaTime;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
        vehiclePosition = new Vector3(vehiclePosition.x, 1, vehiclePosition.z); 
        if(direction != Vector3.zero)
        {
            gameObject.transform.forward = direction;
        }

        transform.position = vehiclePosition;

        CalcSteeringForce();
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }
    //GameObjects Seek target position
    public Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = targetPosition - vehiclePosition;

        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;

        Vector3 seekingForce = desiredVelocity - velocity;

        return seekingForce;
    }

    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    //GameObjects run flee from targetPosition
    public Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = vehiclePosition - targetPosition;

        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;

        Vector3 fleeingForce = desiredVelocity - velocity;

        return fleeingForce;
    }
    //Forces Gameobjects to stay in bounds
    public Vector3 InBounds()
    {
        //Checks if gameObjects are too close the edge and forces them to seek center
        if (vehiclePosition.x > 21 || vehiclePosition.x < -21 || vehiclePosition.z > 21 || vehiclePosition.z < -21)
        {
            return Seek(Vector3.zero - transform.position) * (Vector3.zero - transform.position).sqrMagnitude/700;
        }
        return Vector3.zero;
    }
    //Abstract method used in Zombie and Human classes
    public abstract void CalcSteeringForce();

    //Avoids nearby obstacles
    public Vector3 ObstacleAvoidance(GameObject obstacle)
    {
        distance = obstacle.transform.position - vehiclePosition;
        dotF = Vector3.Dot(distance, transform.forward);
        dotR = Vector3.Dot(distance, transform.right);
        sum = obstacle.GetComponent<MeshFilter>().mesh.bounds.extents.x + gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().bounds.extents.x;


        if (dotF < 0 || distance.magnitude > safeDistance || sum < Mathf.Abs(dotR))
            return Vector3.zero;

        if (dotR < 0)
            desiredVelocity = transform.right * maxSpeed;
        else
            desiredVelocity = -transform.right * maxSpeed;

        Debug.DrawLine(transform.position, obstacle.transform.position, Color.blue);

        return desiredVelocity - velocity;
    }

    //Code to have gameObjects randomly wander when not doing anything
    public Vector3 Wander()
    {
        //Creates circle 5 units ahead with radius 5 and a random angle
        Vector3 circleCenter = transform.position + direction * 5;
        float angle = Random.Range(0, 360);
        float radius = 5;
        //Use random angle and circle to seek a random position in front of the gameObject
        return Seek(new Vector3(circleCenter.x + Mathf.Cos(angle) * radius, transform.position.y , circleCenter.z + Mathf.Sin(angle) * radius));

    }

    //Pursues a target by perdicting future position using velocity
    public Vector3 Pursue(Vector3 target, Vector3 vel)
    {
        return Seek(target + vel * 2);
    }

    //Evades a target by perdicting future position using velocity
    public Vector3 Evade(Vector3 target, Vector3 vel)
    {
        return Seek(target + vel * 2);
    }

    //GL Lines
    private void OnRenderObject()
    {
        //Line for foward position
        fowardLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + gameObject.transform.forward * 5);
        GL.End();

        //Line for right 
        rightLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + gameObject.transform.right * 5);
        GL.End();

        //Line for future position
        futureLine.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(gameObject.transform.position);
        GL.Vertex(gameObject.transform.position + velocity * 5);
        GL.End();
    }
}
