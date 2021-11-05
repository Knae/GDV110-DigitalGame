using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHP : MonoBehaviour
{
    public Slider Slider;
    public Color Low;
    public Color High;


    public void SetHealth(float health, float maxhealth)
    {
        Slider.value = health;
        Slider.maxValue = maxhealth;

    }
 

}
