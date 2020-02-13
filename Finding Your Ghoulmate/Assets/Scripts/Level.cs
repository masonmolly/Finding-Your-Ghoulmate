using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Level.cs is responsible for spawning in all characters, and determining level size, colour, and transitions. */

public class Level : MonoBehaviour
{
    [SerializeField]
    GameObject ghostAI, ghoulmate, player, startingTransition, exitTransition, levelInTransition, levelOutTransition;

    bool hasEnded;
    float timerRaw;
    public static int timer;
    public static bool timerStop;
    public static int levelCount; //variable to hold score
    public static bool levelComplete; //variable to inform if level is complete
    public static bool gameOver;
    public static bool isRunning;

    /** LEVEL SIZES **/
    public static LevelData levelSize;
    Vector3 smallCamera = new Vector3(30.41f, 32.82f, 62.19f);
    Vector3 mediumCamera = new Vector3(30.41f, 38, 70);
    Vector3 largeCamera = new Vector3(30.41f, 44.54f, 79.44f);
    Vector3 smallCube = new Vector3(40, 5, 30);
    Vector3 mediumCube = new Vector3(50, 5, 40);
    Vector3 largeCube = new Vector3(60, 5, 50);
    int[] smallSpawns = new int[] { 13, 47, 8, 32 };
    int[] mediumSpawns = new int[] { 7, 52, 2, 40 };
    int[] largeSpawns = new int[] { 3, 57, -2, 42 };

    /** COLOURS **/
    [SerializeField]
    Material cubeGreen, cubeBlue, cubeYellow, cubeOrange, cubeRed, cubePink, cubePurple, cubeBlack;
    [SerializeField]
    Material AImaterial, ghoulmatesGreen, ghoulmatesBlue, ghoulmatesYellow, ghoulmatesOrange, ghoulmatesRed, ghoulmatesPink, ghoulmatesPurple;

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
        if (StartMenu.gameStart != true) //runs when scene is reloaded, so for start of every new level
        {
            levelCount += 1;
            transition = "ContinueInTransition"; //fade transition
        }
        else //runs when game starts
        {
            levelCount = 0;
            transition = "StartTransition"; //heart transition
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
        if (timerRaw <= 0 && hasEnded == false) //Game over trigger
        {
            hasEnded = true; //hasEnded prevents GameEnd() from being called multiple times per level
            gameOver = true;
            timerStop = true; //timer stops when level ends
            GameEnd();
        }
        else if (timerStop != true)
        {
            timerRaw -= Time.deltaTime;
            timer = Mathf.RoundToInt(timerRaw);
        }

        if (levelComplete == true) //Level complete trigger
        {
            levelComplete = false;
            StartCoroutine("LevelComplete");
        }
    }

    IEnumerator StartTransition() //Coroutine for first loading the levels (heart fade in)
    {
        startingTransition.SetActive(true);
        yield return new WaitForSeconds(1f);
        startingTransition.SetActive(false);
    }

    IEnumerator ContinueInTransition() //Coroutine for loading a subsequent level (plain fade in)
    {
        levelInTransition.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        levelInTransition.SetActive(false);
    }

