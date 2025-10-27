using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacioController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            collision.transform.position = new Vector2(player.posicionInicialx, player.posicionInicialy+1);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
    }
}
