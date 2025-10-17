using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemigoController : MonoBehaviour
{
    public float distanciaMovimiento = 5f;
    public float velocidad = 2f;
    public float tiempoQuieto = 3f;
    public float knockbackForce = 10f;

    public float knockbackDuration = 0.5f;

    private Vector2 poiscionInicial;
    private bool moviendoIzq;
    private float tiempoEspera;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        poiscionInicial = transform.position;
        tiempoEspera = tiempoQuieto;
        moviendoIzq = true;    
    }

    // Update is called once per frame
    void Update()
    {
        gestionarGiro();
        if (tiempoEspera > 0)
        {
            tiempoEspera -= Time.deltaTime;
            return;
        }
        Vector2 destino = moviendoIzq ? poiscionInicial + Vector2.left * distanciaMovimiento :
                                        poiscionInicial + Vector2.right * distanciaMovimiento;

        transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

        if (Vector2.Distance(transform.position, destino) < 0.1f)
        {
            moviendoIzq = !moviendoIzq;
            tiempoEspera = tiempoQuieto;
        }

    }
    void gestionarGiro()    
    {
        if (tiempoEspera <= 0)
        {
            animator.SetBool("enMovimiento", true);
            if (!moviendoIzq)
                GetComponent<SpriteRenderer>().flipX = true;
            else GetComponent<SpriteRenderer>().flipX = false;
        }
        else animator.SetBool("enMovimiento", false);

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            Vector2 collisionNormal = other.contacts[0].normal;
            Vector2 direccionKnockback = -collisionNormal;
            playerRb.AddForce(direccionKnockback * knockbackForce, ForceMode2D.Impulse);

            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            
        }
    }
}
