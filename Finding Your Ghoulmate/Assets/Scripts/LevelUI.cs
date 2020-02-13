using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* LevelUI.cs is responsible for keeping the score and timer updated within the level UI. */

public class LevelUI : MonoBehaviour
{
    public Text score;
    public Text timer;

    void Start()
    {
        
    }

    void Update()
    {
        score.text = Level.levelCount.ToString();
        timer.text = Level.timer.ToString();
    }
}
