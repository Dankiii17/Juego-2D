using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemigoController : MonoBehaviour
{
    public float velocidad = 3f;       
    public float distanciaAtaque = 0.5f; 
    public float vida = 1f;            
    public float dañoEnemigo=1f;
    public float daño;
    public bool estaMuerto = false;
    private Transform jugador;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
   

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }


    void Update()
    {
        if (jugador == null) return;

        
        Vector2 direccion = (jugador.position - transform.position);
        float distancia = Vector2.Distance(transform.position, jugador.position);

        
        if (distancia > distanciaAtaque)
        {
            transform.position = Vector2.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
            if (animator != null) animator.SetBool("enMovimiento", true);
        }
        else
        {
            if (animator != null) animator.SetBool("enMovimiento", false);
        }

        
        if (spriteRenderer != null)
            spriteRenderer.flipX = direccion.x > 0;
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

    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0)
        {
            estaMuerto = true;
            animator.SetTrigger("Muerte");
            StartCoroutine(Morir());
            Debug.Log("FUNCIONA");
        }
    }
    public IEnumerator Morir(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }
}
