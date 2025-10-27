using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDashing;
    public bool canDash = true;
    public bool muerto= false;
    public float dashForce;
    public float dashingTime = 0.2f;
    public float posicionInicialx;
    public float posicionInicialy;
    public bool isOnIce = false;

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
    public float vida = 5f;

    public AudioSource disparoAudio;
    public PlayerStatsSO playerStatsSO;
    public GameObject proyectilPrefab;
    private Transform trf;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        posicionInicialx = trf.position.x;
        posicionInicialy = trf.position.y;
        playerStatsSO.live = playerStatsSO.maxLive;
        playerStatsSO.municion=playerStatsSO.municionMax;
    }

    void FixedUpdate()
    {
        if(muerto){
            return;
        }
        Movement();
        checkjump();
        checkAnimation();
    }

    private void Update()
    {
        if(muerto) return;
        if (Input.GetKeyDown(KeyCode.Q) && canShoot && playerStatsSO.municion > 0)
        {
            StartCoroutine(Shoot());
        }
        if (Input.GetKeyDown(KeyCode.R) && canAttack)
        {
            StartCoroutine(Attack());
        }

        if (attackPoint != null)
        {
            float offsetX = Mathf.Abs(attackPoint.localPosition.x);
            if (GetComponent<SpriteRenderer>().flipX)
                attackPoint.localPosition = new Vector3(-offsetX, attackPoint.localPosition.y, attackPoint.localPosition.z);
            else
                attackPoint.localPosition = new Vector3(offsetX, attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        disparoAudio.time = 1f;
        disparoAudio.Play();
        
        animator.SetTrigger("shoot");
        StartCoroutine(DetenerSonido(1f));
        yield return new WaitForSeconds(0.8f);

        Vector2 posProyectil;
        if (GetComponent<SpriteRenderer>().flipX)
            posProyectil = new Vector2(trf.position.x - 2.3f, trf.position.y - 0.8f);
        else
            posProyectil = new Vector2(trf.position.x + 2.3f, trf.position.y - 0.8f);

        Instantiate(proyectilPrefab, posProyectil, Quaternion.identity);

        
        playerStatsSO.municion = Mathf.Max(playerStatsSO.municion - 1, 0);

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
    IEnumerator DetenerSonido(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        disparoAudio.Stop();
    }

    void Movement()
    {
        if (isDashing) return;

        bool enSuelo = gameObject.GetComponentInChildren<CheckGround>().isGround;
        if (enSuelo)
        {
            float moveSpeed = Input.GetKey(KeyCode.LeftShift) ? speed * 1.3f : speed;

            if (Input.GetKey(KeyCode.D))
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            else if (Input.GetKey(KeyCode.A))
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else
        {
            float horizontalInput = 0f;
            if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
            else if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;

            float currentSpeed = Mathf.Abs(rb.velocity.x);
            float targetSpeed = currentSpeed > speed ? currentSpeed : speed;

            rb.velocity = new Vector2(horizontalInput * targetSpeed, rb.velocity.y);
        }

        if (Input.GetKey(KeyCode.W) && canDash)
            StartCoroutine(dash());
    }

    void checkjump()
    {
        if (Input.GetKey(KeyCode.Space) && gameObject.GetComponentInChildren<CheckGround>().isGround)
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }

    void checkAnimation()
    {
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
        else if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;

        if (isDashing)
        {
            animator.SetBool("isDashing", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            return;
        }
        else animator.SetBool("isDashing", false);

        if (!gameObject.GetComponentInChildren<CheckGround>().isGround)
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isJumping", false);
            if (Math.Abs(horizontalInput) > 0.1f)
            {
                if (Math.Abs(rb.velocity.x) > speed)
                {
                    animator.SetBool("isRunning", true);
                    animator.SetBool("isWalking", false);
                }
                else
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isRunning", false);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }

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

    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
    foreach (Collider2D col in hitEnemies)
    {
        if (col.CompareTag("Enemigo"))
        {
            EnemigoController e = col.GetComponent<EnemigoController>();
            if (e != null && !e.estaMuerto) e.RecibirDaño(daño);
        }
        else if (col.CompareTag("Jefe"))
        {
            JefeController jefe = col.GetComponent<JefeController>();
            if (jefe != null) jefe.RecibirDaño(daño);
        }
        else if (col.CompareTag("Patrulla"))
        {
            EnemigoPatrulla patrulla = col.GetComponent<EnemigoPatrulla>();
            if (patrulla != null) patrulla.RecibirDaño(daño);
        }
    }

    canAttack = true;
}


    public void RecibirDaño(float dañoEnemigo)
    {
        animator.SetTrigger("Hit");
        playerStatsSO.live -= dañoEnemigo;
        if (playerStatsSO.live <= 0)
        {
            animator.SetTrigger("Muerte");
            StartCoroutine(Morir());
        }
    }

    public IEnumerator Morir()
    {   muerto=true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void SpikesDamage()
    {
        animator.SetTrigger("Hit");
        transform.position = new Vector2(posicionInicialx, posicionInicialy + 1);
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hielo"))
            isOnIce = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hielo"))
            isOnIce = false;
    }
}
