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

     public float daño = 1f;                 
    public Transform attackPoint;           
    public float attackRange = 1f;          
    public LayerMask enemigoLayer;          
    public float attackCooldown = 0.4f;     
    private bool canAttack = true;


    public float dashingCooldown = 5f;
    public float shootCooldown = 2f;
    public bool canShoot = true;
    public float speed;
    public float jumpforce;
    public GameObject proyectilPrefab;

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

        Movement();
        checkjump();
        checkAnimation();


    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Q) && canShoot)
        {

            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(0.3f);
        Vector2 posProyectil;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            
            posProyectil = new Vector2(trf.position.x - 2f, trf.position.y - 1.2f);
            
        }
        else
        {
            posProyectil = new Vector2(trf.position.x + 2f, trf.position.y - 1.2f);
        }

        Instantiate(proyectilPrefab, posProyectil, Quaternion.identity);
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    void Movement()
    {



        if (isDashing)
        {
            return;
        }
        bool enSuelo = gameObject.GetComponentInChildren<CheckGround>().isGround;
        if (enSuelo)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {

                if (Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2((float)(speed * 1.3), rb.velocity.y);

                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2((float)(-speed * 1.3), rb.velocity.y);

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
        else
        {
            if (Input.GetKey(KeyCode.D))
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

                }
                else if (Input.GetKey(KeyCode.A))
                {
                    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);

                }
        }
        if (Input.GetKey(KeyCode.W) && canDash)
        {
            StartCoroutine(dash());
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
    // Dash tiene prioridad
    if (isDashing)
    {
        animator.SetBool("isDashing", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isJumping", false);
        return; // Salimos, nada más se procesa
    }
    else
    {
        animator.SetBool("isDashing", false);
    }

    // Salto tiene segunda prioridad
    if (!gameObject.GetComponentInChildren<CheckGround>().isGround)
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
    }
    else
    {
        animator.SetBool("isJumping", false);

        // Correr y caminar
        if (Math.Abs(rb.velocity.x) > speed)
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

    // Flip sprite
    if (rb.velocity.x < -0.1f)
        GetComponent<SpriteRenderer>().flipX = true;
    else if (rb.velocity.x > 0.1f)
        GetComponent<SpriteRenderer>().flipX = false;
}

    private IEnumerator dash()
    {
        canDash = false;
        isDashing = true;
        float originalgravity = rb.gravityScale;
        rb.gravityScale = 0;

        float direccion = GetComponent<SpriteRenderer>().flipX ? -1 : 1;

        rb.velocity = new Vector2(speed * dashForce * direccion, 0);
       
        yield return new WaitForSeconds(dashingTime);
        
        isDashing = false;
        rb.gravityScale = originalgravity;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }
    private IEnumerator Attack()
    {
        canAttack = false;
        animator.SetTrigger("attack"); 
        
        yield return new WaitForSeconds(0.15f);

        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemigoLayer);
        foreach (Collider2D col in hitEnemies)
        {
           
            EnemigoController enemigo = col.GetComponent<EnemigoController>();
            if (enemigo != null)
            {
                enemigo.RecibirDaño(daño);
            }
            else
            {
                
            }
        }
    private void RecibirDaño(float daño){
             
    }
}
