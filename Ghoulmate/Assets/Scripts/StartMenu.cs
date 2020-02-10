using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    GameObject startUI, howtoplayUI, hiscoresUI, startTransition, exitTransition;

    public static bool heartStart;
    public static bool gameStart;

    void Start()
    {
        if (heartStart == true)
        {
            StartCoroutine("OpenTransition");
        }      
    }

    void Update()
    {
       
    }

    IEnumerator OpenTransition()
    {
        startTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        startTransition.SetActive(false);
        heartStart = false;
    }

    IEnumerator LevelTransition()
    {
        exitTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameStart = true; //Resets game score
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    public void StartGame() 
    {
        StartCoroutine("LevelTransition");    
    }

    public void HowToPlay()
    {
        startUI.SetActive(false);
        howtoplayUI.SetActive(true);
    }

    public void Hiscores()
    {
        startUI.SetActive(false);
        hiscoresUI.SetActive(true);
    }

    public void Quit() 
    {
        Application.Quit();
    }

    public void HiscoresReset()
    {
        PlayerPrefs.SetString("Score1", "0");
        PlayerPrefs.SetString("Score2", "0");
        PlayerPrefs.SetString("Score3", "0");
        PlayerPrefs.SetString("Score4", "0");
        GameObject.Find("Hiscores").GetComponent<HiscoresUI>().enabled = false;
        GameObject.Find("Hiscores").GetComponent<HiscoresUI>().enabled = true;
    }

    public void HowToPlayBack()
    {
        howtoplayUI.SetActive(false);
        startUI.SetActive(true);
    }

    public void HiscoresBack()
    {
        hiscoresUI.SetActive(false);
        startUI.SetActive(true);
    }
}
