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
        STREAM
    }
    private WEAPONMODE m_iCurrentWeaponMode = WEAPONMODE.BASIC;
    private BaseBulletPattern m_refCurrentPattern;
    private BulletPatternBasic m_refBulletBasicPattern;
    private BulletPatternSpray m_refBulletSprayPattern;
    private BulletPatternStream m_refBulletStreamPattern;

    private void Start()
    {
        //Get references to bullet pattern scripts attached to player's
        //weapons
        m_refBulletSprayPattern = GetComponent<BulletPatternSpray>();
        m_refBulletBasicPattern = GetComponent<BulletPatternBasic>();
        m_refBulletStreamPattern = GetComponent<BulletPatternStream>();
        ChangeWeaponMode(m_refBulletBasicPattern);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            m_refCurrentPattern.fireProjectiles();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            switch (m_iCurrentWeaponMode)
            {
                case WEAPONMODE.BASIC:
                {
                    ChangeWeaponMode(m_refBulletSprayPattern);
                    m_iCurrentWeaponMode = WEAPONMODE.SPRAY;
                    break;
                }
                case WEAPONMODE.SPRAY:
                {
                    ChangeWeaponMode(m_refBulletStreamPattern);
                    m_iCurrentWeaponMode = WEAPONMODE.STREAM;
                    break;
                }
                case WEAPONMODE.STREAM:
                {
                    ChangeWeaponMode(m_refBulletBasicPattern);
                    m_iCurrentWeaponMode = WEAPONMODE.BASIC;
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
    }

    public void SetWeaponToSpray()
    {
        ChangeWeaponMode(m_refBulletSprayPattern);
        m_iCurrentWeaponMode = WEAPONMODE.SPRAY;
    }
    public void SetWeaponToStream()
    {
        ChangeWeaponMode(m_refBulletStreamPattern);
        m_iCurrentWeaponMode = WEAPONMODE.STREAM;
    }

}
