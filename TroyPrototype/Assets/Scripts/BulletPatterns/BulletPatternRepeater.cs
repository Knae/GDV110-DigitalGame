using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternRepeater : BaseBulletPattern
{
    [Header("Pattern Specific Settings - Repeater")]
    public float m_fScatterRange = 0.15f;
    public float m_fRepeaterRange = 8.0f;
    public float m_fBulletForce_Repeater = 5.0f;
    public float m_fFiringDelay_Repeater = 0.1f;
    public float m_fFiringTime = 1.5f;
    public float m_fFiringCooldown = 0.5f;
    public float m_fBulletDamage_Repeater = 0.5f;
    public int m_iTotalAmmo_Repeater = 400;

    private bool m_bIsOnDelay = false;
    private bool m_bIsOnCooldown = false;
    private bool m_bWasFiring = false;

    protected override void Awake()
    {
        m_fBulletForce = m_fBulletForce_Repeater;
        m_fFiringDelay = m_fFiringDelay_Repeater;
        m_fBulletDamage = m_fBulletDamage_Repeater;
        m_iTotalAmmo = m_iTotalAmmo_Repeater;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_bIsOnDelay = false;
    }

    protected override void FixedUpdate()
    {
        //if (m_bIsOnCooldown)
        //{
        //    if (m_fCounterTime <= m_fFiringCooldown)
        //    {
        //        m_fCounterTime += 1.0f * Time.deltaTime;
        //    }
        //    else
        //    {
        //        m_fCounterTime = 0;
        //        m_bIsOnCooldown = false;
        //    }
        //}
        //else
        //{
        //    if (!m_bWasFiring && m_fCounterTime >= 0)
        //    {
        //        m_fCounterTime -= 1.0f * Time.deltaTime;
        //    }

        //    m_bWasFiring = false;
        //}
        base.FixedUpdate();
    }

    public override bool fireProjectiles()
    {
        if (m_iCurrentAmmoCount > 0 || m_bUsedByAI)
        {
            if (m_fCounterTime >= m_fFiringDelay)
            {
                GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
                newBullet.GetComponent<BulletLifetime>().SetBulletRange(m_fRange);
                Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
                newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
                m_fCounterTime = 0.0f;
                if(source!=null)
                {
                    source.PlayOneShot(clip);
                }

                m_iCurrentAmmoCount--;
            }
            //if (!m_bIsOnCooldown)
            //{
            //    if (m_fCounterTime <= m_fFiringTime)
            //    {
            //        m_bIsOnDelay = false;
            //        //If we've not reached the max firing time, then keep firing
            //        //while adding to the timer.
            //        GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
            //        Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
            //        newBullet.GetComponent<BulletLifetime>().SetBulletRange(m_fRange);
            //        Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
            //        //float forceRandomizer = Random.Range(m_fMinimumForce, m_fMaximumForce);
            //        newBulletBody.AddForce((gunFirePoint.up + directionRandomizer) * m_fBulletForce, ForceMode2D.Impulse);

            //        //this boolean is so that we don't decrease the counter in the next update if we already fired
            //        m_bWasFiring = true;
            //        m_iCurrentAmmoCount--;
            //        m_fCounterTime += 1.0f * Time.deltaTime;
            //    }
            //    else
            //    {
            //        //If we've reached max firing time, then start cooldown mode
            //        m_bIsOnCooldown = true;
            //        m_bIsOnDelay = true;
            //        m_fCounterTime = 0f;
            //    }
            //}
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Reset()
    {
        base.Reset();
    }
}
