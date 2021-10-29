using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [Header("Object Settings")]
    public bool m_bCanBeDestroyedByBullets = true;

    protected float m_fHP = 20;
    protected float m_fDamageToTake;

    protected virtual void ReceiveDamage(float _inputDamage)
    {
        m_fHP -= _inputDamage;
        if(m_fHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_bCanBeDestroyedByBullets)
        {
            if (collision.tag == "ProjectileEnemy" || collision.tag == "ProjectilePlayer")
            {
                m_fDamageToTake  = collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
                Destroy(collision.gameObject);
                ReceiveDamage(m_fDamageToTake);
                m_fDamageToTake = 0;
            }
        }
    }
};