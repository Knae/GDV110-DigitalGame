﻿using System.Collections;
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

    [Header("Debug Variables")]
    [SerializeField] protected float m_fRange = 3.0f;
    [SerializeField] protected bool m_bUsedByAI = false;
    [SerializeField] protected int m_iTotalAmmo = 20;
    [SerializeField] protected int m_iCurrentAmmoCount = 0;
    [SerializeField] protected float m_fCounterTime = 0.0f;
    [SerializeField] protected float m_fBulletForce;
    [SerializeField] protected float m_fFiringDelay;
    [SerializeField] protected float m_fBulletDamage;

    public AudioSource source;
    public AudioClip clip;

    protected virtual void Awake()
    {
        m_fBulletForce = m_fBulletForceBase;
        m_fFiringDelay = m_fFiringDelayBase;
        m_fBulletDamage = m_fDamageBase;
        m_iCurrentAmmoCount = m_iTotalAmmo;
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
            newBullet.GetComponent<BulletLifetime>().SetBulletRange(m_fRange);
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

    public virtual void SetAsUsedByAI()
    {
        m_bUsedByAI = true;
    }

    public virtual void SetAsUsedByPlayer()
    {
        m_bUsedByAI = false;
    }

    public virtual void Reset()
    {
        m_iCurrentAmmoCount = m_iTotalAmmo;
    }
}
