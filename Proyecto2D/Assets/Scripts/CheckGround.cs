using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isGround;

    private void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        isGround = true;
    }

    // Update is called once per frame
    public void OnTriggerExit2D(Collider2D other)
    {
        isGround = false;
    }
}
