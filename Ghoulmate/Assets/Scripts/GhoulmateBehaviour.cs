using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulmateBehaviour : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        while (player == null) { 
            player = GameObject.Find("Player(Clone)");
        }

        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        
        while (distance < 20f)
        {
            this.transform.position = new Vector3(Random.Range(49, 9), 2.25f, Random.Range(4, 34));
            distance = Vector3.Distance(this.transform.position, player.transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
