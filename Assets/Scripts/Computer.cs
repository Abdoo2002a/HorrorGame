using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
public class Computer : MonoBehaviour
{
    public static Computer instance;

    public GameObject miniGame;

    [Header("the time to load the next level \"in secends\"")]
    public float TimeToLoadTheNextLevel;

    public GameObject ui;

    [HideInInspector]
    public GameObject floppyDiskInHand;
    public int levelCounter;

    public GameObject titleScreen;

    public Material crtMat;
    public Material GiltchMat;

    float timerGiltchingFreq;

    public GameObject computerScreen;

    bool startgame;

    bool a;
  
    

    public GameObject MiniGameCam;

    public GameObject computerView;

    public Dialogue dialogue;
    // the desk inside the computer;
    [HideInInspector]
    public FloppyDisk currentDesk;

    //to check 
    public bool thereIsFloopyDesk;

    public RenderTexture screen;

    bool playDialogue;

    public GameObject text;

    [HideInInspector] public bool isUsingComputer = false;

    public GameObject scoreUI;
    public GameObject timerUI;

    public GameObject[] bootUpScreen;


    float timeToFinichTheGame;
    float timer = 0;

    public bool plugedIn = false;


    bool countTime = false;
    bool isLoading = false;

    bool playSound;

    //when the timer runs out and the computer makes the beb sound
    [HideInInspector] public bool alertMode;
    bool inAlertMode;


    //the light coming from the screen when the game is running "change later but works for now" 
    public GameObject tvLight;
    public GameObject tvloadingScreen;

    private void Start()
    {
        alertMode = false;
        timerGiltchingFreq = 500;
        startgame = false;
        scoreUI = ui.transform.Find("score background").gameObject;
        //timerUI = ui.transform.Find("Timer").gameObject;
        playDialogue = true;
        computerView.SetActive(false);
        MiniGameCam.SetActive(false);
        thereIsFloopyDesk = false;
        instance = this;
        tvLight.SetActive(false);
        tvloadingScreen.SetActive(false);
        playSound = true;
    }


    private void Update()
    {
        //Debug.Log("is using the coumputer: " + isUsingComputer);
        //Debug.Log("in alert mode: " + inAlertMode);
        
        if (Input.GetKeyDown(KeyCode.G) && isUsingComputer && !inAlertMode)
        {
            turnOffComputer();
        }

       
        if (FirstMiniGame.instance != null && FirstMiniGame.instance.gameIsFinished)
        {
            takeOutCurrentFloppyDisk();
        }

        if (!startgame && Input.GetKeyDown(KeyCode.Return) && isUsingComputer && titleScreen.activeSelf)
        {
            startgame = true;
        }

        if (startgame)
        {
            timerGiltchingFreq = 500;
            StopAllCoroutines();
            titleScreen.SetActive(false);
            Invoke("spawnLevel", 0.2f);
            startgame = false;
        }


        if (!plugedIn)
        {

            if (currentDesk != null && alertMode)
            {
                AudioManager.instance.stopSound("alert sound");
                inAlertMode = false;
                alertMode = false;
                playSound = true;
                exitComputerMode(0f);
                Enemy.instanse.exexute = 0;
                StopAllCoroutines();

            }

        }
        else
        {
            if (alertMode)
            {
                alertMode = true;
                computerView.SetActive(false);
                TheFirstPerson.FPSController.instance.movementEnabled = true;
                floppyDiskInHand.transform.parent.gameObject.SetActive(true);
                isUsingComputer = false;
            }
        }

        Debug.Log(currentDesk);

    }
 

    //this code is hard coded , adjust it later 
    public void openComputer()
    {
        FloppyDisk d;
        //to check wether the player is holding in item or not 
        if (GameObject.Find("item holder").transform.childCount == 0)
        {
            Debug.Log("no item in hand ? ");
            return;
        }
        else
        {
            floppyDiskInHand = GameObject.Find("item holder").transform.GetChild(0).gameObject;
            d = floppyDiskInHand.GetComponent<FloppyDisk>();
        }


        // to check if the item that the player is holding is a floppy desk 


        if (d != null && !d.disk.isFinished)
        {

            //to get the floopy desk information;
            if (currentDesk == null)
            {
                currentDesk = d;
            }

            thereIsFloopyDesk = true;

            if (currentDesk != null && !alertMode)
            {
                timeToFinichTheGame = currentDesk.disk.timeToFinichTheGame;
                //to put the player in computer mode
                computerScreen.GetComponent<MeshRenderer>().material = crtMat;
                AudioManager.instance.playSound("computer sound");
                enterComputerMode();
                showBootupScreen();
                //this if statement id for showing the first dialogue only once, the first time he puts the disk in the computer 

            }
            else
            {
                Debug.Log("cant find the floopy disk");
                return;
            }

        }

    }


