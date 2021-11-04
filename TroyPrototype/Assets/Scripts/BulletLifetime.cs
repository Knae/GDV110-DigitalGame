using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetime : MonoBehaviour
{
    public float m_fRange = 5.0f;
    public CircleCollider2D thisBulletCollider;

    [Header("BulletSettings")]
    private int m_iDamage = 1;
    private Vector3 m_PrevPos = Vector3.zero;
    private float m_fDistanceTraveled = 0f;

    private void Awake()
    {
        m_PrevPos = transform.position;
    }

    void FixedUpdate()
    {
        m_fDistanceTraveled = (transform.position - m_PrevPos).magnitude;
        m_fRange -= (transform.position - m_PrevPos).magnitude;
        m_PrevPos = transform.position;
        if (m_fRange<0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int check = LayerMask.GetMask("Projectiles");
        if (collision.tag == "Indestructible")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == LayerMask.GetMask("Projectiles"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    public void SetBulletRange (float _input)
    {
        m_fRange = _input;
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