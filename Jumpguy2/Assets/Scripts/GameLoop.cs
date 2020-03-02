using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject jumpGuy;
    public GameObject wallPrefab;
    public Text Sky;

    Vector3 touchPosWorld;
    bool startGame = false;

    float spawnChance = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // After play button is clicked, spawn the initial parts (jumpguy and the first wall)
        if (GlobalVars.camState == 1 && startGame == false)
        {
            Instantiate(jumpGuy, new Vector3(-1.5f, -1.5f, -1), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f, -1.1f), 0), Quaternion.identity);
            startGame = true;
        }

        menuHandler();

        //if we're alive, spawn walls and move things
        if (GlobalVars.isDead == false && GlobalVars.camState == 1)
        {
            spawnChance = Random.Range(0, 700);
            //spawn wall
            if (spawnChance > 0 && spawnChance < 8)
            {
                Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f, -1.1f), 0), Quaternion.identity);
            }
        }

        //display score
        if (GlobalVars.camState == 1)
        {
            Sky.enabled = true;
            Sky.text = "Score: " + GlobalVars.localScore;
        } else
        {
            Sky.enabled = false;
        }

    }

    void menuHandler()
    {
        //if we're in the menu:
        if (GlobalVars.camState == 0)
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
                        GlobalVars.camState = 1;
                    }
                }
            }
        }
        else if (GlobalVars.camState == 1 && GlobalVars.isDead == true)
        {
            //this is when we would see the RETRY or EXIT buttons in game
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
                RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

                if (hitInformation.collider != null)
                {
                    //We should have hit something with a 2D Physics collider!
                    GameObject touchedObject = hitInformation.transform.gameObject;
                    if (touchedObject.name == "btnRetry")
                    {
                        //Delete walls, move background/foreground/jumpguy back to initial positions

                    }
                }
            }
        }
    }
}
