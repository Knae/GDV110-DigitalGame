using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWheel : BaseProjectileEffect
{
    //Base Variables
    //======================================================
    //public BaseBulletPattern m_ShootingPattern;
    //[Header("Settings")]
    //[SerializeField] protected bool m_bEnabled = false;
    //[SerializeField] protected float m_fHP = 5.0f;
    //[SerializeField] protected float m_fLifeTime = 0.0f;
    //[Header("Base Debug Variables")]
    //[SerializeField] protected Rigidbody2D m_thisRgdBdy2D;
    //======================================================
    public BaseBulletPattern[] m_BulletPtnShootingPattern;

    [Header("BulletWheel Settings")]
    [SerializeField] public int   m_iNumberOfFirePoints = 1;
    [SerializeField] public float m_fActivationDelay = 0f;
    [SerializeField] public float m_fRotateSpeed = 0.0f;
    [SerializeField] public float m_fFiringInterval = 0.0f;
    [SerializeField] public float m_fProjectileDamage = 0.0f;
    [SerializeField] public float m_fProjectileForce = 1.5f;
    [SerializeField] public float m_fProjectileRange = 0.0f;

    [Header("Debug Variables - Bullet Wheel")]
    [SerializeField] private float m_fCounter = 0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if(m_iNumberOfFirePoints>4)
        {
            m_iNumberOfFirePoints = 4;
        }

        ResetBulletSettings();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        gameObject.transform.Rotate(0, 0, m_fRotateSpeed * Time.deltaTime);
        if (m_bEnabled)
        {
            FireAttachedGun();
            base.FixedUpdate();
        }
        else
        {
            m_fCounter += Time.deltaTime;
            if(m_fCounter>=m_fActivationDelay)
            {
                m_bEnabled = true;
                m_thisRgdBdy2D.velocity = Vector2.zero;
            }
        }
    }

    public void ResetBulletSettings()
    {
        foreach (var item in m_BulletPtnShootingPattern)
        {
            item.m_fFiringDelayBase = m_fFiringInterval;
            item.m_fDamageBase = m_fProjectileDamage;
            item.m_fRangeBase = m_fProjectileRange;
            item.m_fBulletForceBase = m_fProjectileForce;
            item.ResetBaseSettings();
        }
    }

    private void FireAttachedGun()
    {
        for(int i=0; i<m_iNumberOfFirePoints;i++)
        {
            m_BulletPtnShootingPattern[i].fireProjectiles();
        }
    }

    public override void TakeDamage(float _input)
    {
        base.TakeDamage(_input);
    }

    protected override void IsDestroyed()
    {
        base.IsDestroyed();
    }
}
