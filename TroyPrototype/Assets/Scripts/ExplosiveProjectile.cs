using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    public CircleCollider2D thisBulletCollider;

    [Header("ProjectileSettings")]
    [SerializeField] private int m_iDamage = 1;
    [SerializeField] private float m_fRange = 0.0f;
    [SerializeField] private float m_fExploRadius = 1.0f;
    [SerializeField] private float m_fFuseTime = 1.3f;
    [SerializeField] private float m_fDistanceTraveled = 0f;
    [SerializeField] private Vector3 m_PrevPos = Vector3.zero;
    [Header("Debug Variables")]
    [SerializeField] const float m_iSpriteRadius = 32.0f;
    [SerializeField] bool m_bPrimed = false;

    private void Awake()
    {
        m_PrevPos = transform.position;
    }

    void FixedUpdate()
    {
        m_fDistanceTraveled += (transform.position - m_PrevPos).magnitude;
        m_PrevPos = transform.position;
        if (m_fDistanceTraveled >= m_fRange && !m_bPrimed)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            m_bPrimed = true;
        }
    }

    private void Explode()
    {

    }

    public void SetFuseTime(float _input)
    {
        m_fFuseTime = _input;
    }

    public void SetExploRadius(float _input)
    {
        m_fExploRadius = _input;
    }

    public void SetBulletRange(float _input)
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
