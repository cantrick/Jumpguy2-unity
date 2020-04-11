﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 horizontal;
    public GameObject bgPrefab;

    bool bgSpawn = true;



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
            horizontal = new Vector3(speed + (float)(GlobalVars.localScore / 19.0f), 0, 0);
            transform.position = transform.position - (horizontal * Time.deltaTime);

            //spawn background
            if ((transform.position.x < -2.3f) && bgSpawn == true)
            {
                Instantiate(bgPrefab, new Vector3(7.85f, 0, 2), Quaternion.identity);
                bgSpawn = false;
            }
            else if (transform.position.x < -7.93f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
