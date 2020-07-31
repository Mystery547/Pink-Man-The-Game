using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskDude : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator animator;

    public float speed;
    public bool playerDestroyed;

    public Transform rightCollider;
    public Transform leftCollider;
    public Transform headPoint;

    private bool colliding;

    public LayerMask layer;

    public BoxCollider2D box;
    public CircleCollider2D circle;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);

        colliding = Physics2D.Linecast(rightCollider.position, leftCollider.position, layer);

        if(colliding)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            speed = -speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")    
        {
            float height = collision.contacts[0].point.y - headPoint.position.y;
            Debug.Log(height);

            if(height > 0 && !playerDestroyed)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                speed = 0;

                box.enabled = false;
                circle.enabled = false;

                rig.bodyType = RigidbodyType2D.Kinematic;


                animator.SetTrigger("die");
                Destroy(gameObject, 0.3f);
            }
            else
            {
                playerDestroyed = true;
                GameController.instance.ShowGameOver();
                Destroy(collision.gameObject);
            }
        }
    }
}
