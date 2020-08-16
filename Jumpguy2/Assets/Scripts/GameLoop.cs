using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GameLoop : MonoBehaviour
{
    public GameObject jumpGuy;
    public GameObject wallPrefab;
    public GameObject platfPrefab;
    public Text Sky;
    public Text SkyHigh;
    public Text gpText;
    public Text ErrorText;
    public GameObject tree1;
    public GameObject tree2;
    public GameObject pCloud;


    Vector3 touchPosWorld;
    bool startGame = false;
    GameObject btnRetry;
    GameObject btnExit;
    GameObject btnPlay;
    GameObject btnScores;
    GameObject jgClone;
    GameObject ipf;
    GameObject scoreCanvas;
    GameObject spawnPoint;
    public Animator playAnimator;
    public Animator exitAnimator;
    public Animator retryAnimator;
    public Animator scoresAnimator;
    //public Animator scoreAnimator;
    //public Animator retryAnimator;
    public GameObject lNum;
    private int wallCount;
    //private string deviceid;

    float spawnChance = 0.0f;
    float last1 = 0;
    float last2 = 0;
    bool gotHSfromDB = false;
    bool adLoaded = false;

    public Component[] sRenderers;
    public float timeBetweenSpawn;
    public float elapsedTime;
    public Sprite num0;
    public Sprite num1;
    public Sprite num2;
    public Sprite num3;
    public Sprite num4;
    public Sprite num5;
    public Sprite num6;
    public Sprite num7;
    public Sprite num8;
    public Sprite num9;

    private BannerView bannerView;
    private string bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    private string appId = "ca-app-pub-3349476549916905~3847937716";
    // Start is called before the first frame update
    void Start()
    {
        //make buttons inactive until we need them
        btnRetry = GameObject.Find("btnRetry");
        btnExit = GameObject.Find("btnExit");
        btnPlay = GameObject.Find("btnPlay");
        btnScores = GameObject.Find("btnScores");
        ipf = GameObject.Find("InputField");
        scoreCanvas = GameObject.Find("ScoreCanvas");
        spawnPoint = GameObject.Find("SpawnPoint");

        btnExit.SetActive(false);
        btnRetry.SetActive(false);
        scoreCanvas.SetActive(false);

        ErrorText.enabled = false;

        MobileAds.Initialize(appId);
        this.RequestBanner();


        elapsedTime = 0.0f;
        Sky.color = Color.black;

        //get highscore from file
        //PlayerPrefs.SetInt("highscore", 0);
        GlobalVars.highScore = PlayerPrefs.GetInt("highscore");
        Debug.Log(SystemInfo.deviceUniqueIdentifier + "--" + PlayerPrefs.GetString("userName"));
        if (CheckInternet.isOnline == true)
        {
            if(!PlayerPrefs.HasKey("userId"))
            {
                //get the username only if there is one
                GetComponent<addUserScript>().CallGetUser(SystemInfo.deviceUniqueIdentifier);
                ipf.SetActive(true);

            }
            else
            {
                GlobalVars.userID = PlayerPrefs.GetInt("userId");
                btnPlay.SetActive(false);
                btnScores.SetActive(false);
            }
        }

        if (PlayerPrefs.HasKey("userName"))
        {
            ipf.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (PlayerPrefs.HasKey("userName"))
        {
            if(ErrorText.enabled == false)
            {
                btnPlay.SetActive(true);

            }
            ipf.SetActive(false);
            btnScores.SetActive(true);
        }
        else
        {
            ipf.SetActive(true);
            btnScores.SetActive(false);
        }

        debugButtonFunction();
        scoreSprites();
        //menuHandler();

        // After play button is clicked, spawn the initial parts (jumpguy and the first wall)
        if (GlobalVars.gameState == 1 && startGame == false)
        {
            jgClone = Instantiate(jumpGuy, new Vector3(-1.5f, -1.5f, -1.2f), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(3.2f, UnityEngine.Random.Range(-2.3f, -9.1f), 0), Quaternion.identity);
            startGame = true;
        }


        //if we're alive, spawn walls and move things
        if (GlobalVars.isDead == false && GlobalVars.gameState == 1)
        {
            timeBetweenSpawn = UnityEngine.Random.Range(0.05f, 15.2f);
            elapsedTime += Time.deltaTime;

            if(elapsedTime > timeBetweenSpawn)
            {
                spawnWalls();
                elapsedTime = 0.0f;
              
            }

        }

        //display score
        if (GlobalVars.gameState == 1)
        {
            Sky.enabled = false;
            gpText.enabled = false;
            SkyHigh.enabled = false;
            //Sky.text = "Score: " + GlobalVars.localScore;
        }
        else if (GlobalVars.gameState == 0)
        {
            Sky.enabled = true;
            gpText.enabled = false;
            SkyHigh.enabled = false;
            Sky.text = "HIGH SCORE: " + GlobalVars.highScore;
        } else if (GlobalVars.gameState == 2)
        {
            Sky.enabled = true;
            gpText.enabled = false;
            //SkyHigh.enabled = true;
            //Sky.text = "Score: " + GlobalVars.localScore;
            Sky.text = "HIGH SCORE: " + GlobalVars.highScore;
        } else if (GlobalVars.gameState == 3)
        {
            gpText.enabled = true;
            ErrorText.enabled = false;
            SkyHigh.enabled = true;
            Sky.enabled = false;
            gpText.text = "Your current position: " + GlobalVars.globalPos;
            SkyHigh.text = "Global Highscores";

        }

        if (GlobalVars.isDead == true)
        {
            GlobalVars.gameState = 2;
            playerDies();
            menuHandler();
        }

    }

    private void RequestBanner()
    {
        this.bannerView = new BannerView(bannerAdId, AdSize.SmartBanner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

    }

    private void ShowAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        this.bannerView.LoadAd(request);
        adLoaded = true;
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    void playerDies()
    {
        //display retry and exit buttons
        btnExit.SetActive(true);
        btnRetry.SetActive(true);
        if(adLoaded == false)
        {
            ShowAd();
            adLoaded = true;
        }
    }

    void spawnWalls() {
        GameObject tempObject;

        //Debug.Log(spawnChance);
        if(wallCount > 5)
        {
            tempObject = Instantiate(platfPrefab, new Vector3(5.1f, UnityEngine.Random.Range(-3.47f, -2.0f), 0), Quaternion.identity);
            wallCount = 0;
        }
        else
        {
            tempObject = Instantiate(wallPrefab, new Vector3(3.2f, UnityEngine.Random.Range(-3f, -1.18f), 0), Quaternion.identity);

            if (tempObject.transform.position.y <= -2.3f)
            {
                sRenderers = tempObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sRenderers)
                {
                    sprite.color = Color.green;
                }
            }
            else if (tempObject.transform.position.y > -1.52f)
            {
                sRenderers = tempObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sRenderers)
                {
                    sprite.color = Color.red;
                }
            }
            else
            {
                sRenderers = tempObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprite in sRenderers)
                {
                    sprite.color = Color.yellow;
                }
            }
        }

        if(wallCount%3 == 0)
        {
            int temprand = UnityEngine.Random.Range(0, 3);
            if (temprand <= 1)
            {
                Instantiate(tree1, new Vector3(5.1f, -0.1f, 0.5f), Quaternion.identity);

            }
            else
            {
                Instantiate(tree2, new Vector3(5.1f, 0.66f, 0.5f), Quaternion.identity);

            }
        }
        else if(wallCount%4 == 0)
        {
            Instantiate(pCloud, new Vector3(6.17f, UnityEngine.Random.Range(0.9f, 5.05f), 2.0f), Quaternion.identity);
        }


        wallCount += 1;

    }

    void menuHandler()
    {
        //if we're in the menu:
        if (GlobalVars.gameState == 0)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                if (hitInformation.collider != null)
                {
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    //if it's the play button, let's play the game
                    if (touchedObject.name == "btnPlay")
                    {
                        Debug.Log("PLAY");
                        playAnimator.SetBool("pushPlay", true);
                        if (PlayerPrefs.HasKey("userName"))
                        {
                            GlobalVars.gameState = 1;
                        }
                    } else
                    {
                        //playAnimator.SetBool("pushPlay", false);
                    }
                }
            } else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {

            }
        }
        else if (GlobalVars.gameState == 2 && GlobalVars.isDead == true)
        {
            //this is when we would see the RETRY or EXIT buttons in game
            

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                if (hitInformation.collider != null)
                {
                    Debug.Log("death222");
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    if (touchedObject.name == "btnRetry")
                    {
                        Debug.Log("retry touch");
                        
                        //Delete walls, move background/foreground/jumpguy back to initial positions
                        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
                        {
                            Destroy(o);
                        }
                        
                        Destroy(jgClone);
                        startGame = false;
                        btnExit.SetActive(false);
                        btnRetry.SetActive(false);

                        //do score logic:
                        if (GlobalVars.localScore > GlobalVars.highScore)
                        {
                            GlobalVars.highScore = GlobalVars.localScore;
                            PlayerPrefs.SetInt("highscore", GlobalVars.highScore);
                            GlobalVars.localScore = 0;
                        }
                        else
                        {
                            GlobalVars.localScore = 0;
                        }

                        GlobalVars.gameState = 1;
                        GlobalVars.isDead = false;
                    }
                    else if (touchedObject.name == "btnExit")
                    {
                        Debug.Log("exit touch");
                        //Delete walls, move background/foreground/jumpguy back to initial positions
                        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
                        {
                            Destroy(o);
                        }
                        
                        Destroy(jgClone);
                        startGame = false;
                        btnExit.SetActive(false);
                        btnRetry.SetActive(false);

                        //do score logic:
                        if (GlobalVars.localScore > GlobalVars.highScore)
                        {
                            GlobalVars.highScore = GlobalVars.localScore;
                            PlayerPrefs.SetInt("highscore", GlobalVars.highScore);
                            GlobalVars.localScore = 0;
                        }
                        else
                        {
                            GlobalVars.localScore = 0;
                        }

                        GlobalVars.gameState = 0;
                        GlobalVars.isDead = false;
                    }
                }
            }
        }
    }


    void scoreSprites()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Score"))
        {
            Destroy(o);
        }

        char[] ca = GlobalVars.localScore.ToString().ToCharArray();

        GameObject tempHolder = GameObject.Find("Numbers");

        for(int i = 0;i<ca.Length;i++)
        {

            GameObject tempNumObject = Instantiate(lNum,
                new Vector3(tempHolder.transform.position.x+((float)i/2),
                            tempHolder.transform.position.y,
                            tempHolder.transform.position.z)
                ,Quaternion.identity);

            switch (ca[i])
            {
                case '0':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num0;
                    break;
                case '8':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num8;
                    break;
                case '1':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num1;
                    break;
                case '2':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num2;
                    break;
                case '3':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num3;
                    break;
                case '4':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num4;
                    break;
                case '5':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num5;
                    break;
                case '6':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num6;
                    break;
                case '7':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num7;
                    break;
                case '9':
                    tempNumObject.GetComponent<SpriteRenderer>().sprite = num9;
                    break;
            }

        }
    }

    void debugButtonFunction()
    {
        //Handle button animations:
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "btnPlay")
                {
                    playAnimator.SetBool("pushPlay", true);
                }
                else if (hit.collider.gameObject.name == "btnRetry")
                {
                    retryAnimator.SetBool("pressRetry", true);
                }
                else if (hit.collider.gameObject.name == "btnExit")
                {
                    //exitAnimator.SetBool("pushExit", true);
                }
                else if (hit.collider.gameObject.name == "btnScores")
                {
                    scoresAnimator.SetBool("pushScores", true);
                }
            }
        }

        //if we click the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            playAnimator.SetBool("pushPlay", false);
            retryAnimator.SetBool("pressRetry", false);
            //exitAnimator.SetBool("pushExit", false);
            scoresAnimator.SetBool("pushScores", false);

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                //handle this for the PLAY button, gameState = 0
                if (hit.collider.gameObject.name == "btnPlay")
                { 
                    Debug.Log("PLAY");
                    Debug.Log(GlobalVars.userID + " : " + PlayerPrefs.GetInt("userId") + " : " + PlayerPrefs.GetString("userName") + " : " + PlayerPrefs.GetInt("highscore"));

                    if (PlayerPrefs.HasKey("userName"))
                    {
                        ipf.SetActive(false);
                        GlobalVars.gameState = 1;
                    }

                }
                //handle this for the PLAY button, gameState = 2
                else if (hit.collider.gameObject.name == "btnRetry")
                {
                    Debug.Log("retry touch");
                    //Delete walls, move background/foreground/jumpguy back to initial positions
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
                    {
                        Destroy(o);
                    }
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("pform"))
                    {
                        Destroy(o);
                    }
                    Destroy(jgClone);
                    this.bannerView.Destroy();
                    this.RequestBanner();
                    adLoaded = false;
                    startGame = false;
                    btnExit.SetActive(false);
                    btnRetry.SetActive(false);

                    //do score logic:
                    if (GlobalVars.localScore > GlobalVars.highScore)
                    {
                        GlobalVars.highScore = GlobalVars.localScore;
                        PlayerPrefs.SetInt("highscore",GlobalVars.highScore);
                        if (CheckInternet.isOnline == true)
                        {
                            GetComponent<HSController>().CallAddScore(PlayerPrefs.GetInt("userId").ToString(), PlayerPrefs.GetInt("highscore").ToString());
                        }
                        GlobalVars.localScore = 0;
                    }
                    else
                    {
                        GlobalVars.localScore = 0;
                    }

                    GlobalVars.gameState = 1;
                    GlobalVars.isDead = false;
                }
                //handle this for the PLAY button, gameState = 2
                else if (hit.collider.gameObject.name == "btnExit")
                {
                    Debug.Log("exit touch");
                    //Delete walls, move background/foreground/jumpguy back to initial positions
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
                    {
                        Destroy(o);
                    }
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("pform"))
                    {
                        Destroy(o);
                    }
                    Destroy(jgClone);
                    startGame = false;
                    this.bannerView.Destroy();
                    this.RequestBanner();
                    adLoaded = false;
                    btnExit.SetActive(false);
                    btnRetry.SetActive(false);

                    //do score logic:
                    if (GlobalVars.localScore > GlobalVars.highScore)
                    {
                        GlobalVars.highScore = GlobalVars.localScore;
                        PlayerPrefs.SetInt("highscore", GlobalVars.highScore);
                        if (CheckInternet.isOnline == true)
                        {
                            GetComponent<HSController>().CallAddScore(PlayerPrefs.GetInt("userId").ToString(), PlayerPrefs.GetInt("highscore").ToString());
                        }
                        GlobalVars.localScore = 0;
                    }
                    else
                    {
                        GlobalVars.localScore = 0;
                    }

                    GlobalVars.gameState = 0;
                    GlobalVars.isDead = false;
                }
                else if (hit.collider.gameObject.name == "btnScores")
                {

                    if (CheckInternet.isOnline == true)
                    {
                        ErrorText.enabled = true;
                        ErrorText.text = "Loading scores...";
                        btnPlay.SetActive(false);
                        GetComponent<HSController>().CallGetScore(); 
                    }
                    else
                    {
                        //TODO make it so high score list just says "Not connected to internet"
                    }
                    scoreDisplay.getScore = false;
                    scoreCanvas.SetActive(true);
                }
                else if (hit.collider.gameObject.name == "btnExit2")
                {
                    GlobalVars.gameState = 0;
                    btnPlay.SetActive(true);
                    //delete the list items:
                    for (int i = spawnPoint.transform.childCount - 1; i >= 0; i--)
                    {
                        GameObject.Destroy(spawnPoint.transform.GetChild(i).gameObject);
                    }
                    scoreCanvas.SetActive(false);
                }
            }
        }
    }
}