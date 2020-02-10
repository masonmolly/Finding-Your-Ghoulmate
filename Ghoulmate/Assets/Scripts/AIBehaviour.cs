using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    Transform target;
    float speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
