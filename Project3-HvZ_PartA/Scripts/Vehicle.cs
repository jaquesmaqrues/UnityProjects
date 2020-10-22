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

    public void Start()
    {
        vehiclePosition = transform.position;
    }

    void Update()
    {
        CalcSteeringForce();
        WrapShip();

        velocity += acceleration * Time.deltaTime;
        vehiclePosition += velocity * Time.deltaTime;
        direction = velocity.normalized;
        acceleration = Vector3.zero;
        vehiclePosition = new Vector3(vehiclePosition.x, 1, vehiclePosition.z);
        gameObject.transform.forward = direction;
        transform.position = vehiclePosition;
    }

    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

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

    public Vector3 Flee(Vector3 targetPosition)
    {
        Vector3 desiredVelocity = vehiclePosition - targetPosition;

        desiredVelocity.Normalize();
        desiredVelocity = desiredVelocity * maxSpeed;

        Vector3 fleeingForce = desiredVelocity - velocity;

        return fleeingForce;
    }

    void WrapShip()
    {
        if (vehiclePosition.x > 25)
            vehiclePosition = new Vector3(-24f, vehiclePosition.y, vehiclePosition.z);
        if (vehiclePosition.x < -25)
            vehiclePosition = new Vector3(24f, vehiclePosition.y, vehiclePosition.z);
        if (vehiclePosition.z > 25)
            vehiclePosition = new Vector3(vehiclePosition.x, vehiclePosition.y, -24f);
        if (vehiclePosition.z < -25)
            vehiclePosition = new Vector3(vehiclePosition.x, vehiclePosition.y, 24f);
    }

    public abstract void CalcSteeringForce();

    public void Freeze()
    {
        vehiclePosition = transform.position;
        transform.rotation = Quaternion.identity;
    }
}
