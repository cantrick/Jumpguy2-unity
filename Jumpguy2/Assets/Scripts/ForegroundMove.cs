using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundMove : MonoBehaviour
{

    public int speed = 3;
    public Vector3 horizontal;
    public GameObject fgPrefab;

    bool fgSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        horizontal = new Vector3(speed, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.isDead == false)
        {
            transform.position = transform.position - (horizontal * Time.deltaTime);

            //spawn background
            if ((transform.position.x < -2.3f) && fgSpawn == true)
            {
                Instantiate(fgPrefab, new Vector3(7.90f, -0.3f, 0), Quaternion.identity);
                fgSpawn = false;
            }
            else if (transform.position.x < -7.93f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
