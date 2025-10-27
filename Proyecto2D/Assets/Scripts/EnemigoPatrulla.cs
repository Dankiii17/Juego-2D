using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    public Transform pointA;          
    public Transform pointB;           
    public float speed = 2f;           
    public float detectionRange = 5f;  
    public Transform player;           
    public CheckGround checkGround;    
    public float vida = 1f; 
    public float dañoEnemigo=1f;

    private Vector2 targetPoint;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
      
        if (pointA == null) pointA = new GameObject("PointA").transform;
        if (pointB == null) pointB = new GameObject("PointB").transform;

        pointA.position = transform.position + Vector3.right * 4f;
        pointB.position = transform.position + Vector3.left * 4f;

        targetPoint = pointB.position;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            MoveTowards(player.position);
        }
        else
        {
            Patrullar();
        }
    }

    void Patrullar()
    {
        
        if (checkGround.isGround)
        {
            MoveTowards(targetPoint);

            if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
            {
                targetPoint = targetPoint == (Vector2)pointA.position ? pointB.position : pointA.position;
            }
        }
        else
        {
            
            targetPoint = targetPoint == (Vector2)pointA.position ? pointB.position : pointA.position;
        }
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

      
        if (rb.velocity.x < -0.1f)
            sr.flipX = true;
        else if (rb.velocity.x > 0.1f)
            sr.flipX = false;

      
        
    }

   
    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0f)
        {
            
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           
           PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.RecibirDaño(dañoEnemigo);
            }
        }
    }

   
    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
}
