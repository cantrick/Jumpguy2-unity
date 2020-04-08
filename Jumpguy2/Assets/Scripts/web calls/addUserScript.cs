using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class addUserScript : MonoBehaviour
{
    public Text ErrorText;

    private void Start()
    {
        ErrorText.enabled = false;
    }

    public void CallAddUser(string deviceid, string name)
    {
        StartCoroutine(AddUser(deviceid, name));
    }

    public void CallGetUser(string deviceid)
    {
        StartCoroutine(GetUser(deviceid));
    }

    public void CallGetUserName(string deviceid)
    {
        StartCoroutine(GetUserName(deviceid));
    }

    IEnumerator AddUser(string deviceid, string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("deviceid", deviceid);
        form.AddField("name", name);
        UnityWebRequest www = UnityWebRequest.Post("https://salmonpants.xyz/files/adduser2.php", form);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text != "0")
        {
            Debug.Log(www.downloadHandler.text);
            ErrorText.enabled = true;
            ErrorText.text = www.downloadHandler.text;
            GlobalVars.dupeUser = true;
        }
        else
        {
            GlobalVars.dupeUser = false;
            Debug.Log("User Created successfully");
            ErrorText.enabled = false;
            CallGetUser(deviceid);
        }

    }

    IEnumerator GetUserName(string deviceid)
    {
        WWWForm form = new WWWForm();
        form.AddField("deviceid", deviceid);
        UnityWebRequest www = UnityWebRequest.Post("https://salmonpants.xyz/files/getusername.php", form);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Contains("!!ERROR"))
        {
            Debug.Log(www.downloadHandler.text);
            ErrorText.enabled = true;
            ErrorText.text = www.downloadHandler.text;
        }
        else
        {
            Debug.Log("Username retrieved successfully: " + www.downloadHandler.text);
            ErrorText.enabled = false;
            PlayerPrefs.SetString("userName",www.downloadHandler.text);
        }


    }

    IEnumerator GetUser(string deviceid)
    {
        WWWForm form = new WWWForm();
        form.AddField("deviceid", deviceid);
        UnityWebRequest www = UnityWebRequest.Post("https://salmonpants.xyz/files/getuserid.php", form);

        yield return www.SendWebRequest();

        while(GlobalVars.userID == 0)
        {
            yield return new WaitForSeconds(0.5f);
            checkUserID(www,deviceid);
        }
       

    }

    void checkUserID (UnityWebRequest www, string deviceid)
    {
        if (www.downloadHandler.text.Contains("!!ERROR 3"))
        {
            Debug.Log(www.downloadHandler.text);
            if (GlobalVars.keysDeleted == false)
            {
                PlayerPrefs.DeleteAll();
                GlobalVars.keysDeleted = true;
            }
            

        }
        else if (www.downloadHandler.text.Contains("!!ERROR"))
        {
            Debug.Log(www.downloadHandler.text);
            ErrorText.enabled = true;
            ErrorText.text = www.downloadHandler.text;
        }
        else
        {
            Debug.Log("User Retrieved successfully");
            GlobalVars.userID = int.Parse(www.downloadHandler.text);
            PlayerPrefs.SetInt("userId",GlobalVars.userID);
            Debug.Log("USER ID: " + GlobalVars.userID);
            ErrorText.enabled = false;
            //once we get userId, grab high score from DB
            CallGetUserName(deviceid);
            GetComponent<HSController>().CallGetMyScore(GlobalVars.userID.ToString());
        }
    }

}
