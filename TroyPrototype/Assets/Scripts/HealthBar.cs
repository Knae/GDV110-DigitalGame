using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Color Low;
    public Color High;


    public void SetMaxHealth(float health)
    {
        slider.value = health;
        slider.maxValue = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
