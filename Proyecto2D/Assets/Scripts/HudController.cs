using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO playerStatsSO;

   
    [SerializeField] private Slider slider;

   
    [SerializeField] private List<Image> bulletsImages;

    void Start()
    {
        
    }

    void Update()
    {
       
        slider.value = playerStatsSO.live / playerStatsSO.maxLive;

        
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if(bulletsImages == null || bulletsImages.Count == 0) return;

        for (int i = 0; i < bulletsImages.Count; i++)
        {
            if (i < playerStatsSO.municion)
                bulletsImages[i].color = Color.white; 
            else
                bulletsImages[i].color = Color.black; 
        }
    }
}

