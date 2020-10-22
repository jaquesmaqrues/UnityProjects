using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //Public prefabs added
    public GameObject zombie;
    public GameObject human;
    public Camera cameraBase;
    public GameObject obstacle;

    //Lists of gameObjects
    public List<GameObject> humans;
    public List<GameObject> zombies;
    public List<GameObject> obstacles;

    //Closest human and zombie
    GameObject nearestHuman;
    GameObject nearestZombie;

    //Force to seperate humans and zombies
    Vector3 sepZombie;
    Vector3 sepHuman;

    //CameraController Script
    CameraController cameraController;

    void Start()
    {
        humans = new List<GameObject>();
        zombies = new List<GameObject>();
        obstacles = new List<GameObject>();
        cameraController = GameObject.Find("Manager").GetComponent<CameraController>();
        

        //Spawns in obstacles, zombies, and humans at random positions
        for (int x = 0; x < 10; x++)
        {
            obstacles.Add(Instantiate(obstacle, new Vector3(Random.Range(-23, 23), .5f, Random.Range(-23, 23)), Quaternion.identity));
        }

        for (int x = 0; x < 2; x++)
        {
            zombies.Add(Instantiate(zombie, new Vector3(Random.Range(-23, 23), 0f, Random.Range(-23, 23)), Quaternion.identity));
            //Creates two cameras too follow the orginal two zombies
            cameraController.cameras.Add(Instantiate(cameraBase, zombies[zombies.Count - 1].transform.position, Quaternion.identity));
            cameraController.cameras[cameraController.cameras.Count - 1].gameObject.AddComponent<SmoothFollow>();
        }
        for (int x = 0; x < 5; x++)
        {
            humans.Add(Instantiate(human, new Vector3(Random.Range(-23, 23), 0f, Random.Range(-23, 23)), Quaternion.identity));
        }

    }

    void Update()
    {
        //Updates the positions of the cameras
        for (int c = 3; c < cameraController.cameras.Count; c++)
        {
            cameraController.cameras[c].transform.position = new Vector3(zombies[c - 3].transform.position.x - 1, 5.2f, zombies[c - 3].transform.position.z - 1);
            cameraController.cameras[c].transform.rotation = zombies[c - 3].transform.rotation;
        }

        //Checks if any humans are left
        if (humans.Count != 0)
        {
            //Checks if any humans are colliding with any zombies
            foreach (GameObject h in humans)
            {
                foreach (GameObject z in zombies)
                {
                    if ((h.transform.position - z.transform.position).sqrMagnitude < 4)
                    {
                        zombies.Add(Instantiate(zombie, h.transform.position, Quaternion.identity));
                        humans.Remove(h);
                        Destroy(h);
                        return;
                    }
                }
            }

            for (int z = 0; z < zombies.Count; z++)
            {
                nearestHuman = humans[0];
                for (int h = 0; h < humans.Count; h++)
                {
                    //Finds the closest human for each zombie
                    if ((humans[h].transform.position - zombies[z].transform.position).sqrMagnitude < (nearestHuman.transform.position - zombies[z].transform.position).sqrMagnitude)
                    {
                        nearestHuman = humans[h].gameObject;
                    }
                }
                //Gives the zombie the position and velocity
                zombies[z].GetComponent<Zombie>().HumanPosition = nearestHuman.gameObject.transform.position;
                zombies[z].GetComponent<Zombie>().HumanVelocity = nearestHuman.GetComponent<Human>().velocity;
                sepZombie = Vector3.zero;
                //Checks if any zombies are too close and creates the seperation vector
                for (int zo = 0; zo < zombies.Count; zo++)
                {
                    if (zo != z && (zombies[z].transform.position - zombies[zo].transform.position).sqrMagnitude < 9)
                    {
                        sepZombie += (zombies[z].transform.position - zombies[zo].transform.position);
                    }
                }
                zombies[z].GetComponent<Zombie>().Seperate = sepZombie;
            }

            for (int h = 0; h < humans.Count; h++)
            {
                nearestZombie = zombies[0];
                for (int z = 0; z < zombies.Count; z++)
                {
                    //Finds the nearest zombie too each human
                    if ((humans[h].transform.position - zombies[z].transform.position).sqrMagnitude < (nearestZombie.transform.position - humans[h].transform.position).sqrMagnitude)
                    {
                        nearestZombie = zombies[z];
                    }
                }
                humans[h].GetComponent<Human>().ZombiePosition = nearestZombie.transform.position;
                humans[h].GetComponent<Human>().ZombieVelocity = nearestZombie.GetComponent<Zombie>().velocity;

                sepHuman = Vector3.zero;
                //Checks if any humans are too close and creates the seperation vector
                for (int hu = 0; hu < humans.Count; hu++)
                {
                    if (hu != h && (humans[h].transform.position - humans[hu].transform.position).sqrMagnitude < 9)
                    {
                        sepZombie += humans[h].transform.position - humans[hu].transform.position;
                    }
                }
                humans[h].GetComponent<Human>().Seperate = sepHuman;
            }

        }
        //If there are no humans left zombies start too wander
        else
        {
            foreach (GameObject z in zombies)
            {
                z.GetComponent<Zombie>().HumanPosition = Vector3.zero;
            }
        }
    }
}
