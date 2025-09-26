using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float jumpforce;


    private Transform transform;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        


    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {   
           
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed * 2, rb.velocity.y);
                GetComponent<SpriteRenderer>().flipX = false;
                animator.SetBool("isRunning", true);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed * 2, rb.velocity.y);
                GetComponent<SpriteRenderer>().flipX = true;
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                GetComponent<SpriteRenderer>().flipX = false;
                animator.SetBool("isWalking", true);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                GetComponent<SpriteRenderer>().flipX = true;
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }
}
