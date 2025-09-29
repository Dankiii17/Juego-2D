using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
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
        checkAnimation();
        checkjump();

    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed * 2, rb.velocity.y);

            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed * 2, rb.velocity.y);

            }

        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);

            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);

            }

        }
        
    }

    void checkjump()
    {
        if (Input.GetKey(KeyCode.Space) && gameObject.GetComponentInChildren<CheckGround>().isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
        }
    }
    void checkAnimation()
    {
        if (rb.velocity.x < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
           
        }
        else if (rb.velocity.x > 0.1f )
        {
            GetComponent<SpriteRenderer>().flipX = false;
            
        }
        if (Math.Abs(rb.velocity.x) >= speed * 2)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
        else if (Math.Abs(rb.velocity.x) > 0.1f)
        {

            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
