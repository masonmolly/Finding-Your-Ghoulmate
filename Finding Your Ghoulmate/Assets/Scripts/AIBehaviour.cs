using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* AIBehaviour.cs is responsible for the AI ghoul behaviour - looking at the player, moving towards the player, and moving away from the player if within range. */

public class AIBehaviour : MonoBehaviour
{
    Transform target;
    float speed = 4.0f;

    void Start()
    {

    }

    void Update()
    {
        if (target == null)
        {
            target = GameObject.Find("Player(Clone)").transform;
        }

        if (target != null)
        {
            transform.LookAt(target);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            float distance = Vector3.Distance(this.transform.position, target.position);

            if (distance > 10f)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }

    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name == "Player(Clone)")
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
    }
}
