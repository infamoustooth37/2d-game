using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //So you can use SceneManager 
public class PlayerController : MonoBehaviour
{
    //Staart Variables
    private Animator anim;
    private Rigidbody2D rb; 
    private Collider2D coll;
   [SerializeField] private int cherries = 0;
   [SerializeField] private Text cherryText;
    //States
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;
    //Inspector Variables
    [SerializeField]private LayerMask ground;
    [SerializeField]private float speed = 5f;
    [SerializeField]private float JumpForce = 12f;
    [SerializeField]private float hurtForce = 10f;

 

    
    private void OnCollisionEnter2D(Collision2D other)//destroying enemy
    {
        if(other.gameObject.tag == "Enemy")
        {
            if(state == State.falling)
            {
                Destroy(other.gameObject);
                Jump();
            }
            else
            {
                state =State.hurt;
                print("This is happenning");
                if(other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy is on the right therfore damadge and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                    print("To my right");
                }
                else
                {
                    //enemny is on the left therefore damadge and move rigth
                     rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                     print("to my left");
                }
            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Collectable"){
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }    
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void Update() 
    {
        if(state != State.hurt)
        {
            InputManager();
        }
        InputManager();
        VelocityState();
        anim.SetInteger("state", (int)state);//game choosing state of player and sets animation based on velocity and direction
    }
    
    private void InputManager()
    {
        float hDirection = Input.GetAxis("Horizontal");
        
        if(hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y); //moving left
            transform.localScale = new Vector2 (-1, 1); //flip sprite
            
        }
        
        else if(hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);//moving right
            transform.localScale = new Vector2 (1, 1);//flip sprite
            
        }
    //jumping
        if(Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground)) //jump + saying which layer player can jump on
        {
            Jump();
        }  
    
        
       
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        state = State.jumping;
    }
    private void VelocityState()
    {
        if(state ==State.jumping)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if(state == State.falling)
        {
            if(coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state ==State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state=State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }
        else
        {
            state =State.idle;
        }

    }
     
 public void RestartGame() 
 {

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
 }

 

}



