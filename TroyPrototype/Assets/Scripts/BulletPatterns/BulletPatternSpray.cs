using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternSpray : BaseBulletPattern
{
    [Header("Pattern Specific Settings -Spray")]
    public int m_iNumberOfPelletsToCreate = 10;
    public float m_fMaximumForce = 3.0f;
    public float m_fMinimumForce = 2.5f;
    public float m_fScatterRange = 0.35f;
    public float m_fPelletRangeMax = 2.0f;
    public float m_fPelletRangeMin = 1.8f;
    public float m_fFiringDelay_Spray = 1.5f;
    public float m_fPelletDamage = 0.2f;
    public int m_fMaxAmmo_Spray = 20;

    private int m_iPelletsCreated = 0;

    protected override void Awake()
    {
        ResetSettings();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool fireProjectiles()
    {
        if(m_iCurrentAmmoCount > 0 || m_bUsedByAI)
        {
            if (m_fCounterTime >= m_fTimeBetweenShots)
            {
                while (m_iPelletsCreated <= m_iNumberOfPelletsToCreate)
                {
                    GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
                    Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
                    newBullet.GetComponent<BulletLifetime>().SetDamage(m_fBulletDamage);
                    newBullet.GetComponent<BulletLifetime>().SetBulletRange(Random.Range(m_fPelletRangeMin, m_fPelletRangeMax));
                    Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
                    float forceRandomizer = Random.Range(m_fMinimumForce, m_fMaximumForce);
                    newBulletBody.AddForce((gunFirePoint.up + directionRandomizer) * forceRandomizer, ForceMode2D.Impulse);
                    m_iPelletsCreated++;
                }
                if (source != null)
                {
                    source.PlayOneShot(clip);
                }
                m_iPelletsCreated = 0;
                m_iCurrentAmmoCount--;
                m_fCounterTime = 0f;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ResetAmmo()
    {
        base.ResetAmmo();
    }

    public override void ResetSettings()
    {
        m_fTimeBetweenShots = m_fFiringDelay_Spray;
        m_fBulletDamage = m_fPelletDamage;
        m_iTotalAmmo = m_fMaxAmmo_Spray;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_fRange = m_fPelletRangeMax;
    }

    public override float GetBulletForce()
    {
        return base.GetBulletDamage();
    }
}
