using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFiring : MonoBehaviour
{
    public Transform gunFirePoint;
    public GameObject projectilePrefab;
    public float m_fBulletForce = 2.0f;
    public float m_fFiringDelay = 0.1f;

    private float m_fCounterTime = 0.0f;

    // Update is called once per frame
    private void Update()
    {
        m_fCounterTime += 1.0f * Time.deltaTime;
    }
    void FixedUpdate()
    {
        if(Input.GetButton("Fire1"))
        {
            if(m_fCounterTime>=0.25f)
            {
                m_fCounterTime = 0;
                fireProjectiles();
            }
        }
    }

    void fireProjectiles()
    {
        GameObject newBullet =  Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint .rotation);
        Rigidbody2D newBulletBody = newBullet.GetComponent < Rigidbody2D>();
        newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
    }
}
