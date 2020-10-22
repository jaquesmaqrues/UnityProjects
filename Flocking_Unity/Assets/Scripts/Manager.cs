using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //Public prefabs added
    public Camera cameraBase;

    //Lists of gameObjects
    public List<GameObject> obstacles;
    public List<Fish> fish;

    //Forces caculated from all fish
    Vector3 fowTotal;
    Vector3 posTotal;
    Vector3 disTotal;
    //Random spot chosen to seek
    Vector3 randomSpot;

    //GameObject to show center of flock
    public GameObject center;

    bool draw;

    float time;

    //CameraController Script
    CameraController cameraController;

    void Start()
    {
        cameraController = GameObject.Find("Manager").GetComponent<CameraController>();
        randomSpot = new Vector3(Random.Range(-15f, 15f), Random.Range(2f, 15f), Random.Range(-15f, 15f));
    }

    void Update()
    {
        time += Time.deltaTime;
        //Turns on and off drawing of center object
        if (Input.GetKeyDown("space"))
        {
            draw = !draw;
        }

        //Resets values
        fowTotal = Vector3.zero;
        posTotal = Vector3.zero;

        //Loops through all fish
        foreach (Fish f in fish)
        {
            disTotal = Vector3.zero;
            //Caculates distance from all objects
            foreach (Fish f2 in fish)
            {
                if (f2.gameObject != f.gameObject)
                {
                    if ((f.gameObject.transform.position - f2.gameObject.transform.position).sqrMagnitude < 9)
                    {
                        disTotal += f.gameObject.transform.position - f2.gameObject.transform.position;
                    }
                }
            }
            f.DisTotal = disTotal;
            //Finds all foward and position vectors
            fowTotal += f.gameObject.transform.forward;
            posTotal += f.gameObject.transform.position;
            //Updates random spot if any fish are too close
            if (((randomSpot - f.gameObject.transform.position).sqrMagnitude < 16) || time > 5)
            {
                time = 0;
                randomSpot = new Vector3(Random.Range(-15f, 15f), Random.Range(2f, 15f), Random.Range(-15f, 15f));
            }
        }

        //Gives values to each fish
        foreach (Fish f in fish)
        {
            f.FowTotal = fowTotal;
            f.PosTotal = posTotal / fish.Count;
            f.RandomSpot = randomSpot;
        }

        if (draw)
        {
            center.transform.position = posTotal / fish.Count;
        }
        else
        {
            center.transform.position = Vector3.zero;
        }

        //Moves cameras based on flock position
        for (int c = 0; c < cameraController.cameras.Count; c++)
        {
            if (c == 1)
            {
                cameraController.cameras[c].transform.position = posTotal / fish.Count - fowTotal;
                cameraController.cameras[c].transform.forward = fowTotal;
            }
            if (c == 2)
            {
                cameraController.cameras[c].transform.position = posTotal / fish.Count + fowTotal;
                cameraController.cameras[c].transform.forward = fowTotal;
            }
        }
    }

        
}
