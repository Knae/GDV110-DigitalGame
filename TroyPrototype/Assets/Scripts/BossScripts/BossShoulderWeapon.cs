using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoulderWeapon : MonoBehaviour
{
    public Rigidbody2D m_EnemyBody;
    public GameObject bossBody;
    public GameObject gunBody;
    public Transform gunFirePoint;
    public GameObject projectilePrefab;
    [Header("Settings")]
    [SerializeField] private float m_fHP = 75;
    [SerializeField] private float m_fRotationOffset = 45f;
    [SerializeField] private SpriteRenderer m_sprtRenderer;
    [SerializeField] private Sprite[] m_sprtShoulderSprite;
    [Header("Weapon Stats")]
    [SerializeField] private float m_fBossFiringCooldown = 2.0f;
    [SerializeField] private float m_fWeaponRange = 1.0f;
    [Header("Repeater")]
    [SerializeField] private BulletPatternRepeater m_refRepeaterGun_Boss;
    [SerializeField] private float m_fFiringPeriod_Repeater = 4.0f;
    [SerializeField] private float m_fFiringCooldown_Repeater = 0.5f;
    [SerializeField] private float m_fFiringWindup_Repeater = 0.2f;
    [SerializeField] private float m_fDamage_Repeater = 0.5f;
    [SerializeField] private float m_fBulletForce_Repeater = 2.5f;
    [SerializeField] private float m_fFiringInterval_Repeater = 0.4f;
    //[SerializeField] private BulletPatternSpray m_refShotgun_Boss;
    //[SerializeField] private float m_fFiringDelay_Shotgun = 1.2f;
    //[SerializeField] private float m_fDamage_ShotgunPellet = 0.2f;
    [Header("Stream")]
    [SerializeField] private BulletPatternStream m_refFlameStreamGun_Boss;
    [SerializeField] private float m_fFiringTime_Stream = 1.5f;
    [SerializeField] private float m_fFiringCooldown_Stream = 0.5f;
    [SerializeField] private float m_fDamage_StreamPellet = 0.02f;

    //[Header("Grenade")]
    //[SerializeField] private BulletPatternGrenade m_refGrenadeGun_Boss;
    //[SerializeField] private float m_fGrenadeInterval = 1.5f;
    //[SerializeField] private float m_fGrenadeExplosiveForce = 2.0f;
    //[SerializeField] private float m_fGrenadeExplosiveRadius = 1.0f;
    //[SerializeField] private float m_fDamage_Grenade = 5.0f;

    [SerializeField] private BaseBulletPattern m_refCurrentPattern;

    [Header("Debug Variables")]
    [SerializeField] private bool m_bEnableFiringTime;
    [SerializeField] private bool m_bStopRotation;
    [SerializeField] private float m_fAngle = 0f;
    [SerializeField] private float m_fPredictedAngle = 0f;
    [SerializeField] private float m_fAngleToTarget = 0f;
    [SerializeField] private float m_fDistanceFromPlayer = 0f;
    [SerializeField] private bool m_bFacingEnemy = false;
    [SerializeField] private string m_strCollidedSightTag;
    [SerializeField] private STATES m_eCurrentState = STATES.NOTFIRING;
    [SerializeField] private Vector2 m_PlayerPrevLocation;
    [SerializeField] private Vector2 m_TargetPositionAdjustment;
    [SerializeField] private Vector2 m_TargetVelocity;
    [SerializeField] private Vector2 predictedPlayerLocation;
    [SerializeField] private Vector2 projectedDistanceTravelled_Dir;
    [SerializeField] private float projectedDistanceTravelled_Mag;
    [SerializeField] private float timeToProjHit;
    [SerializeField] private float m_fProjectMass = 0.5f;
    [SerializeField] private float m_fProjectForce = 5.0f;
    [SerializeField] private float m_fBossStatePredictModifier = 1.0f;

    private float m_fCurrentDelay = 0f;
    private float m_fFiringTime = 0.0f;
    private BossBehaviour m_ScriptParentBehaviour;

    public enum STATES
    {
        NONE,
        NOTFIRING,
        FIRING,
        COOLINGDOWN,
        DESTROYED
    }

    public enum WEAPONMODE
    {
        NONE,
        BASIC,
        SPRAY,
        STREAM,
        REPEATER
    }

    private void Start()
    {
        m_ScriptParentBehaviour = bossBody.GetComponent<BossBehaviour>();

        m_refRepeaterGun_Boss = GetComponent<BulletPatternRepeater>();
        m_refRepeaterGun_Boss.m_fFiringTime = m_fFiringPeriod_Repeater;
        m_refRepeaterGun_Boss.m_fTimeBetweenShots_Repeater = m_fFiringInterval_Repeater;
        m_refRepeaterGun_Boss.m_fFiringCooldown = m_fFiringCooldown_Repeater;
        m_refRepeaterGun_Boss.m_fWindupTime = m_fFiringWindup_Repeater;
        m_refRepeaterGun_Boss.m_fBulletDamage_Repeater = m_fDamage_Repeater;
        m_refRepeaterGun_Boss.m_fBulletForce_Repeater = m_fBulletForce_Repeater;
        m_refRepeaterGun_Boss.m_bHasWindup = true;
        m_refRepeaterGun_Boss.m_bHasCooldown = true;
        m_refRepeaterGun_Boss.ResetSettings();
        m_refRepeaterGun_Boss.SetAsUsedByAI();

        //m_refShotgun_Boss = GetComponent<BulletPatternSpray>();
        m_refFlameStreamGun_Boss = GetComponent<BulletPatternStream>();
        m_refFlameStreamGun_Boss.m_fFiringTime = m_fFiringTime_Stream;
        m_refFlameStreamGun_Boss.m_fFiringCooldown = m_fFiringCooldown_Stream;
        m_refFlameStreamGun_Boss.m_fPelletDamage = m_fDamage_StreamPellet;
        m_refFlameStreamGun_Boss.ResetSettings();
        m_refFlameStreamGun_Boss.SetAsUsedByAI();

        //m_refGrenadeGun_Boss = GetComponent<BulletPatternGrenade>();
        //m_refGrenadeGun_Boss.m_fTimeBetweenShots_Grenade = m_fGrenadeInterval;
        //m_refGrenadeGun_Boss.m_fDmgRadius = m_fGrenadeExplosiveRadius;
        //m_refGrenadeGun_Boss.m_fExplosiveForce = m_fGrenadeExplosiveForce;
        ////m_refGrenadeGun_Boss
        //m_refGrenadeGun_Boss.ResetSettings();
        //m_refGrenadeGun_Boss.SetAsUsedByAI();

        m_refCurrentPattern = m_refRepeaterGun_Boss;
        m_fWeaponRange = m_refCurrentPattern.GetRange();

        m_fBossStatePredictModifier = 0.75f;
        if (projectilePrefab.GetComponent<Rigidbody2D>() != null)
        {
            m_fProjectMass = projectilePrefab.GetComponent<Rigidbody2D>().mass;
        }
        else
        {
            m_fProjectMass = 0.5f;
        }
        m_fProjectForce = m_refCurrentPattern.GetBulletForce();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_fHP <= 0 && m_eCurrentState != STATES.DESTROYED)
        {
            m_eCurrentState = STATES.DESTROYED;
        }
        else
        {
            m_bFacingEnemy = FaceGunsToEnemy();

            m_fCurrentDelay += 1.0f * Time.deltaTime;
            if (m_bEnableFiringTime)
            {
                if (m_eCurrentState == STATES.FIRING)
                {
                    m_fFiringTime += 1 * Time.deltaTime;
                }
                else if (m_eCurrentState == STATES.NOTFIRING)
                {
                    if (m_fFiringTime > 0)
                    {
                        m_fFiringTime -= 1 * Time.deltaTime;
                        if (m_fFiringTime < 0)
                        {
                            m_fFiringTime = 0;
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        GunsAreThinking(CheckIfPlayerInRange());

        if (!m_ScriptParentBehaviour.GetIfPlayerInSight())
        {
            m_PlayerPrevLocation = Vector2.zero;
            m_TargetPositionAdjustment = Vector2.zero;
        }
        else
        {
            if (m_PlayerPrevLocation == Vector2.zero)
            {
                m_PlayerPrevLocation = m_EnemyBody.transform.position;
            }
            Vector2 playerCurrentPosition = m_EnemyBody.transform.position;
            m_TargetPositionAdjustment = playerCurrentPosition - m_PlayerPrevLocation;
            m_TargetVelocity = m_TargetPositionAdjustment / Time.deltaTime;
            m_PlayerPrevLocation = m_EnemyBody.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ProjectilePlayer")
        {
            TakeDamage(collision.gameObject.GetComponent<BulletLifetime>().GetDamage());
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float _input )
    {
        m_fHP -= _input;
    }

    private void fireWeapon()
    {
        m_refCurrentPattern.fireProjectiles();
    }

    bool CheckIfPlayerInRange()
    {
        m_fDistanceFromPlayer = Vector3.Distance(m_EnemyBody.position, gunFirePoint.transform.position);
        return m_fDistanceFromPlayer <= m_fWeaponRange;
    }
    void GunsAreThinking(bool _playerInRange)
    {
        switch (m_eCurrentState)
        {
            case STATES.NOTFIRING:
            {
                if (_playerInRange && m_bFacingEnemy)
                {
                    m_eCurrentState = STATES.FIRING;
                    m_fCurrentDelay = 0;
                }
                break;
            }
            case STATES.FIRING:
            {
                fireWeapon();

                if (!_playerInRange || !m_ScriptParentBehaviour.GetIfPlayerInSight())
                {
                    m_eCurrentState = STATES.NOTFIRING;
                }
               
                break;
            }
            case STATES.COOLINGDOWN:
            {
                if (m_fCurrentDelay >= m_fBossFiringCooldown)
                {
                    m_eCurrentState = STATES.NOTFIRING;
                }
                break;
            }
            case STATES.DESTROYED:
            {
                Destroy(gameObject);
                break;
            }
        }
    }

    bool FaceGunsToEnemy()
    {
        if (m_fHP > 0)
        {
            Vector2 playerCurrentPosition = m_EnemyBody.transform.position;
            FindAngleToPlayer(playerCurrentPosition);
            m_fPredictedAngle = FindAngleToPlayer_Prediction();
            if (!CheckIfLookingAtPlayer())
            {
                if (m_ScriptParentBehaviour.GetIfPlayerInSight() && !m_bStopRotation)
                {
                    //gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fAngle + m_fRotationOffset/* + m_fAimAssistAngle*/);
                    gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fPredictedAngle + m_fRotationOffset);
                    if (m_fAngle > 0f && m_sprtShoulderSprite.Length > 1)
                    {
                        m_sprtRenderer.sprite = m_sprtShoulderSprite[1];
                    }
                    else
                    {
                        m_sprtRenderer.sprite = m_sprtShoulderSprite[0];
                    }
                }
                else
                {
                    //Player not in sight or rotation locked
                }
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Possible common function
    /// </summary>
    /// <param name="_inPointA"></param>
    private void FindAngleToPlayer(Vector3 _inPointA)
    {
        m_fAngle = Mathf.Atan2(_inPointA.y - transform.position.y, _inPointA.x - transform.position.x) * Mathf.Rad2Deg;
    }

    private float FindAngleToPlayer_Prediction()
    {
        Vector2 currentPlayerPosition = m_EnemyBody.transform.position;
        timeToProjHit = m_fDistanceFromPlayer / (m_fProjectForce/m_fProjectMass);
        projectedDistanceTravelled_Dir = m_TargetVelocity.normalized;
        projectedDistanceTravelled_Mag = m_TargetVelocity.magnitude * timeToProjHit;
        Vector2 projectDistance = projectedDistanceTravelled_Dir * projectedDistanceTravelled_Mag * Random.Range(m_fBossStatePredictModifier-0.1f, m_fBossStatePredictModifier+0.1f);
        predictedPlayerLocation = currentPlayerPosition + projectDistance;
        float predictedAngle = Mathf.Atan2(predictedPlayerLocation.y - transform.position.y, predictedPlayerLocation.x - transform.position.x) * Mathf.Rad2Deg;
       return predictedAngle;
    }

    /// <summary>
    /// possible common function
    /// </summary>
    /// <returns></returns>
    private bool CheckIfLookingAtPlayer()
    {
        int maskIgnoreProjectileAndBoss = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 14);
        RaycastHit2D checkSight = Physics2D.Raycast(gunFirePoint.position, gunFirePoint.up, m_fWeaponRange+5, ~maskIgnoreProjectileAndBoss);
        //WHY WON"T YOU WORK
        //Debug.DrawRay(gunFirePoint.position, gunFirePoint.up * (m_fRange+10), Color.yellow);
        if (checkSight.collider != null && checkSight.collider.gameObject.tag == "Player")
        {
            m_strCollidedSightTag = checkSight.collider.gameObject.name;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeWeaponPattern(WEAPONMODE _inputMode)
    {
        switch (_inputMode)
        {
            case WEAPONMODE.STREAM:
            {
                m_refCurrentPattern = m_refFlameStreamGun_Boss;
                break;
            }
            case WEAPONMODE.REPEATER:
            {
                m_refCurrentPattern = m_refRepeaterGun_Boss;
                break;
            }
            default:
            case WEAPONMODE.NONE:
            case WEAPONMODE.BASIC:
            case WEAPONMODE.SPRAY:
                break;
        }
        m_fWeaponRange = m_refCurrentPattern.GetRange();
        m_fProjectForce = m_refCurrentPattern.GetBulletForce();
    }

    public float GetHP()
    {
        return m_fHP;
    }

    public void ChangeBossStateModifier(float _inModifier)
    {
        m_fBossStatePredictModifier = _inModifier;
    }
}
