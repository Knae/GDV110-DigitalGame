using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternSpray : BaseBulletPattern
{
    [Header("Pattern Specific Settings -Spray")]
    public int m_iNumberOfPelletsToCreate = 10;
    public float m_fMaximumForce = 3.0f;
    public float m_fMinimumForce = 2.0f;
    public float m_fScatterRange = 0.75f;
    public float m_fPelletLifeTime = 2.0f;
    [Header("Player Weapon Settings")]
    public float m_fBulletForce_Spray = 1.0f;
    public float m_fFiringDelay_Spray = 2.0f;
    public float m_fPelletDamage = 0.2f;
    public int m_iTotalAmmo = 20;

    private int m_iPelletsCreated = 0;

    protected override void Awake()
    {
        m_fBulletForce = m_fBulletForce_Spray;
        m_fFiringDelay = m_fFiringDelay_Spray;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool fireProjectiles()
    {
        if(m_iTotalAmmo>0)
        {
            if (m_fCounterTime >= m_fFiringDelay)
            {
                while (m_iPelletsCreated <= m_iNumberOfPelletsToCreate)
                {
                    GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
                    Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
                    newBullet.GetComponent<BulletLifetime>().SetBulletLifeTime(m_fPelletLifeTime);
                    Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
                    float forceRandomizer = Random.Range(m_fMinimumForce, m_fMaximumForce);
                    newBulletBody.AddForce((gunFirePoint.up + directionRandomizer) * forceRandomizer, ForceMode2D.Impulse);
                    m_iPelletsCreated++;
                }
                m_iPelletsCreated = 0;
                m_iTotalAmmo--;
                m_fCounterTime = 0f;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