    //this is hard coded 
    void closeloading()
    {
        StopAllCoroutines();
        countTime = false;
        computerView.SetActive(false);
        tvloadingScreen.SetActive(false);
        tvLight.gameObject.SetActive(false);
        MiniGameCam.SetActive(false);
        isUsingComputer = false;
        TheFirstPerson.FPSController.instance.movementEnabled = true;
        AudioManager.instance.stopSound("computer sound");
        screen.Release();
    }



    // adjust the computer mode from here
    void enterComputerMode()
    {
        titleScreen.SetActive(false);
        isLoading = true;
        countTime = false;
        timer = timeToFinichTheGame;
        isUsingComputer = true;

        floppyDiskInHand.transform.parent.gameObject.SetActive(false);
        MiniGameCam.SetActive(true);
        computerView.SetActive(true); 
        tvLight.SetActive(true);
        tvLight.GetComponent<Light>().color = Color.white;
        
        TheFirstPerson.FPSController.instance.movementEnabled = false;
        Invoke("showTitleScreen", (float)bootUpScreen[1].GetComponent<VideoPlayer>().length);
    }

    public void spawnLevel()
    {
        //Instantiate(currentDesk.disk.levelManager, gameObject.transform).GetComponent<LevelManager>();

        Instantiate(currentDesk.disk.miniGame, gameObject.transform);
        //miniGame.SetActive(true);
        //for controlling the ui       
        //countTime = false;
        //isLoading = false;

        //tvloadingScreen.SetActive(false);

        tvLight.GetComponent<Light>().color = currentDesk.disk.screenLightColor;


        //if (currentDesk.disk.firstInsertion)
        //{
            //dialogue.startDialogue(currentDesk.disk.firstDialogue);
        //}
        //else
        //{
            //AudioManager.instance.playSound("mini game background music ");
            //countTime = true;
            //AudioManager.instance.playSound("timer sound");
            //StartCoroutine(timerGiltching());
        //}

    }

    void showBootupScreen()
    {
        StartCoroutine(startboot());
    }

    IEnumerator startboot()
    {
        bootUpScreen[0].gameObject.SetActive(true);
        bootUpScreen[1].GetComponent<VideoPlayer>().Play();
        yield return new WaitForSeconds((float)bootUpScreen[1].GetComponent<VideoPlayer>().length);
        bootUpScreen[0].gameObject.SetActive(false);
    }

    void exitComputerMode(float time)
    {
        
        if(FirstMiniGame.instance != null)
        {
            FirstMiniGame.instance.closeMiniGame();
        }       
        turnOffGameUI();
        inAlertMode = false;
        alertMode = false;
        thereIsFloopyDesk = false;
        floppyDiskInHand.transform.parent.gameObject.SetActive(true);
        Invoke("closeloading", 0.3f);    
    }


    public void takeOutCurrentFloppyDisk()
    {
        currentDesk.disk.isFinished = true;
        currentDesk = null;
        exitComputerMode(1f);
    }

    void turnOffGameUI()
    {

        scoreUI.SetActive(false);
        timerUI.SetActive(false);
    }
    void turnOnGameUI()
    {       
        scoreUI.SetActive(true);
        timerUI.SetActive(true);
    }

    void showTitleScreen()
    {

        tvLight.GetComponent<Light>().color = currentDesk.disk.gameTitleScreenColor;
        startgame = false;
        StartCoroutine(titleAnim());
        titleScreen.SetActive(true);
        titleScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = currentDesk.disk.gameTitle;
        titleScreen.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = currentDesk.disk.gameTitleScreenColor;
    }

    IEnumerator titleAnim()
    {
        while (true)
        {
            GameObject t = titleScreen.transform.Find("Text (TMP)").gameObject;
            t.SetActive(!t.activeSelf);
            yield return new WaitForSeconds(0.35f);
        }
    }

    public void glitchScreen(float time)
    {
        StartCoroutine(startGlitch(time));
    }

    IEnumerator startGlitch(float time)
    {
        AudioManager.instance.playSound("glitch sound");
        computerScreen.GetComponent<MeshRenderer>().material = GiltchMat;
        yield return new WaitForSeconds(time);
        AudioManager.instance.stopSound("glitch sound");
        computerScreen.GetComponent<MeshRenderer>().material = crtMat;
        yield break;
    }



    public void turnOffComputer()
    {
        StopAllCoroutines();
        timerGiltchingFreq = 1000;
        AudioManager.instance.stopSound("mini game background music ");
        AudioManager.instance.stopSound("timer sound");
        exitComputerMode(1f);
    }


}
