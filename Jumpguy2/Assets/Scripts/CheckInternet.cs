using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CheckInternet : MonoBehaviour
{
    private UnityWebRequest adobeRequest;
    public float pingInterval = 5f; //seconds
    public static bool isOnline = true;

    void Start()
    {
        StartCoroutine(Ping());
    }


    IEnumerator Ping()
    {
        while (true)
        {
            adobeRequest = new UnityWebRequest("http://www.google.com");
            yield return adobeRequest;
            isOnline = !adobeRequest.isNetworkError;
            yield return new WaitForSeconds(pingInterval);
        }
    }
}
