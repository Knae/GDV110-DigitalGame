using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternRepeater : BaseBulletPattern
{
    [Header("Pattern Specific Settings - Repeater")]
    [SerializeField] public float m_fScatterRange = 0.15f;
    [SerializeField] public float m_fRepeaterRange = 1000.0f;
    [SerializeField] public float m_fBulletForce_Repeater = 5.0f;
    [SerializeField] public float m_fTimeBetweenShots_Repeater = 0.1f;
    [SerializeField] public float m_fWindupTime = 0f;
    [SerializeField] public float m_fFiringTime = 1.5f;
    [SerializeField] public float m_fFiringCooldown = 0.5f;
    [SerializeField] public float m_fBulletDamage_Repeater = 0.5f;
    [SerializeField] public int m_iTotalAmmo_Repeater = 400;
    [SerializeField] public bool m_bHasCooldown = false;
    [SerializeField] public bool m_bHasWindup = false;
    [Header("Debug")]
    //[SerializeField] private bool m_bIsOnDelay = false;
    //[SerializeField] private bool m_bIsOnCooldown = false;
    [SerializeField] private bool m_bWasFiring = false;
    [SerializeField] private float m_fTimeAfterLastShot = 0f;
    [SerializeField] private STATE m_eCurrentState = STATE.DELAY;

    enum STATE
    {
        NONE,
        DELAY,
        NORMAL,
        COOLDOWN
    }

    protected override void Awake()
    {
        ResetSettings();
    }

    protected override void FixedUpdate()
    {
        switch (m_eCurrentState)
        {
            case STATE.DELAY:
            {
                if(!m_bHasWindup)
                {
                    m_eCurrentState = STATE.NORMAL;
                }
                else if (m_bWasFiring)
                {
                    m_fCounterTime += 1.0f * Time.deltaTime;
                    if (m_fCounterTime >= m_fWindupTime)
                    {
                        m_fCounterTime = 0;
                        m_eCurrentState = STATE.NORMAL;
                    } 
                }
                else
                {
                    if (m_fCounterTime > 0)
                    {
                        m_fCounterTime -= 1.0f * Time.deltaTime;
                    }
                    else
                    {
                        m_fCounterTime = 0;
                    }
                }
                break;
            }
            case STATE.NORMAL:
            {
                if(m_bWasFiring)
                {
                    m_fTimeAfterLastShot += 1.0f * Time.deltaTime;

                    if (m_bHasCooldown)
                    {
                        m_fCounterTime += 1.0f * Time.deltaTime;
                        if (m_fCounterTime >= m_fFiringTime)
                        {
                            m_fCounterTime = 0f;
                            m_fTimeAfterLastShot = 0f;
                            m_eCurrentState = STATE.COOLDOWN;
                        } 
                    }
                    else if(m_bHasWindup)
                    {
                        if(m_fCounterTime < m_fWindupTime)
                        {
                            m_fCounterTime += 1.0f * Time.deltaTime;
                        }
                    }
                }
                else
                {
                    if (m_fTimeAfterLastShot < m_fTimeBetweenShots)
                    {
                        m_fTimeAfterLastShot += 1.0f * Time.deltaTime; 
                    }

                    m_fCounterTime -= 1.0f * Time.deltaTime;
                    if (m_fCounterTime < 0)
                    {
                        m_fCounterTime = 0f;
                        if (m_bHasWindup)
                        {
                            m_eCurrentState = STATE.DELAY; 
                        }
                    }
                }
                break;
            }
            case STATE.COOLDOWN:
            {
                m_fCounterTime += 1.0f * Time.deltaTime;
                if (m_fCounterTime >= m_fFiringCooldown)
                {
                    m_fCounterTime = 0f;
                    m_eCurrentState = STATE.DELAY;
                }
                break;
            }
            default:
            {
                break;
            }
        }
        m_bWasFiring = false;
    }

    public override bool fireProjectiles()
    {
        m_bWasFiring = true;

        if ((m_iCurrentAmmoCount > 0 || m_bUsedByAI) )
        {
            if (m_eCurrentState == STATE.NORMAL &&  (m_fTimeAfterLastShot >= m_fTimeBetweenShots) )
            {
                GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
                newBullet.GetComponent<BulletLifetime>().SetBulletRange(m_fRange);
                Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
                newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
                m_fTimeAfterLastShot = 0.0f;
                if(source != null)
                {
                    source.PlayOneShot(clip);
                }

                if(m_iCurrentAmmoCount > 0)
                {
                    m_iCurrentAmmoCount--;
                }

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

    public override void ResetAmmo()
    {
        base.ResetAmmo();
    }

    public override void ResetSettings()
    {
        m_fBulletForce = m_fBulletForce_Repeater;
        m_fTimeBetweenShots = m_fTimeBetweenShots_Repeater;
        m_fBulletDamage = m_fBulletDamage_Repeater;
        m_iTotalAmmo = m_iTotalAmmo_Repeater;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_fRange = m_fRepeaterRange;
        //m_bIsOnDelay = false;
    }
}
