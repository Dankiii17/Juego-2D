using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField]private PlayerStatsSO playerStatsSO;
    [SerializeField]private TextMeshProUGUI vidas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vidas.text = "Vidas:"+playerStatsSO.live.ToString();
    }
}
