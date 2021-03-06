using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetime : MonoBehaviour
{
    public CircleCollider2D thisBulletCollider;

    [Header("ProjectileSettings")]
    [SerializeField] private float m_fDamage = 1.0f;
    [SerializeField] private float m_fRange = 0.0f;
    [SerializeField] private float m_fDistanceTraveled = 0f;
    [SerializeField] private Vector3 m_PrevPos = Vector3.zero;
    //[SerializeField] private bool m_bPlayerShot;

    private void Awake()
    {
        m_PrevPos = transform.position;
    }

    void FixedUpdate()
    {
        m_fDistanceTraveled += (transform.position - m_PrevPos).magnitude;
        //m_fRange -= (transform.position - m_PrevPos).magnitude;
        m_PrevPos = transform.position;
        if (m_fDistanceTraveled >= m_fRange)
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
        //else if (   (collision.gameObject.tag == "ProjectilePlayer" && !m_bPlayerShot) ||
        //            (collision.gameObject.tag == "ProjectileEnemy" && m_bPlayerShot))
        //{
        //    Destroy(collision.gameObject);
        //    Destroy(gameObject);
        //}
    }

    public void SetBulletRange (float _input)
    {
        m_fRange = _input;
    }

    public void SetDamage(float _input)
    {
        m_fDamage = _input;
    }

    public float GetDamage()
    {
        return m_fDamage;
    }

    //private void OnDestroy()
    //{
    //    print("This " + name + " has been destroyed");
    //}
}