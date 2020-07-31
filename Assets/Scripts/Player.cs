using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public bool isJumping;
    public bool doubleJumping;
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer sprite;
    private bool isFloating;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if(movement > 0)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        if(movement < 0)
        {
            anim.SetBool("walk", true);
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        if(movement == 0)
        {
            anim.SetBool("walk", false);
        }
        
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && !isFloating)
        {

            if(!isJumping)
            {
                rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                doubleJumping = true;
                anim.SetBool("jump", true);
            }
            else
            {
                if(doubleJumping)
                {
                    rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                    doubleJumping = false;
                }
            }
            
        }
        
    }

    void OnCollisionEnter2D(Collision2D collisor)
    {
        if(collisor.gameObject.layer == 8)
        {
            isJumping = false;
            anim.SetBool("jump", false);
        }
        
        if(collisor.gameObject.tag == "Spike")
        {
           GameController.instance.ShowGameOver();
           Destroy(gameObject);
        }

        if(collisor.gameObject.tag == "Saw")
        {
            GameController.instance.ShowGameOver();
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collisor)
    {
        isJumping = true;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 11)
        {
            isFloating = true;
            rig.gravityScale = 2f;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == 11)
        {
            isFloating = false;
            rig.gravityScale = 4f;
        }
    }
}
