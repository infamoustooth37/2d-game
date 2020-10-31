using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;

    [SerializeField] private LayerMask Ground;

    private Collider2D coll;
    private Rigidbody2D rb;
   


    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    
    }


    private bool facingLeft = true;

    private void Update()
    {
     // Jump -> fall

        if(anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < .1)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
               
        }


        // fall->idle

        if(coll.IsTouchingLayers(Ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            
        }

    }

    private void Move()
    {
        if (facingLeft)
        {
            //Test to see if we are beyond left cap
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                //test to see if from is on the ground if so then jump
                if (coll.IsTouchingLayers(Ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight); //jump\
                    anim.SetBool("Jumping", true);

                }
            }
            else
            {
                facingLeft = false;
            }
            //if not we are facing right
        }
        else
        {
            //Test to see if we are beyond left cap
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //test to see if from is on the ground if so then jump
                if (coll.IsTouchingLayers(Ground))
                {
                    //jump
                    rb.velocity = new Vector2(jumpLength, jumpHeight); //jump
                    anim.SetBool("Jumping", true);

                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

   

   
}
