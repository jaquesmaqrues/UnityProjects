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
    float safeDistance;
    public List<GameObject> obstacles;

    float dotF;
    float dotR;
    float sum;
    Vector3 distance;
    Vector3 desiredVelocity;
    Vector3 center;

    public Material centerLine;

    public void Start()
    {
        safeDistance = 5;
        vehiclePosition = transform.position;
        center = new Vector3(vehiclePosition.x, vehiclePosition.y + 18, vehiclePosition.z);
        obstacles = GameObject.Find("Manager").GetComponent<Manager>().obstacles;
        
    }

    void Update()
    {
        velocity += acceleration * Time.deltaTime;
        vehiclePosition += velocity * Time.deltaTime;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
        vehiclePosition = new Vector3(vehiclePosition.x, vehiclePosition.y, vehiclePosition.z); 
        if(direction != Vector3.zero)
        {
            gameObject.transform.forward = direction;
        }
        Debug.Log(velocity);
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


    //Forces Gameobjects to stay in bounds
    public Vector3 InBounds()
    {
        center = new Vector3(vehiclePosition.x, vehiclePosition.y - 20, vehiclePosition.z);
        return Seek(center * -1) * center.sqrMagnitude / 600;
    }
    //Abstract method used in Zombie and Human classes
    public abstract void CalcSteeringForce();

    //Avoids nearby obstacles
    public Vector3 ObstacleAvoidance(GameObject obstacle)
    {
        distance = new Vector3(obstacle.transform.position.x, 0, obstacle.transform.position.z) - new Vector3(vehiclePosition.x, 0, vehiclePosition.z);
        //distance = obstacle.transform.position - vehiclePosition;
        dotF = Vector3.Dot(distance, transform.forward);
        dotR = Vector3.Dot(distance, transform.right);
        sum = obstacle.GetComponent<MeshFilter>().mesh.bounds.extents.x + 2;


        if (dotF < 0 || distance.magnitude > safeDistance || sum < Mathf.Abs(dotR))
            return Vector3.zero;

        if (dotR < 0)
            desiredVelocity = transform.right * maxSpeed;
        else
            desiredVelocity = -transform.right * maxSpeed;

        Debug.DrawLine(transform.position, obstacle.transform.position, Color.blue);

        return desiredVelocity - velocity;
    }

    public Vector3 Algin(Vector3 average)
    {
        desiredVelocity = average.normalized * maxSpeed;
        return desiredVelocity - velocity;
    }
}
