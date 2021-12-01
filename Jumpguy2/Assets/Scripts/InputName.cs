using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class InputName : MonoBehaviour
{

    public InputField ipf;
    public Text errorText;
    public TextAsset badWords;
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
            errorText.enabled = true;
            errorText.text = "Username must not be blank";
        } else if (userName.Length > 12)
        {
            errorText.enabled = true;
            errorText.text = "Username must be 12 characters or less";
        } else if (badName(userName)) {
            errorText.enabled = true;
            errorText.text = "Username contains BAD words. Try a new name.";
        } else
        {
            errorText.enabled = false;
            GetComponent<addUserScript>().CallAddUser(deviceid, userName);

            if (GlobalVars.dupeUser == false)
            {
                PlayerPrefs.SetString("userName", userName);
            }


            Debug.Log(userName);
        }

        
    }

    bool badName(string badName) {
        string fs = badWords.text;
        string[] fLines = Regex.Split ( fs, "\n|\r|\r\n" );

        for ( int i=0; i < fLines.Length; i++ ) {
            Debug.Log("Word: " + fLines[i]);
            if (badName.Contains(fLines[i])) {
                return true;
            }
            
        }

        return false;
    }

}
