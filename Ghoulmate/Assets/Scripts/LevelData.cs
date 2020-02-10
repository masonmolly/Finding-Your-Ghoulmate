using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public string size;
    public float timer;
    public Vector3 ghoulmatesSpawn;
    public Vector3 cubeSize;
    public Vector3 cameraLocation;
    //public int

    public LevelData()
    {
    }

    public void Init(string name, Color32 cameraColour, Material cubeColour, int[] ghoulmatesColour)
    {
        size = name;
        //colourCamera = cameraColour;
        //colourCube = cubeColour;
        //colourGhoulmates = ghoulmatesColour;
    }
}
