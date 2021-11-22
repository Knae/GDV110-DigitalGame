using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletPattern : MonoBehaviour
{
    [Header("RequiredObjects")]
    public Transform gunFirePoint;
    public GameObject projectilePrefab;

    [Header("Base Weapon Values")]
    public float m_fBulletForceBase = 2.0f;
    public float m_fFiringDelayBase = 0.5f;
    public float m_fDamageBase = 1f;
    public float m_fRangeBase = 5.0f;
    public float m_fForceBase = 1.0f;
    public int m_iTotalAmmo = 99;

    [Header("Debug Variables")]
    [SerializeField] protected float m_fRange;
    [SerializeField] protected bool m_bUsedByAI = false;
    [SerializeField] public int m_iCurrentAmmoCount;
    [SerializeField] protected float m_fCounterTime;
    [SerializeField] protected float m_fBulletForce;
    [SerializeField] protected float m_fTimeBetweenShots;
    [SerializeField] protected float m_fBulletDamage;

    public AudioSource source;
    public AudioClip clip;

    protected virtual void Awake()
    {
        ResetSettings();
    }

    protected virtual void FixedUpdate()
    {
        if (m_fCounterTime <= m_fTimeBetweenShots)
        {
            m_fCounterTime += 1.0f * Time.deltaTime;
        }
    }

    public virtual bool fireProjectiles()
    {
        if (m_fCounterTime >= m_fTimeBetweenShots)
        {
            GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
            newBullet.GetComponent<BulletLifetime>().SetBulletRange(m_fRange);
            newBullet.GetComponent<BulletLifetime>().SetDamage(m_fBulletDamage);
            Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
            newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
            m_fCounterTime = 0.0f;
            if (source != null)
            {
                source.PlayOneShot(clip);
            }
        }
        return true;
    }

    public virtual float GetBulletDamage()
    {
        return m_fBulletDamage;
    }

    public virtual float GetRange()
    {
        return m_fRange;
    }

    public virtual void SetAsUsedByAI()
    {
        m_bUsedByAI = true;
    }

    public virtual void SetAsUsedByPlayer()
    {
        m_bUsedByAI = false;
    }

    public virtual void ResetAmmo()
    {
        m_iCurrentAmmoCount = m_iTotalAmmo;
    }

    public virtual int GetCurrentAmmoLeft()
    {
        return m_iCurrentAmmoCount;
    }

    public virtual float GetBulletForce()
    {
        return m_fBulletForce;
    }

    public virtual void ResetSettings()
    {
        m_fBulletForce = m_fBulletForceBase;
        m_fTimeBetweenShots = m_fFiringDelayBase;
        m_fBulletDamage = m_fDamageBase;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_fRange = m_fRangeBase;
        m_fBulletForce = m_fBulletForceBase;
        m_fCounterTime = 0f;
    }

    public virtual void ResetBaseSettings()
    {
        m_fBulletForce = m_fBulletForceBase;
        m_fTimeBetweenShots = m_fFiringDelayBase;
        m_fBulletDamage = m_fDamageBase;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_fRange = m_fRangeBase;
        m_fBulletForce = m_fBulletForceBase;
        m_fCounterTime = 0f;
    }
}
