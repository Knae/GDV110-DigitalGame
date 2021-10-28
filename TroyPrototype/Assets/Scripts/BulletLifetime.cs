using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetime : MonoBehaviour
{
    //public GameObject objThis;
    public float m_fLifetime = 5.0f;
    public CircleCollider2D thisBulletCollider;

    private int m_iDamage = 1;

    void FixedUpdate()
    {
        m_fLifetime -= Time.deltaTime;
        if(m_fLifetime<0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Indestructible")
        {
            Destroy(gameObject);
        }
        else if(collision.tag == "ProjectileEnemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void SetBulletLifeTime (float _input)
    {
        m_fLifetime = _input;
    }

    public void SetDamage(int _input)
    {
        m_iDamage = _input;
    }

    public int GetDamage()
    {
        return m_iDamage;
    }
}