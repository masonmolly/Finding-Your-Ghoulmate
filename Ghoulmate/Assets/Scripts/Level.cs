using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField]
    GameObject ghostAI, ghoulmate, player, startingTransition, exitTransition, levelInTransition, levelOutTransition;

    Vector3 ghoulmateSpawn;
    Vector3 playerSpawn;

    Renderer cubeColour, playerColour, ghoulmateColour;
    Camera cameraColour;
    Animator heart;

    bool hasEnded;
    float timerRaw;
    public static int timer;
    public static bool timerStop;
    public static int levelCount; //variable to hold score
    public static bool levelComplete; //variable to inform if level is complete
    public static bool gameOver;
    public static bool isRunning;
    public static string[] hiscoresString;
    public static int[] hiscores;

    /** COLOURS **/
    [SerializeField]
    Material cubeGreen, cubeBlue, cubeYellow, cubeOrange, cubeRed, cubePink, cubePurple, cubeBlack;
    [SerializeField]
    Material ghoulmatesGreen, ghoulmatesBlue, ghoulmatesYellow, ghoulmatesOrange, ghoulmatesRed, ghoulmatesPink, ghoulmatesPurple;

    Color32 cameraGreen = new Color32(198, 255, 189, 255);
    Color32 cameraBlue = new Color32(189, 242, 255, 255);
    Color32 cameraYellow = new Color32(255, 254, 201, 255);
    Color32 cameraOrange = new Color32(255, 213, 146, 255);
    Color32 cameraRed = new Color32(253, 178, 182, 255);
    Color32 cameraPink = new Color32(255, 218, 249, 255);
    Color32 cameraPurple = new Color32(222, 200, 251, 255);
    Color32 cameraBlack = new Color32(10, 9, 9, 255);

    int[] greenGhoulmates = { 2, 4 }; //Yellow, red
    int[] blueGhoulmates = { 0, 2, 5 }; //Green, yellow, red
    int[] yellowGhoulmates = { 0, 1, 3, 4, 5, 6 }; //Green, blue, orange, red, pink, purple
    int[] orangeGhoulmates = { 0, 1, 2, 5 }; //Green, blue, yellow, pink
    int[] redGhoulmates = { 0, 2 }; //Green, yellow
    int[] pinkGhoulmates = { 1, 2, 3 }; //Blue, yellow, orange
    int[] purpleGhoulmates = { 0, 1, 2, 3 }; //Green, blue, yellow, orange
    int[] blackGhoulmates = { 0, 1, 2, 3, 4, 5, 6 }; //Any colour

    void Start()
    {
        string transition;
        if (StartMenu.gameStart != true)
        {
            levelCount += 1;
            transition = "ContinueInTransition";
        }
        else
        {
            levelCount = 0;
            transition = "StartTransition";
            StartMenu.gameStart = false;     
        }
        levelComplete = false;
        hasEnded = false;
        timerStop = false;
        gameOver = false;
        NewLevel();
        StartCoroutine(transition);
    }

    void Update()
    {
        if (timerRaw <= 0 && hasEnded == false)
        {
            hasEnded = true;
            gameOver = true;
            timerStop = true;
            GameEnd();
        }
        else if (timerStop != true)
        {
            timerRaw -= Time.deltaTime;
            timer = Mathf.RoundToInt(timerRaw);
        }

        if (levelComplete == true)
        {
            levelComplete = false;
            StartCoroutine("LevelComplete");
        }
    }

    IEnumerator StartTransition()
    {
        startingTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        startingTransition.SetActive(false);
    }

    IEnumerator ContinueInTransition()
    {
        levelInTransition.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        levelInTransition.SetActive(false);
    }

    IEnumerator LevelComplete()
    {
        isRunning = true;
        heart = GameObject.Find("LevelHeart").GetComponentInChildren<Animator>();
        heart.SetBool("PopTime", true);
        yield return new WaitForSeconds(0.5f);
        levelOutTransition.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isRunning = false;
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    void NewLevel()
    {
        //GameObject.Find("Cube").transform.localScale = new Vector3(50, 50, 50);
        int smallTimer;
        int medTimer;
        int largeTimer;
        timerRaw = 10;
        ghoulmateSpawn = new Vector3(Random.Range(51, 8), 2.25f, Random.Range(3, 39));
        playerSpawn = new Vector3(Random.Range(51, 8), 2.25f, Random.Range(3, 39));

        Instantiate(player, playerSpawn, Quaternion.identity);
        Instantiate(ghoulmate, ghoulmateSpawn, Quaternion.identity);

        for (int i = 7; i <= 52; i += 3) 
        {
            for (int j = 2; j <= 40; j += 3) 
            {
                Instantiate(ghostAI, new Vector3(i, 2.25f, j), Quaternion.identity);
            }
        }

        ColourChooser();
    }

    void GameEnd() //Takes the achieved score, checks if high enough for hiscores, if so, sorts into the current hiscores
    {
        int score1 = int.Parse(PlayerPrefs.GetString("Score1", "0"));
        int score2 = int.Parse(PlayerPrefs.GetString("Score2", "0"));
        int score3 = int.Parse(PlayerPrefs.GetString("Score3", "0"));
        int score4 = int.Parse(PlayerPrefs.GetString("Score4", "0"));
        hiscores = new int[] { score4, score3, score2, score1 };

        if (levelCount > hiscores[0])
        {
            hiscores[0] = levelCount;
        }

        int len = hiscores.Length;
        for (int i = 0; i < len - 1; i++)
        {
            for (int j = 0; j < len - i - 1; j++)
            {
                if (hiscores[j] > hiscores[j + 1])
                {
                    int temp = hiscores[j];
                    hiscores[j] = hiscores[j + 1];
                    hiscores[j + 1] = temp;
                }
            }
        }

        PlayerPrefs.SetString("Score1", hiscores[3].ToString());
        PlayerPrefs.SetString("Score2", hiscores[2].ToString());
        PlayerPrefs.SetString("Score3", hiscores[1].ToString());
        PlayerPrefs.SetString("Score4", hiscores[0].ToString());
        StartMenu.gameStart = true;
    }

    void ColourChooser() //Chooses a random colour scheme each call, matching the background and cube, with contrasting ghoul and player colours
    {
        cameraColour = GameObject.Find("Main Camera").GetComponent<Camera>();
        cubeColour = GameObject.Find("Cube").GetComponent<MeshRenderer>();
        ghoulmateColour = GameObject.Find("Ghoulmate(Clone)").transform.Find("Sphere").GetComponent<SkinnedMeshRenderer>();
        playerColour = GameObject.Find("Player(Clone)").transform.Find("Sphere").GetComponent<SkinnedMeshRenderer>();
        ColourData green = ScriptableObject.CreateInstance<ColourData>();
        ColourData blue = ScriptableObject.CreateInstance<ColourData>();
        ColourData yellow = ScriptableObject.CreateInstance<ColourData>();
        ColourData orange = ScriptableObject.CreateInstance<ColourData>();
        ColourData red = ScriptableObject.CreateInstance<ColourData>();
        ColourData pink = ScriptableObject.CreateInstance<ColourData>();
        ColourData purple = ScriptableObject.CreateInstance<ColourData>();
        ColourData black = ScriptableObject.CreateInstance<ColourData>();
        green.Init("green", cameraGreen, cubeGreen, greenGhoulmates);
        blue.Init("blue", cameraBlue, cubeBlue, blueGhoulmates);
        yellow.Init("yellow", cameraYellow, cubeYellow, yellowGhoulmates);
        orange.Init("orange", cameraOrange, cubeOrange, orangeGhoulmates);
        red.Init("red", cameraRed, cubeRed, redGhoulmates);
        pink.Init("pink", cameraPink, cubePink, pinkGhoulmates);
        purple.Init("purple", cameraPurple, cubePurple, purpleGhoulmates);
        black.Init("black", cameraBlack, cubeBlack, blackGhoulmates);

        ColourData[] colours = { green, blue, yellow, orange, red, pink, purple };
        Material[] ghoulmates = { ghoulmatesGreen, ghoulmatesBlue, ghoulmatesYellow, ghoulmatesOrange, ghoulmatesRed, ghoulmatesPink, ghoulmatesPurple };
        int random = Random.Range(0, colours.Length);

        if (levelCount % 5 == 0  && levelCount != 0)
        {
            cameraColour.backgroundColor = black.colourCamera;
            cubeColour.material = black.colourCube;
            int randomBlack = Random.Range(0, black.colourGhoulmates.Length);
            ghoulmateColour.material = ghoulmates[black.colourGhoulmates[randomBlack]];
            playerColour.material = ghoulmates[black.colourGhoulmates[randomBlack]];
            return;
        }

        cameraColour.backgroundColor = colours[random].colourCamera;
        cubeColour.material = colours[random].colourCube;
        int random2 = Random.Range(0, colours[random].colourGhoulmates.Length);
        ghoulmateColour.material = ghoulmates[colours[random].colourGhoulmates[random2]];
        playerColour.material = ghoulmates[colours[random].colourGhoulmates[random2]];

    }

}
