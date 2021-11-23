using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponFiring : MonoBehaviour
{
    public enum WEAPONMODE
    {
        NONE,
        BASIC,
        SPRAY,
        STREAM,
        REPEATER,
        GRENADE
    }

    [Header("Debug")]
    [SerializeField] private WEAPONMODE m_iCurrentWeaponMode = WEAPONMODE.BASIC;
    [SerializeField] private int m_iAmmoLeft = 0;
    //[SerializeField] private bool m_bGrenadeEnabled = false;
    //[SerializeField] private int m_iAmmoShotty = 100;
    //[SerializeField] private int m_iAmmoRepeater = 100;
    //[SerializeField] private int m_iAmmoFlamethrower = 1000;

    public SpriteRenderer m_currentGunSprite;
    public Sprite m_pistolSprite;
    public Sprite m_shottySprite;
    public Sprite m_rifleSprite;
    public Sprite m_flamethrowerSprite;

    [SerializeField] private Image PistolDisplay;
    [SerializeField] private Image ShottyDisplay;
    [SerializeField] private Image RepeaterDisplay;
    [SerializeField] private Image FlamerDisplay;


    public BaseBulletPattern m_refCurrentPattern;
    private BulletPatternBasic m_refBulletBasicPattern;
    private BulletPatternSpray m_refBulletSprayPattern;
    private BulletPatternStream m_refBulletStreamPattern;
    private BulletPatternRepeater m_refBulletRepeaterPattern;
    private BulletPatternGrenade m_refBulletGrenadePattern;

    private void Start()
    {
        //Get references to bullet pattern scripts attached to player's
        //weapons
        m_refBulletSprayPattern = GetComponent<BulletPatternSpray>();
        m_refBulletBasicPattern = GetComponent<BulletPatternBasic>();
        m_refBulletStreamPattern = GetComponent<BulletPatternStream>();
        m_refBulletRepeaterPattern = GetComponent<BulletPatternRepeater>();
        m_refBulletGrenadePattern = GetComponent<BulletPatternGrenade>();
        ChangeWeaponMode(m_refBulletBasicPattern);
        //SetWeaponToBasic();
        m_currentGunSprite.sprite = m_pistolSprite;

        //m_refBulletSprayPattern.m_iCurrentAmmoCount = 0;
        //m_refBulletStreamPattern.m_iCurrentAmmoCount = 0;
        //m_refBulletRepeaterPattern.m_iCurrentAmmoCount = 0;

        // UI
        PistolDisplay = GameObject.Find("PistolImage").GetComponent<Image>();
        ShottyDisplay = GameObject.Find("ShottyImage").GetComponent<Image>(); 
        RepeaterDisplay = GameObject.Find("RepeaterImage").GetComponent<Image>(); 
        FlamerDisplay = GameObject.Find("FlamerImage").GetComponent<Image>(); 

    }

    // Update is called once per frame
    void Update()
    {
        m_iAmmoLeft = GetAmmoLeft();

        if (Input.GetButton("Fire1"))
        {
            if (!m_refCurrentPattern.fireProjectiles())
            {
                SetWeaponToBasic(); ;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeaponToBasic();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeaponToSpray();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeaponToRepeater();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeaponToStream();
        }

        //else if (Input.GetKeyDown(KeyCode.X))
        //{
        //    switch (m_iCurrentWeaponMode)
        //    {
        //        case WEAPONMODE.BASIC:
        //            {
        //                SetWeaponToSpray();
        //                break;
        //            }
        //        case WEAPONMODE.SPRAY:
        //            {
        //                SetWeaponToStream();
        //                break;
        //            }
        //        case WEAPONMODE.STREAM:
        //            {
        //                SetWeaponToRepeater();
        //                break;
        //            }
        //        case WEAPONMODE.REPEATER:
        //            {
        //                if (m_bGrenadeEnabled)
        //                {
        //                    SetWeaponToGrenade();
        //                }
        //                else
        //                {
        //                    SetWeaponToBasic();
        //                }
        //                break;
        //            }
        //        case WEAPONMODE.GRENADE:
        //            {
        //                SetWeaponToBasic();
        //                break;
        //            }
        //        default:
        //            {
        //                break;
        //            }
        //    }
        //}
    }

    private void ChangeWeaponMode(BaseBulletPattern _inputPattern)
    {
        m_refCurrentPattern = _inputPattern;
        //m_refCurrentPattern.ResetAmmo();
    }

    public void SetWeaponToSpray() // shotty
    {
        ChangeWeaponMode(m_refBulletSprayPattern);
        m_iCurrentWeaponMode = WEAPONMODE.SPRAY;
        m_currentGunSprite.sprite = m_shottySprite;

        // enabling/disabling UI elements
        PistolDisplay.enabled = false;
        ShottyDisplay.enabled = true;
        RepeaterDisplay.enabled = false;
        FlamerDisplay.enabled = false;
    }
    public void SetWeaponToStream() // flamer
    {
        ChangeWeaponMode(m_refBulletStreamPattern);
        m_iCurrentWeaponMode = WEAPONMODE.STREAM;
        m_currentGunSprite.sprite = m_flamethrowerSprite;

        // enabling/disabling UI elements
        PistolDisplay.enabled = false;
        ShottyDisplay.enabled = false;
        RepeaterDisplay.enabled = false;
        FlamerDisplay.enabled = true;
    }

    public void SetWeaponToRepeater() // repeater/rifle
    {
        ChangeWeaponMode(m_refBulletRepeaterPattern);
        m_iCurrentWeaponMode = WEAPONMODE.REPEATER;
        m_currentGunSprite.sprite = m_rifleSprite;

        // enabling/disabling UI elements
        PistolDisplay.enabled = false;
        ShottyDisplay.enabled = false;
        RepeaterDisplay.enabled = true;
        FlamerDisplay.enabled = false;
    }

    public void SetWeaponToBasic()
    {
        ChangeWeaponMode(m_refBulletBasicPattern);
        m_iCurrentWeaponMode = WEAPONMODE.BASIC;
        m_currentGunSprite.sprite = m_pistolSprite;

        // enabling/disabling UI elements
        PistolDisplay.enabled = true;
        ShottyDisplay.enabled = false;
        RepeaterDisplay.enabled = false;
        FlamerDisplay.enabled = false;
    }

    public void SetWeaponToGrenade()
    {
        ChangeWeaponMode(m_refBulletGrenadePattern);
        m_iCurrentWeaponMode = WEAPONMODE.GRENADE;
    }

    public int GetAmmoLeft()
    {
        return m_refCurrentPattern.GetCurrentAmmoLeft();
    }
}
