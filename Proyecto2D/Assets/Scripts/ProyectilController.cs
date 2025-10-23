using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilController : MonoBehaviour
{
    private const float TIME2LIVE = 4f;

    [SerializeField] private float speed;
    private Rigidbody2D rb;

    private GameObject player;
    private float initTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player.GetComponent<SpriteRenderer>().flipX)
        {
            speed = -speed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, 0f);
        if (Time.time - initTime > TIME2LIVE)
        {
            Destroy(gameObject);
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            EnemigoController e =other.GetComponent<EnemigoController>();
            if(e!=null){
                e.RecibirDaño(100);
            }
        
            Destroy(gameObject);      
        }else if (other.CompareTag("Jefe"))
        {
            JefeController jefe = other.GetComponent<JefeController>();
            if(jefe != null){
                jefe.RecibirDaño(100);
            }
            
            Destroy(gameObject);
        }
    }
}
