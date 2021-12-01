using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector3 horizontal;
    public GameObject wallPrefab;


    private bool hasScored = false;

    // Start is called before the first frame update
    void Start()
    {
        horizontal = new Vector3(speed, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.isDead == false && GlobalVars.gameState == 1) {
            if(wallPrefab.CompareTag("pform"))
            {
                horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
                transform.position = transform.position - (horizontal * Time.deltaTime);

                if (transform.position.x < -4.73f)
                {
                    Destroy(this.gameObject);
                }

                if (!hasScored && transform.position.x < -1.5f)
                {
                    GlobalVars.localScore++;
                    hasScored = true;
                }
            }
            else if (wallPrefab.CompareTag("tree"))
            {
                horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
                transform.position = transform.position - (horizontal * Time.deltaTime);

                if (transform.position.x < -4.73f)
                {
                    Destroy(this.gameObject);
                }

                if (!hasScored && transform.position.x < -1.5f)
                {
                    GlobalVars.localScore++;
                    hasScored = true;
                }
            }
            else if (wallPrefab.CompareTag("Cloud"))
            {
                horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
                transform.position = transform.position - (horizontal * Time.deltaTime);

                if (transform.position.x < -6.2f)
                {
                    Destroy(this.gameObject);
                }

                if (!hasScored && transform.position.x < -1.5f)
                {
                    GlobalVars.localScore++;
                    hasScored = true;
                }
            }
            else if (wallPrefab.CompareTag("Wall"))
            {
                horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
                transform.position = transform.position - (horizontal * Time.deltaTime);

                if (transform.position.x < -3.2f)
                {
                    Destroy(this.gameObject);
                }

                if (!hasScored && transform.position.x < -1.5f)
                {
                    GlobalVars.localScore++;
                    hasScored = true;
                }
            }
            
        }

    }

    void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("WALL COLLIDING");
            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("pform"))
        {
            Debug.Log("PFORM COLLIDING");

            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
