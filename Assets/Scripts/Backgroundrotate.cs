using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgroundrotate : MonoBehaviour
{
    bool wait = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait){
            foreach (Touch touch in Input.touches)
            {
                if (touch.position.x < Screen.width / 3)
                {
                    rotateLeft();
                }
                else if (touch.position.x > (Screen.width / 3)*2)
                {
                    rotateRight();
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotateLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                rotateRight();
            }
            StartCoroutine(delayRoutine());
        }
    }

    public void rotateLeft() {
        transform.Rotate (Vector3.forward * -9);
    }
    public void rotateRight() {
        transform.Rotate (Vector3.forward * 9);
    }

    IEnumerator delayRoutine()
    {
        wait = true;
        yield return new WaitForSeconds(0.2F);
        wait = false;
    }
}