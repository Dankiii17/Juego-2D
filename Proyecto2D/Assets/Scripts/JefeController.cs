using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeController : MonoBehaviour
{

    public float vida = 40;
    public float tiempoEntreInvocaciones = 5f;
    public int maxEnemigosActivos = 3;
    public float distanciaMaximaInvocacion = 22f; 
    public GameObject prefabEnemigo; 
    public bool estaMuerto=false;
    public GameObject player;

   
    public Vector2 posicionEnemigo;

    private float tiempoUltimaInvocacion;
    private Transform trf;
    private Animator animator;

    void Start(){
        trf=GetComponent<Transform>();
        animator=GetComponent<Animator>();
        player=GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
       
        if (player != null)
        {
            float distancia = Vector2.Distance(trf.position, player.transform.position);

            if (distancia <= distanciaMaximaInvocacion && Time.time - tiempoUltimaInvocacion >= tiempoEntreInvocaciones)
            {
                InvocarEnemigos();
                tiempoUltimaInvocacion = Time.time;
            }
        }
    }

    void InvocarEnemigos()
    {
        
        int enemigosActuales = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (enemigosActuales >= maxEnemigosActivos) return;

        
       posicionEnemigo = new Vector2(trf.position.x - 2.3f, trf.position.y - 0.8f);

        
        Instantiate(prefabEnemigo, posicionEnemigo, Quaternion.identity);

       
    }

    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0)
        {
            estaMuerto = true;
            animator.SetTrigger("Muerte");
            StartCoroutine(Morir());
           
        }
    }
    public IEnumerator Morir(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }
}

