using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternGrenade : BaseBulletPattern
{
    public GameObject enemyBody;
    public ExplosiveProjectile boomBall;
    public Camera worldScreen;
    [Header("Pattern Specific Settings - Grenade")]
    [SerializeField] public float m_fScatterRange = 0.25f;
    [SerializeField] public float m_fGrenadeRange = 4.0f;
    [SerializeField] public float m_fAIRangeVariation = 1.5f;
    [SerializeField] public float m_fBulletForce_Grenade = 1.0f;
    [SerializeField] public float m_fGrenadeDmgRadius = 3.0f;
    [SerializeField] public float m_fFuseTime = 1.2f;
    [SerializeField] public float m_fTimeBetweenShots_Grenade = 0.1f;
    [SerializeField] public float m_fWindupTime = 0f;
    [SerializeField] public float m_fFiringTime = 1.5f;
    [SerializeField] public float m_fFiringCooldown = 0.5f;
    [SerializeField] public float m_fBulletDamage_Grenade = 0.5f;
    [SerializeField] public int m_iTotalAmmo_Grenade = 10;
    [SerializeField] public bool m_bHasCooldown = false;
    [SerializeField] public bool m_bHasWindup = false;
    //[Header("Debug")]


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
        if ((m_iCurrentAmmoCount > 0 || m_bUsedByAI))
        {
            if( m_fCounterTime >= m_fTimeBetweenShots)
            {
                float targetRange = 0.0f;
                if (m_bUsedByAI)
                {
                    if (enemyBody == null)
                    {
                        //If it doesn't know where the enemy is
                        targetRange = Random.Range(m_fRange-m_fAIRangeVariation,m_fRange+m_fAIRangeVariation);
                    }
                    else
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, enemyBody.transform.position);
                        targetRange = Random.Range(distanceToTarget - m_fAIRangeVariation, distanceToTarget + m_fAIRangeVariation);

                    }
                }
                else
                {
                    if(worldScreen != null)
                    {
                        Vector3 targetPosition = worldScreen.ScreenToWorldPoint( Input.mousePosition);
                        targetRange = Vector2.Distance(targetPosition, gunFirePoint.position);
                        if(targetRange > (m_fRange))
                        {
                            targetRange = m_fRange;
                        }
                    }

                }

                ExplosiveProjectile newBomb = Instantiate(boomBall, gunFirePoint.position, gunFirePoint.rotation);
                newBomb.SetBulletRange(targetRange);
                newBomb.SetExploRadius(m_fGrenadeDmgRadius);
                newBomb.SetFuseTime(m_fFuseTime);
                Rigidbody2D newBulletBody = newBomb.GetComponent<Rigidbody2D>();
                Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
                newBulletBody.AddForce((gunFirePoint.up+directionRandomizer) * m_fBulletForce, ForceMode2D.Impulse);
                m_fCounterTime = 0.0f;
                if (source != null)
                {
                    source.PlayOneShot(clip);
                }

                if (m_iCurrentAmmoCount > 0)
                {
                    m_iCurrentAmmoCount--;
                }

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
        m_fBulletForce = m_fBulletForce_Grenade;
        m_fTimeBetweenShots = m_fTimeBetweenShots_Grenade;
        m_fBulletDamage = m_fBulletDamage_Grenade;
        m_iTotalAmmo = m_iTotalAmmo_Grenade;
        m_iCurrentAmmoCount = m_iTotalAmmo;
        m_fRange = m_fGrenadeRange;
        //m_bIsOnDelay = false;
    }

    public override float GetBulletForce()
    {
        return base.GetBulletForce();
    }
    
    private bool CheckIfPositionOutOfWindow(Vector2 _inPosition)
    {
        if (_inPosition.x > Screen.width || _inPosition.x < 0 || _inPosition.y > Screen.height || _inPosition.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
