using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GhoulmateBehaviour.cs is responsible for the ghoulmate behaviour - respawning if initially too close to player, and looking at the player. */

public class GhoulmateBehaviour : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        while (player == null) { 
            player = GameObject.Find("Player(Clone)");
        }

        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        
        while (distance < Level.levelSize.distance)
        {
            this.transform.position = new Vector3(Random.Range(Level.levelSize.spawnCoords[0], Level.levelSize.spawnCoords[1]), 2.25f, Random.Range(Level.levelSize.spawnCoords[2], Level.levelSize.spawnCoords[3]));
            distance = Vector3.Distance(this.transform.position, player.transform.position);
        }

    }

    void Update()
    {
        transform.LookAt(player.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
