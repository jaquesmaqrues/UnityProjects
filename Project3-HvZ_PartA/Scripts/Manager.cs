using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject zombie;
    public GameObject human;
    public GameObject psg;
    List<GameObject> humans;
    List<Human> humanScripts;
    List<GameObject> zombies;
    List<Zombie> zombieScripts;
    GameObject nearestHuman;
    GameObject nearestZombie;

    void Start()
    {
        humans = new List<GameObject>();
        humanScripts = new List<Human>();
        zombies = new List<GameObject>();
        zombieScripts = new List<Zombie>();

        psg = Instantiate(psg, new Vector3(0, 1.5f, 0), Quaternion.identity);

        for (int x = 0; x < 2; x++)
        {
            zombies.Add(Instantiate(zombie, new Vector3(Random.Range(-23, 23), 1.5f, Random.Range(-23, 23)), Quaternion.identity));
            zombieScripts.Add(zombies[x].GetComponent<Zombie>());
        }
        for (int x = 0; x < 5; x++)
        {
            humans.Add(Instantiate(human, new Vector3(Random.Range(-23, 23), 1, Random.Range(-23, 23)), Quaternion.identity));
            humanScripts.Add(humans[x].GetComponent<Human>());
        }

    }

    void Update()
    {
        if(humans.Count != 0)
        {
            nearestHuman = humans[0];
            nearestZombie = humans[0];
            foreach (Human h in humanScripts)
            {
                h.PSGPosition = psg.transform.position;
                foreach (Zombie z in zombieScripts)
                {
                    if ((h.gameObject.transform.position - z.gameObject.transform.position).sqrMagnitude < (nearestHuman.transform.position - z.gameObject.transform.position).sqrMagnitude)
                    {
                        z.HumanPosition = h.gameObject.transform.position;
                        nearestHuman = h.gameObject;
                    }

                    if ((h.gameObject.transform.position - z.gameObject.transform.position).sqrMagnitude < (nearestZombie.transform.position - h.gameObject.transform.position).sqrMagnitude)
                    {
                        h.ZombiePosition = z.gameObject.transform.position;
                        nearestZombie = z.gameObject;
                    }

                    if ((h.gameObject.transform.position - z.gameObject.transform.position).sqrMagnitude < 4)
                    {
                        zombies.Add(Instantiate(zombie, h.gameObject.transform.position, Quaternion.identity));
                        zombieScripts.Add(zombies[zombies.Count - 1].GetComponent<Zombie>());
                        humans.Remove(h.gameObject);
                        humanScripts.Remove(h);
                        Destroy(h.gameObject);
                    }
                }

            }
        }
        else
        {
            foreach (Zombie z in zombieScripts)
            {
                z.HumanPosition = z.gameObject.transform.position;
                z.Freeze();
            }
        }
    }
}
