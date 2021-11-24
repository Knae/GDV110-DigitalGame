using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternStream : BaseBulletPattern
{
    [Header("Pattern Specific Settings -Stream")]
    public int m_iNumberOfPelletsToCreate = 5;
    public float m_fMaximumForce = 2.0f;
    public float m_fMinimumForce = 1.5f;
    public float m_fScatterRange = 0.25f;
    public float m_fPelletRangeMax = 2.4f;
    public float m_fPelletRangeMin = 2.0f;
    public float m_fBulletForce_Stream = 1.0f;
    public float m_fFiringTime = 1.0f;
    public float m_fFiringCooldown = 0.5f;
    public float m_fPelletDamage = 0.05f;
    public int m_iTotalAmmo_Stream = 200;

    private int m_iPelletsCreated = 0;
    private bool m_bIsOnCooldown = false;
    private bool m_bWasFiring = false;

    protected override void Awake()
    {
        ResetSettings();
    }

    protected override void FixedUpdate()
    {
        if(m_bIsOnCooldown)
        {
            if (m_fCounterTime <= m_fFiringCooldown)
            {
                m_fCounterTime += 1.0f * Time.deltaTime;
            }
            else
            {
                m_fCounterTime = 0;
                m_bIsOnCooldown = false;
            }
        }
        else
        {
            if (!m_bWasFiring && m_fCounterTime >= 0)
            {
                m_fCounterTime -= 1.0f * Time.deltaTime;
            }

            m_bWasFiring = false;
        }

    }

    public override bool fireProjectiles()
    {
        if (m_iCurrentAmmoCount > 0 || m_bUsedByAI)
        {
            if (!m_bIsOnCooldown)
            {
                if(m_fCounterTime <= m_fFiringTime)
                {
                    //If we've not reached the max firing time, then keep firing
                    //while adding to the timer.
                    while (m_iPelletsCreated < 1)//= m_iNumberOfPelletsToCreate)
                    {
                        GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
                        Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();                        
                        newBullet.GetComponent<BulletLifetime>().SetBulletRange(Random.Range(m_fPelletRangeMin, m_fPelletRangeMax) );
                        newBullet.GetComponent<BulletLifetime>().SetDamage(m_fBulletDamage);
                        Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
                        float forceRandomizer = Random.Range(m_fMinimumForce, m_fMaximumForce);
                        newBulletBody.AddForce((gunFirePoint.up + directionRandomizer) * forceRandomizer, ForceMode2D.Impulse);
                        m_iPelletsCreated++;
                    }
                    if (source != null && !source.isPlaying) 
                    {
                        source.PlayOneShot(clip);
                    }
                    //this boolean is so that we don't decrease the counter in the next update if we already fired
                    m_bWasFiring = true;
                    m_fCounterTime += 1.0f * Time.deltaTime;
                }
                else
                {
                    //If we've reached max firing time, then start cooldown mode
                    m_bIsOnCooldown = true;
                    m_fCounterTime = 0f;
                }
                m_iPelletsCreated = 0;
                m_iCurrentAmmoCount--;
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
        m_fBulletForce = m_fBulletForce_Stream;
        m_fTimeBetweenShots = 0f;
        m_fBulletDamage = m_fPelletDamage;
        m_iTotalAmmo = m_iTotalAmmo_Stream;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_fRange = m_fPelletRangeMax;
    }

    public override float GetBulletForce()
    {
        return base.GetBulletForce();
    }
}
