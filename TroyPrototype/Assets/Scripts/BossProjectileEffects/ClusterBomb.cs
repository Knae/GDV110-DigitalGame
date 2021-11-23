using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBomb : BaseProjectileEffect
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
    public BulletPatternGrenade[] m_ExplosiveEjectors;

    [Header("ClusterBomb Settings")]
    [SerializeField] public int m_iNumberOfFirePoints = 4;
    [SerializeField] public float m_fActivationDelay = 0f;
    [SerializeField] public float m_fFuseTime_Cluster = 0.0f;
    [SerializeField] public float m_fRotateSpeed = 0.0f;
    [SerializeField] public float m_fFiringInterval = 0.0f;
    [SerializeField] public float m_fBombDamage = 0.0f;
    [SerializeField] public float m_fExplosiveForce = 5.0f;
    [SerializeField] public float m_fExplosiveRadius = 0.5f;
    [SerializeField] public float m_fProjectileForce = 1.0f;
    [SerializeField] public float m_fProjectileRange = 5.0f;
    [SerializeField] public float m_fScatterRange = 0.0f;
    [SerializeField] public float m_fTimeBetweenShots_Grenade = 0.0f;

    [Header("Debug Variables - Bullet Wheel")]
    [SerializeField] private float m_fCounter = 0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (m_iNumberOfFirePoints > 8)
        {
            m_iNumberOfFirePoints = 8;
        }

        foreach (var item in m_ExplosiveEjectors)
        {
            item.m_fBulletDamage_Grenade = m_fBombDamage;
            item.m_fGrenadeRange = m_fProjectileRange;
            item.m_fExplosiveForce = m_fExplosiveForce;
            item.m_fDmgRadius = m_fExplosiveRadius;
            item.m_fFuseTime = m_fFuseTime_Cluster;
            item.m_fTimeBetweenShots_Grenade = m_fFiringInterval;
            item.m_fBulletForce_Grenade = m_fProjectileForce;
            item.SetAsUsedByAI();

            item.ResetSettings();
        }
    }

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
            if (m_fCounter >= m_fActivationDelay)
            {
                m_bEnabled = true;
                m_thisRgdBdy2D.velocity = Vector2.zero;
            }
        }
    }

    private void FireAttachedGun()
    {
        for (int i = 0; i < m_iNumberOfFirePoints; i++)
        {
            m_ExplosiveEjectors[i].fireProjectiles();
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
