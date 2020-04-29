using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreDisplay : MonoBehaviour
{
    GameObject score;
    public static bool getScore = false;

    [SerializeField]
    private Transform SpawnPoint = null;
    public RectTransform content;

    [SerializeField]
    private GameObject item = null;


    // Start is called before the first frame update
    void Start()
    {
       // GlobalVars.scoreResults.Length
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.gameState == 3 && getScore == false)
        {
            for (int i = 0;i< GlobalVars.scoreResults.Length; i++)
            {
                // 60 width of item
                float spawnY = i * 30;
                //newSpawn Position
                Vector3 pos = new Vector3(SpawnPoint.position.x+100, -spawnY, SpawnPoint.position.z);
                //instantiate item
                GameObject SpawnedItem = Instantiate(item, pos, SpawnPoint.rotation);
                //setParent
                SpawnedItem.transform.SetParent(SpawnPoint, false);
                //get ItemDetails Component
                ItemDetails itemDetails = SpawnedItem.GetComponent<ItemDetails>();

                //set name
                itemDetails.text.color = Color.white;
                itemDetails.text.text = GlobalVars.scoreResults[i];

                //score = GameObject.Find(("Text"+(i+1)));

                //score.GetComponent<Text>().text = GlobalVars.scoreResults[i];
            }
            content.sizeDelta = new Vector2(0, (GlobalVars.scoreResults.Length*30));
            getScore = true;
        }
    }
}
