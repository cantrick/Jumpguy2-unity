using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputName : MonoBehaviour
{

    public InputField ipf;
    public Text errorText;
    string userName;
    private string deviceid;


    private void Start()
    {
        deviceid = SystemInfo.deviceUniqueIdentifier;
        errorText.enabled = false;
    }

    // Start is called before the first frame update
    public void SendNameToDB()
    {
        userName = ipf.text;

        if (userName == "")
        {
            errorText.text = "Username must not be blank";
        } else if (userName.Length > 12)
        {
            errorText.text = "Username must be 12 characters or less";
        } else
        {
            GetComponent<addUserScript>().CallAddUser(deviceid, userName);

            if (GlobalVars.dupeUser == false)
            {
                PlayerPrefs.SetString("userName", userName);
            }


            Debug.Log(userName);
        }

        
    }

}
