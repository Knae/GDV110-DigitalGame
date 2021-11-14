using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public int ammo;
    public bool isFiring;

    public Text ammoDisplay;

    void Start()
    {
        
    }

    void Update()
    {

        ammoDisplay.text = ammo.ToString();
        if (Input.GetMouseButtonDown(0) && !isFiring && ammo > 0)
        {
            isFiring = true;
            ammo--;
            isFiring = false;
        }
    }
}
