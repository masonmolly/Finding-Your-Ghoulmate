using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Music.cs is responsible for keeping the background music playing throughout all scenes. */

public class Music : MonoBehaviour
{
    public static GameObject playing;

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
        if (playing == null)
        {
            playing = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
