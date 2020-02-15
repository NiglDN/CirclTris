using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlockBehavior : MonoBehaviour
{
    public float fallTime = 1;
    int interval = 1;
    float nextTime = 3;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextTime) {
            this.transform.Translate(
            0,
            -1,
            0
            );
            nextTime += interval; 
        }
    }
}
