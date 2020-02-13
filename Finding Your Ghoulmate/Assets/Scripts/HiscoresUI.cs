using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* HiscoresUI.cs is responsible for displaying the high scores within the hiscore UI in the start menu. */

public class HiscoresUI : MonoBehaviour
{
    public Text score1;
    public Text score2;
    public Text score3;
    public Text score4;

    void Start()
    {
        score1.text = PlayerPrefs.GetString("Score1", "0");
        score2.text = PlayerPrefs.GetString("Score2", "0");
        score3.text = PlayerPrefs.GetString("Score3", "0");
        score4.text = PlayerPrefs.GetString("Score4", "0");
    }

    void Update()
    {
        if (StartMenu.reset == true)
        {
            score1.text = PlayerPrefs.GetString("Score1", "0");
            score2.text = PlayerPrefs.GetString("Score2", "0");
            score3.text = PlayerPrefs.GetString("Score3", "0");
            score4.text = PlayerPrefs.GetString("Score4", "0");
            StartMenu.reset = false;
        }
    }
}
