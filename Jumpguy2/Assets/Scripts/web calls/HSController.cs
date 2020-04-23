using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HSController : MonoBehaviour
{
    public void CallAddScore(string userId, string score)
    {
        StartCoroutine(AddScore(userId, score));
    }

    public void CallGetScore()
    {
        StartCoroutine(GetScore());
    }

    public void CallGetMyScore(string userId)
    {
        StartCoroutine(GetMyScore(userId));
    }


    IEnumerator AddScore(string userId, string score)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("score", score);
        UnityWebRequest www = UnityWebRequest.Post("https://salmonpants.xyz/files/addscore.php", form);

        yield return www.SendWebRequest();

        if (!www.downloadHandler.text.Contains("successfully"))
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Score added successfully");
        }

    }

    IEnumerator GetScore()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post("https://salmonpants.xyz/files/displayscore.php", form);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Contains("ERROR:"))
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("DEBUG: " + www.downloadHandler.text);
            GlobalVars.scoreResults = www.downloadHandler.text.Split('#');
            GlobalVars.gameState = 3;
        }

    }

    IEnumerator GetMyScore(string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        UnityWebRequest www = UnityWebRequest.Post("https://salmonpants.xyz/files/displaymyscore.php", form);

        yield return www.SendWebRequest();

        if (www.downloadHandler.text.Contains("ERROR: No scores"))
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log("Let's upload your score instead");
            CallAddScore(userId, GlobalVars.highScore.ToString());
        }
        else if (www.downloadHandler.text.Contains("ERROR:"))
        {
            Debug.Log(www.downloadHandler.text);
        }
        else if (www.downloadHandler.text != "")
        {
            Debug.Log("DEBUG: GETMYSCORE: " + www.downloadHandler.text);
            GlobalVars.highScore = int.Parse(www.downloadHandler.text);
            PlayerPrefs.SetInt("highscore", GlobalVars.highScore);
        } else
        {
            Debug.Log("It's Empty!!!!: " + www.downloadHandler.text);
            yield return new WaitForSeconds(0.5f);
            CallGetMyScore(userId);
        }

    }

}
