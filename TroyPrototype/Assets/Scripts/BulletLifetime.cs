using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifetime : MonoBehaviour
{
    //public GameObject objThis;
    public float m_fLifetime = 5.0f;
    public CircleCollider2D thisBulletCollider;

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
    }
}