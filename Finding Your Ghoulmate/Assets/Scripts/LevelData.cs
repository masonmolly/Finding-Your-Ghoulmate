using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* LevelData.cs is a scriptable object responsible for holding level difficulty data. */

public class LevelData : ScriptableObject
{
    public string size;
    public float timer;
    public float distance;
    public Vector3 cameraLocation;
    public Vector3 cubeSize;
    public int[] spawnCoords; //Array of 4 coords in order - minX, maxX, minZ, maxZ

    public LevelData()
    {
    }

    public void Init(string name, float timer, float minDistance, Vector3 camera, Vector3 cube, int[] spawns)
    {
        size = name;
        this.timer = timer;
        distance = minDistance; //minimum spawn distance between player and ghoulmate
        cameraLocation = camera;
        cubeSize = cube;
        spawnCoords = spawns;
    }
}
