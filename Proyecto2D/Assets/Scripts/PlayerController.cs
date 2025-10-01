using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.U2D.Aseprite;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isDashing;
    public bool canDash = true;
    public float dashForce;
    public float dashingTime = 0.2f;

    public float dashingCooldown = 5f;
    public float speed;
    public float jumpforce;


    private Transform trf;
    private Rigidbody2D rb;
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trf = GetComponent<Transform>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if (!isDashing)
        // {
            Movement();
            checkjump();
        // }
       checkAnimation();


    }

    void Movement()
    {
        //  if (Input.GetKeyDown(KeyCode.Q))
        // {
            // dashear();
        // }
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

    // void dashear()
    // {
    //     rb.velocity = new Vector2(speed * 3, rb.velocity.y);
    //     isDashing = true;

        
    // }
    void checkjump()
    {
        if (Input.GetKey(KeyCode.Space) && gameObject.GetComponentInChildren<CheckGround>().isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);


        }
        
        
    }
    void checkAnimation()
    {
        if (Math.Abs(rb.velocity.y) > 0.1f)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else if(gameObject.GetComponentInChildren<CheckGround>().isGround)
        {
            animator.SetBool("isJumping", false);
        }
        if (rb.velocity.x < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;

        }
        else if (rb.velocity.x > 0.1f)
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
