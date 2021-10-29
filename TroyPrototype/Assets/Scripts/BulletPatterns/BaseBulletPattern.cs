using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletPattern : MonoBehaviour
{
    [Header("RequiredObjects")]
    public Transform gunFirePoint;
    public GameObject projectilePrefab;

    [Header("Player Weapon Settings")]
    public float m_fBulletForceBase = 2.0f;
    public float m_fFiringDelayBase = 0.5f;
    public float m_fDamageBase = 1f;
    public bool m_bUsedByAI = false;


    protected float m_fCounterTime = 0.0f;
    protected float m_fBulletForce;
    protected float m_fFiringDelay;
    protected float m_fBulletDamage;

    protected virtual void Awake()
    {
        m_fBulletForce = m_fBulletForceBase;
        m_fFiringDelay = m_fFiringDelayBase;
        m_fBulletDamage = m_fDamageBase;
    }

    protected virtual void FixedUpdate()
    {
        if (m_fCounterTime <= m_fFiringDelay)
        {
            m_fCounterTime += 1.0f * Time.deltaTime;
        }
    }

    public virtual bool fireProjectiles()
    {
        if (m_fCounterTime >= m_fFiringDelay)
        {
            GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
            Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
            newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
            m_fCounterTime = 0.0f;
        }
        return true;
    }

    public virtual float GetBulletDamage()
    {
        return m_fBulletDamage;
    }

    public virtual void SetAsUsedByAI()
    {
        m_bUsedByAI = true;
    }

    public virtual void SetAsUsedByPlayer()
    {
        m_bUsedByAI = false;
    }
}
