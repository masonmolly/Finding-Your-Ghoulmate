using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float runSpeed = 10.0f;
    float walkSpeed = 2.0f;
    float rotateSpeed = 200.0f;
    float distance;
    Animator movement;
    GameObject ghoulmate;

    // Start is called before the first frame update
    void Start()
    {
        movement = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        while (ghoulmate == null)
        {
            ghoulmate = GameObject.Find("Ghoulmate(Clone)");
        }

        distance = Vector3.Distance(this.transform.position, ghoulmate.transform.position);

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            movement.SetBool("IsRunning", true);
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }

        else
        {
            movement.SetBool("IsRunning", false);
        }

        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            transform.Translate(Vector3.back * walkSpeed * Time.deltaTime);
        }

        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime);
        }

        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }

        else if (distance < 3f && (Input.GetKey(KeyCode.Space)) && Level.gameOver != true && Level.isRunning == false) 
        {
            Level.levelComplete = true;
            Level.timerStop = true;
        }
    }

}