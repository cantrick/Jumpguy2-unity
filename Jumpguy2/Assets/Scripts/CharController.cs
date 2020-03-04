using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    public int jumpSpeed;

    private int jumps = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVars.isDead == false)
        {
            GetComponent<Rigidbody2D>().gravityScale = 5;
            //change jump height based on how many jumps you've used
            switch (jumps)
            {
                case 0:
                    jumpSpeed = 50;
                    break;
                case 1:
                    jumpSpeed = 40;
                    break;
                case 2:
                    jumpSpeed = 30;
                    break;
            }
            //zero out velocity so jumps aren't dampened by gravity
            //add force to jump!
            if (Input.GetMouseButtonDown(0) && jumps < 3)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpSpeed));
                jumps += 1;
            }
        } else
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Scenery" || collision.gameObject.tag == "Wall")
        {
            jumps = 0;
        }

        //add fail state if you crash into side of the wall
        if (collision.gameObject.tag == "Wall")
        {
            if((collision.gameObject.transform.position.y + (collision.gameObject.GetComponent<Collider2D>().bounds.size.y / 2)) > transform.position.y )
            {
                /*
                Debug.Log("ColliderPOS is: " + collision.gameObject.transform.position.y);
                Debug.Log("Collider TOP is: " + (collision.gameObject.transform.position.y + (collision.gameObject.GetComponent<Collider2D>().bounds.size.y/2)));
                Debug.Log("Collider is: " + collision.gameObject.GetComponent<Collider2D>().bounds.size.y);
                Debug.Log("Player is: " + transform.position.y);
                */
                GlobalVars.isDead = true;
            }

        }
    }
}
