using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* StartPlayerController.cs is responsible for the player controls within the start menu. */

public class StartPlayerController : MonoBehaviour
{
    float runSpeed = 10.0f;
    float walkSpeed = 2.0f;
    float rotateSpeed = 200.0f;
    Animator movement;
    Animator heart;

    void Start()
    {
        movement = gameObject.GetComponentInChildren<Animator>();
        heart = GameObject.Find("Heart").GetComponentInChildren<Animator>();
    }

    void Update()
    {
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

        else if ((Input.GetKey(KeyCode.Space)))
        {
            if (heart.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                heart.SetBool("SpinTime", true);
            }
            else
            {
                heart.SetBool("SpinTime", false); 
            }
        }
    }
}
