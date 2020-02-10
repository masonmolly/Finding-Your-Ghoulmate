using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] 
    GameObject pauseUI, gameOverUI, exitTransition;
    string scene;

    public Text finalScore;
    public static bool pause;

    // Start is called before the first frame update
    void Start()
    {
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && Level.gameOver != true) //also need to add check for if game over is false (game over bool)
        {
            if (pause == false)
            {
                pause = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else 
            {
                Resume();
            }
        }

        if ((Level.gameOver == true))
        {
            gameOverUI.SetActive(true);
            finalScore.text = Level.levelCount.ToString();
        }
    }

    public void Resume()
    {
        pause = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Replay()
    {
        scene = "Level";
        Time.timeScale = 1.0f;
        StartCoroutine("Transition");
    }

    public void Home()
    {
        scene = "Start";
        StartMenu.heartStart = true;
        Time.timeScale = 1.0f;
        StartCoroutine("Transition");
    }

    IEnumerator Transition()
    {
        exitTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
