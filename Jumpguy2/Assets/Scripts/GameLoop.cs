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
    GameObject jgClone;

    float spawnChance = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //make buttons inactive until we need them
        btnRetry = GameObject.Find("btnRetry");
        btnExit = GameObject.Find("btnExit");

        btnExit.SetActive(false);
        btnRetry.SetActive(false);

        //get highscore from file
        GlobalVars.highScore = PlayerPrefs.GetInt("highscore");
    }

    // Update is called once per frame
    void Update()
    {
        //debugButtonFunction();
        menuHandler();

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
            spawnChance = Random.Range(0, 700);
            //spawn wall
            if (spawnChance > 0 && spawnChance < 10)
            {
                Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f, -1.1f), 0), Quaternion.identity);
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
        } else
        {
            Sky.enabled = true;
            SkyHigh.enabled = true;
            Sky.text = "Score: " + GlobalVars.localScore;
            SkyHigh.text = "HIGH SCORE: " + GlobalVars.highScore;
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
                        GlobalVars.gameState = 1;
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
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.name == "btnPlay")
                {
                    Debug.Log("PLAY");
                    GlobalVars.gameState = 1;
                }
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
                        GlobalVars.localScore = 0;
                    }
                    else
                    {
                        GlobalVars.localScore = 0;
                    }

                    GlobalVars.gameState = 1;
                    GlobalVars.isDead = false;
                }
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
