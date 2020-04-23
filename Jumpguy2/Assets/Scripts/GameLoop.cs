using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject jumpGuy;
    public GameObject wallPrefab;
    public Text Sky;
    public Text SkyHigh;


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
    //private string deviceid;

    float spawnChance = 0.0f;
    float last1 = 0;
    float last2 = 0;
    bool gotHSfromDB = false;

    public Component[] sRenderers;
    public float timeBetweenSpawn;
    public float elapsedTime;

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

        elapsedTime = 0.0f;

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
            ipf.SetActive(false);
            btnPlay.SetActive(true);
            btnScores.SetActive(true);
        }
        else
        {
            ipf.SetActive(true);
            btnScores.SetActive(false);
        }

        spawnChance = Random.Range(1, 700);

        debugButtonFunction();
        //menuHandler();

        // After play button is clicked, spawn the initial parts (jumpguy and the first wall)
        if (GlobalVars.gameState == 1 && startGame == false)
        {
            jgClone = Instantiate(jumpGuy, new Vector3(-1.5f, -1.5f, -1.2f), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f, -1.1f), 0), Quaternion.identity);
            startGame = true;
        }


        //if we're alive, spawn walls and move things
        if (GlobalVars.isDead == false && GlobalVars.gameState == 1)
        {
            timeBetweenSpawn = Random.Range(0.05f, 15.2f);
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
            Sky.enabled = true;
            SkyHigh.enabled = false;
            Sky.text = "Score: " + GlobalVars.localScore;
        }
        else if (GlobalVars.gameState == 0)
        {
            Sky.enabled = true;
            SkyHigh.enabled = false;
            Sky.text = "HIGH SCORE: " + GlobalVars.highScore;
        } else if (GlobalVars.gameState == 2)
        {
            Sky.enabled = true;
            SkyHigh.enabled = true;
            Sky.text = "Score: " + GlobalVars.localScore;
            SkyHigh.text = "HIGH SCORE: " + GlobalVars.highScore;
        } else if (GlobalVars.gameState == 3)
        {
            Sky.enabled = true;
            SkyHigh.enabled = false;
            Sky.text = "Global Highscores";
        }

        if(GlobalVars.isDead == true)
        {
            GlobalVars.gameState = 2;
            playerDies();
            menuHandler();
        }

    }

    void playerDies()
    {
        //display retry and exit buttons
        btnExit.SetActive(true);
        btnRetry.SetActive(true);
    }

    void spawnWalls() {

        //Debug.Log(spawnChance);
        GameObject tempObject = Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f, -1.1f), 0), Quaternion.identity);

        if(tempObject.transform.position.y <= -1.9f)
        {
            sRenderers = tempObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sRenderers)
            {
                sprite.color = Color.green;
            }
        } 
        else if (tempObject.transform.position.y > -1.5f)
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
                        if (PlayerPrefs.HasKey("userName"))
                        {
                            GlobalVars.gameState = 1;
                        }
                    }
                }
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

    void debugButtonFunction()
    {
        //if we click the mouse button
        if (Input.GetMouseButtonDown(0))
        {

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
                    Destroy(jgClone);
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
                    Destroy(jgClone);
                    startGame = false;
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
