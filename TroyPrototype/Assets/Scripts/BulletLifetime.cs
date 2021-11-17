using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetime : MonoBehaviour
{
    public CircleCollider2D thisBulletCollider;

    [Header("BulletSettings")]
    [SerializeField] private int m_iDamage = 1;
    [SerializeField] private float m_fRange = 0.0f;
    [SerializeField] private float m_fDistanceTraveled = 0f;
    [SerializeField] private Vector3 m_PrevPos = Vector3.zero;
    [SerializeField] private bool m_bPlayerShot;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Indestructible")
        {
            Destroy(gameObject);
        }
        else if (   (collision.gameObject.tag == "ProjectilePlayer" && !m_bPlayerShot) ||
                    (collision.gameObject.tag == "ProjectileEnemy" && m_bPlayerShot))
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

    //private void OnDestroy()
    //{
    //    print("This " + name + " has been destroyed");
    //}
}