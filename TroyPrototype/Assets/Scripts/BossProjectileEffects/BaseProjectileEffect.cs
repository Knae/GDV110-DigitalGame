using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileEffect : MonoBehaviour
{
    public BaseBulletPattern m_ShootingPattern;
    [Header("Settings")]
    [SerializeField] protected bool m_bEnabled = false;
    [SerializeField] protected float m_fHP = 5.0f;
    [SerializeField] protected float m_fLifeTime = 0.0f;

    [Header("Base Debug Variables")]
    [SerializeField] protected Rigidbody2D m_thisRgdBdy2D;

    protected virtual void Start()
    {
        m_thisRgdBdy2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        m_fLifeTime -= Time.deltaTime;
        if (m_fLifeTime <= 0)
        {
            IsDestroyed();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ProjectilePlayer")
        {
            TakeDamage(collision.gameObject.GetComponent<BulletLifetime>().GetDamage());
            Destroy(collision.gameObject);
        }
    }

    public virtual void TakeDamage(float _input)
    {
        m_fHP -= _input;
        if (m_fHP <= 0)
        {
            IsDestroyed();
        }
    }

    protected virtual void IsDestroyed()
    {
        Destroy(gameObject);
    }
}
