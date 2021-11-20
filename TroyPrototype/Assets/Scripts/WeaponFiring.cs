using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private bool m_bGrenadeEnabled = false;

    public SpriteRenderer m_currentGunSprite;
    public Sprite m_shottySprite;
    public Sprite m_rifleSprite;
    public Sprite m_flamethrowerSprite;

    private BaseBulletPattern m_refCurrentPattern;
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
    }

    // Update is called once per frame
    void Update()
    {
        m_iAmmoLeft = GetAmmoLeft();

        if (Input.GetButton("Fire1"))
        {
            if(!m_refCurrentPattern.fireProjectiles())
            {
                SetWeaponToBasic(); ;
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            switch (m_iCurrentWeaponMode)
            {
                case WEAPONMODE.BASIC:
                {
                    SetWeaponToSpray();
                    break;
                }
                case WEAPONMODE.SPRAY:
                {
                    SetWeaponToStream();
                    break;
                }
                case WEAPONMODE.STREAM:
                {
                    SetWeaponToRepeater();
                    break;
                }
                case WEAPONMODE.REPEATER:
                {
                    if (m_bGrenadeEnabled)
                    {
                        SetWeaponToGrenade();
                    }
                    else
                    {
                        SetWeaponToBasic();
                    }
                    break;
                }
                case WEAPONMODE.GRENADE:
                {
                    SetWeaponToBasic();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }

    private void ChangeWeaponMode(BaseBulletPattern _inputPattern)
    {
        m_refCurrentPattern = _inputPattern;
        m_refCurrentPattern.ResetAmmo();
    }

    public void SetWeaponToSpray()
    {
        ChangeWeaponMode(m_refBulletSprayPattern);
        m_iCurrentWeaponMode = WEAPONMODE.SPRAY;
        m_currentGunSprite.sprite = m_shottySprite;
    }
    public void SetWeaponToStream()
    {
        ChangeWeaponMode(m_refBulletStreamPattern);
        m_iCurrentWeaponMode = WEAPONMODE.STREAM;
        m_currentGunSprite.sprite = m_flamethrowerSprite;
    }

    public void SetWeaponToRepeater()
    {
        ChangeWeaponMode(m_refBulletRepeaterPattern);
        m_iCurrentWeaponMode = WEAPONMODE.REPEATER;
        m_currentGunSprite.sprite = m_rifleSprite;
    }

    public void SetWeaponToBasic()
    {
        ChangeWeaponMode(m_refBulletBasicPattern);
        m_iCurrentWeaponMode = WEAPONMODE.BASIC;
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