    IEnumerator LevelComplete() //Coroutine for completing a level (heart animation, plain fade out, reload scene)
    {
        isRunning = true; //isRunning prevents LevelComplete() from being called multiple times
        AudioSource levelSound = this.GetComponent<AudioSource>();
        levelSound.Play();
        Animator heart = GameObject.Find("LevelHeart").GetComponentInChildren<Animator>();
        heart.SetBool("PopTime", true); //triggers heart animation at end of level
        yield return new WaitForSeconds(0.5f);
        levelOutTransition.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isRunning = false;
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    void NewLevel() //Spawns a new level each call, basing the level size on current score, and spawning the player & ghoulmate in random place
    {
        LevelData small = ScriptableObject.CreateInstance<LevelData>();
        small.Init("small", 6f, 10f, smallCamera, smallCube, smallSpawns);
        levelSize = small;

        if (levelCount > 10 && levelCount < 21)
        {
            LevelData medium = ScriptableObject.CreateInstance<LevelData>();
            medium.Init("medium", 12f, 15f, mediumCamera, mediumCube, mediumSpawns);
            levelSize = medium;
        }
        else if (levelCount > 20)
        {
            LevelData large = ScriptableObject.CreateInstance<LevelData>();
            large.Init("large", 14f, 20f, largeCamera, largeCube, largeSpawns);
            levelSize = large;
        }

        timerRaw = levelSize.timer;
        timerRaw = timerRaw - (levelCount * 0.1f);
        GameObject.Find("Cube").transform.localScale = levelSize.cubeSize;
        GameObject.Find("Main Camera").transform.position = levelSize.cameraLocation;
        Vector3 ghoulmateSpawn = new Vector3(Random.Range(levelSize.spawnCoords[0], levelSize.spawnCoords[1]), 2.25f, Random.Range(levelSize.spawnCoords[2], levelSize.spawnCoords[3]));
        Vector3 playerSpawn = new Vector3(Random.Range(levelSize.spawnCoords[0], levelSize.spawnCoords[1]), 2.25f, Random.Range(levelSize.spawnCoords[2], levelSize.spawnCoords[3]));

        Instantiate(player, playerSpawn, Quaternion.identity);
        Instantiate(ghoulmate, ghoulmateSpawn, Quaternion.identity);

        for (int i = levelSize.spawnCoords[0]; i <= levelSize.spawnCoords[1]; i += 3) 
        {
            for (int j = levelSize.spawnCoords[2]; j <= levelSize.spawnCoords[3]; j += 3) 
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
        int[] hiscores = new int[] { score4, score3, score2, score1 };

        if (levelCount > hiscores[0])
        {
            hiscores[0] = levelCount;
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
        }

        StartMenu.gameStart = true;
    }

    void ColourChooser() //Chooses a random colour scheme each call, matching the background and cube, with contrasting ghoul and player colours
    {
        Renderer cubeColour, playerColour, ghoulmateColour;
        Camera cameraColour;
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

        if (levelCount % 10 == 0  && levelCount != 0) //Every 10 levels spawns a dark scene, only light visible is the player
        {
            timerRaw += 5;
            cameraColour.backgroundColor = black.colourCamera;
            cubeColour.material = black.colourCube;

            int randomBlack = Random.Range(0, black.colourGhoulmates.Length);
            ghoulmateColour.material = ghoulmates[black.colourGhoulmates[randomBlack]];
            playerColour.material = ghoulmates[black.colourGhoulmates[randomBlack]];
            Light light = GameObject.Find("Player(Clone)").transform.Find("Light").GetComponent<Light>(); ;
            light.enabled = true;

            //Following section disables emission light source on all AI and ghoulmate
            Renderer heart = GameObject.Find("LevelHeart").GetComponent<Renderer>();
            heart.material.DisableKeyword("_EMISSION");
            ghoulmateColour.material.DisableKeyword("_EMISSION");
            GameObject[] ais = GameObject.FindGameObjectsWithTag("AI");
            for (int i = 0; i < ais.Length; i++)
            {
                Renderer cur = ais[i].transform.Find("Sphere").GetComponent<SkinnedMeshRenderer>();
                cur.material.DisableKeyword("_EMISSION");
            }
            return;
        }

        cameraColour.backgroundColor = colours[random].colourCamera;
        cubeColour.material = colours[random].colourCube;
        int random2 = Random.Range(0, colours[random].colourGhoulmates.Length);
        if (levelCount % 5 == 0 && levelCount != 0) //Every level ending in 5 ghoulmate is same colour as AI
        {
            ghoulmateColour.material = AImaterial;
        }
        else
        {
            ghoulmateColour.material = ghoulmates[colours[random].colourGhoulmates[random2]];
        }
        playerColour.material = ghoulmates[colours[random].colourGhoulmates[random2]];

    }

}
