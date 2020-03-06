using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtonScript : MonoBehaviour
{

    GameObject btnRetry;
    GameObject btnExit;

    // Start is called before the first frame update
    void Start()
    {
        btnRetry = GameObject.Find("btnRetry");
        btnExit = GameObject.Find("btnExit");
    }

    // Update is called once per frame
    void Update()
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
                    Debug.Log("death touch");
                    //Delete walls, move background/foreground/jumpguy back to initial positions
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
                    {
                        Destroy(o);
                    }
                    //jumpGuy.transform.position = new Vector3(-1.5f, -1.5f, -1);
                    btnExit.SetActive(false);
                    btnRetry.SetActive(false);
                    GlobalVars.gameState = 1;
                    GlobalVars.isDead = false;
                }
                else if (hit.collider.gameObject.name == "btnExit")
                {
                    Debug.Log("death touch");
                    //Delete walls, move background/foreground/jumpguy back to initial positions
                    foreach (GameObject o in GameObject.FindGameObjectsWithTag("Wall"))
                    {
                        Destroy(o);
                    }
                    Destroy(GameObject.Find("Jumpguy"));
                    btnExit.SetActive(false);
                    btnRetry.SetActive(false);
                    GlobalVars.gameState = 0;
                    GlobalVars.isDead = false;
                }
            }
        }
    }
}
