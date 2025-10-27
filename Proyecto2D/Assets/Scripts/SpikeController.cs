using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.CompareTag("Player"))
        {
            
            collision.transform.GetComponent<PlayerController>().SpikesDamage();
        }

        if (collision.transform.CompareTag("Enemigo"))
        {
            collision.transform.GetComponent<EnemigoController>().RecibirDaño(1000);
        }
        
   }
}
