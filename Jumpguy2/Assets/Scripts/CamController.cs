using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.gameState == 0)
        {
            transform.position = new Vector3(-30,0,-10);
        } else
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }
}
