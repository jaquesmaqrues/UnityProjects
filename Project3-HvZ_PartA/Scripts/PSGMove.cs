using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSGMove : MonoBehaviour
{

    float targetTime = 3.0f;
    public Vector3 position;

    void Update()
    {
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            targetTime = 3.0f;
            transform.position = new Vector3(Random.Range(-23, 23), 1.5f, Random.Range(-23, 23));
        }
    }
}
