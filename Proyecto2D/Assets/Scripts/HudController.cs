using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField]private PlayerStatsSO playerStatsSO;

    [SerializeField] private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = playerStatsSO.live / playerStatsSO.maxLive;
    }
}
