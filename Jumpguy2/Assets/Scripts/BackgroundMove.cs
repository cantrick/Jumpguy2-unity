using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 horizontal;
    public GameObject bgPrefab;

    bool bgSpawn = true;
    bool bg2Spawn = true;
    private float bgRand = 0;



    // Start is called before the first frame update
    void Start()
    {
        horizontal = new Vector3(speed,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.isDead == false && GlobalVars.gameState == 1)
        {
            if(this.tag == "bg2")
            {
                //bgRand = Random.Range(0f,0.36f);
                horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
                transform.position = transform.position - (horizontal * Time.deltaTime);

                //spawn background
                if ((transform.position.x < -6.6f) && bg2Spawn == true)
                {
                    Instantiate(bgPrefab, new Vector3(12.55f, 0.3f, 1), Quaternion.identity);
                    bg2Spawn = false;
                }
                else if (transform.position.x < -13.0f)
                {
                    Destroy(this.gameObject);
                }
            }
            else if(this.tag == "bg1" || this.tag == "Cloud" || this.tag == "tree")
            {
                bgRand = Random.Range(0.24f, 0.69f);
                horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
                transform.position = transform.position - (horizontal * Time.deltaTime);

                //spawn background
                if ((transform.position.x < -2.91f) && bgSpawn == true)
                {
                    Instantiate(bgPrefab, new Vector3(13.14f, bgRand, 2), Quaternion.identity);
                    bgSpawn = false;
                }
                else if (transform.position.x < -13.03f)
                {
                    Destroy(this.gameObject);
                }
            }
            
        }
    }
}
