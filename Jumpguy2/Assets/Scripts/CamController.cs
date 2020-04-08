using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{

    GameObject ipf;

    // Start is called before the first frame update
    void Start()
    {
        //ipf = GameObject.Find("InputField");
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.gameState == 0)
        {
            transform.position = new Vector3(-30, 0, -10);

            /*
            if(PlayerPrefs.HasKey("userName"))
            {
                ipf.SetActive(false);
            } else
            {
                ipf.SetActive(true);
            }
            */

        }
        else if (GlobalVars.gameState == 3) //viewing scores
        {
            transform.position = new Vector3(-60, 0, -10);
        }
        else
        { 
            transform.position = new Vector3(0, 0, -10);
        }
    }
}
