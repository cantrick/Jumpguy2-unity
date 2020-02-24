using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject jumpGuy;
    public GameObject wallPrefab;
    public Text Sky;


    float spawnChance = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(jumpGuy,new Vector3(-1.5f,-1.5f,-1), Quaternion.identity);
        Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f,-1.1f), 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.isDead == false)
        {
            spawnChance = Random.Range(0, 700);
            //spawn wall
            if (spawnChance > 0 && spawnChance < 8)
            {
                Instantiate(wallPrefab, new Vector3(3.2f, Random.Range(-2.3f, -1.1f), 0), Quaternion.identity);
            }
        }

            Sky.text = "Score: " + GlobalVars.localScore;
    }
}
