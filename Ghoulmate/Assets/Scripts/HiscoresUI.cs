using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiscoresUI : MonoBehaviour
{
    public Text score1;
    public Text score2;
    public Text score3;
    public Text score4;

    // Start is called before the first frame update
    void Start()
    {
        score1.text = PlayerPrefs.GetString("Score1", "0");
        score2.text = PlayerPrefs.GetString("Score2", "0");
        score3.text = PlayerPrefs.GetString("Score3", "0");
        score4.text = PlayerPrefs.GetString("Score4", "0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
