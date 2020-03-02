using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMove : MonoBehaviour
{
    public int speed = 3;
    public Vector3 horizontal;
    public GameObject wallPrefab;

    private bool hasScored = false;

    // Start is called before the first frame update
    void Start()
    {
        horizontal = new Vector3(speed, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.isDead == false && GlobalVars.camState == 1) {

            transform.position = transform.position - (horizontal * Time.deltaTime);

            if (transform.position.x < -3.2f)
            {
                Destroy(this.gameObject);
            }

            if (!hasScored && transform.position.x < -1.5f)
            {
                GlobalVars.localScore++;
                hasScored = true;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("WALL COLLIDING");
            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
