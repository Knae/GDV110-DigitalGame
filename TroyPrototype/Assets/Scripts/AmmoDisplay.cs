using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private WeaponFiring m_AmmoCountSource;
    [SerializeField] private Text m_txtAmmoDisplay;
    [Header("Debug")]
    [SerializeField] private int m_iAmmoCount;
    //[SerializeField] private bool isFiring;

    

    void Start()
    {
        
    }

    void Update()
    {
        m_iAmmoCount = m_AmmoCountSource.GetAmmoLeft();
        m_txtAmmoDisplay.text = m_iAmmoCount.ToString();
    }
}
