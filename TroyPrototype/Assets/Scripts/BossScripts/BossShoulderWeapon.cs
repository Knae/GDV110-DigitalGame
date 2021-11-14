using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShoulderWeapon : MonoBehaviour
{
    public Rigidbody2D enemyBody;
    public GameObject bossBody;
    public GameObject gunBody;
    public Transform gunFirePoint;
    public GameObject projectilePrefab;
    [Header("Settings")]
    [SerializeField] private float m_fRotationOffset = 45f;
    [SerializeField] private SpriteRenderer m_sprtRenderer;
    [SerializeField] private Sprite[] m_sprtShoulderSprite;
    [Header("Weapon Stats - old")]
    [SerializeField] private float m_fHP = 50;
    [SerializeField] private float m_fDestroyedAngle = 90.0f;
    [SerializeField] private float m_fBulletForce = 1.5f;
    [SerializeField] private float m_fFiringPeriod = 3.0f;
    [SerializeField] private float m_fFiringCooldown = 2.0f;
    [SerializeField] private float m_fFiringDelay = 0.2f;
    [SerializeField] private float m_fRange = 1.0f;
    [Header("Weapon Stats - new")]
    [SerializeField] private BulletPatternRepeater m_refRepeaterGun_Boss;
    [SerializeField] private float m_fFiringPeriod_Repeater = 4.0f;
    [SerializeField] private float m_fFiringCooldown_Repeater = 0.5f;
    [SerializeField] private float m_fFiringWindup_Repeater = 0.2f;
    [SerializeField] private float m_fDamage_Repeater = 0.5f;
    //[SerializeField] private BulletPatternSpray m_refShotgun_Boss;
    //[SerializeField] private float m_fFiringDelay_Shotgun = 1.2f;
    //[SerializeField] private float m_fDamage_ShotgunPellet = 0.2f;
    [SerializeField] private BulletPatternStream m_refFlameStreamGun_Boss;
    [SerializeField] private float m_fFiringTime_Stream = 1.5f;
    [SerializeField] private float m_fFiringCooldown_Stream = 0.5f;
    [SerializeField] private float m_fDamage_StreamPellet = 0.02f;
    [SerializeField] private BaseBulletPattern m_refCurrentPattern;

    [Header("Debug Variables")]
    [SerializeField] private bool m_bEnableFiringTime;
    [SerializeField] private bool m_bStopRotation;
    [SerializeField] private float m_fAngle = 0f;
    [SerializeField] private float m_fAngleToTarget = 0f;
    [SerializeField] private float m_fDistanceFromPlayer = 0f;
    [SerializeField] private bool m_bFacingEnemy = false;
    [SerializeField] private string m_strCollidedSightTag;
    [SerializeField] private STATES m_eCurrentState = STATES.NOTFIRING;

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
        m_refRepeaterGun_Boss.m_fFiringTime =  m_fFiringPeriod_Repeater;
        m_refRepeaterGun_Boss.m_fFiringCooldown = m_fFiringCooldown_Repeater;
        m_refRepeaterGun_Boss.m_fWindupTime = m_fFiringWindup_Repeater;
        m_refRepeaterGun_Boss.m_fBulletDamage_Repeater = m_fDamage_Repeater;
        m_refRepeaterGun_Boss.m_bHasWindup = true;
        m_refRepeaterGun_Boss.m_bHasCooldown = true;
        m_refRepeaterGun_Boss.SetAsUsedByAI();

        //m_refShotgun_Boss = GetComponent<BulletPatternSpray>();
        m_refFlameStreamGun_Boss = GetComponent<BulletPatternStream>();
        m_refFlameStreamGun_Boss.m_fFiringTime = m_fFiringTime_Stream;
        m_refFlameStreamGun_Boss.m_fFiringCooldown = m_fFiringCooldown_Stream;
        m_refFlameStreamGun_Boss.m_fPelletDamage = m_fDamage_StreamPellet;
        m_refFlameStreamGun_Boss.SetAsUsedByAI();

        m_refCurrentPattern = m_refRepeaterGun_Boss;
        m_fRange = m_refCurrentPattern.GetRange();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ProjectilePlayer")
        {
            m_fHP -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "ProjectilePlayer")
    //    {
    //        m_fHP -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
    //        Destroy(collision.gameObject);
    //    }
    //}

    void FixedUpdate()
    {
        //if (m_eCurrentState != STATES.DESTROYED && CheckIfPlayerInRange())
        //{
        //    m_refCurrentPattern.fireProjectiles();
        //}
        GunsAreThinking(CheckIfPlayerInRange());
    }

    void fireWeapon()
    {
        m_refCurrentPattern.fireProjectiles();

        //if (m_fCurrentDelay >= m_fFiringDelay)
        //{
        //    GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
        //    Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
        //    newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
        //}
    }

    bool CheckIfPlayerInRange()
    {
        m_fDistanceFromPlayer = Vector3.Distance(enemyBody.position, gunFirePoint.transform.position);
        return m_fDistanceFromPlayer <= m_fRange;
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
                //if (m_fCurrentDelay >= m_fFiringDelay)
                //{
                //    fireWeapon();
                //    m_fCurrentDelay = 0;
                //}

                fireWeapon();

                if (!_playerInRange || !m_bFacingEnemy)
                {
                    m_eCurrentState = STATES.NOTFIRING;
                }
                //else if (m_fFiringTime >= m_fFiringPeriod)
                //{
                //    m_fFiringTime = 0;
                //    m_fCurrentDelay = 0;
                //    m_eCurrentState = STATES.COOLINGDOWN;
                //}
                break;
            }
            case STATES.COOLINGDOWN:
            {
                if (m_fCurrentDelay >= m_fFiringCooldown)
                {
                    m_eCurrentState = STATES.NOTFIRING;
                }
                break;
            }
            case STATES.DESTROYED:
            {
                break;
            }
        }
    }

    bool FaceGunsToEnemy()
    {
        if (m_fHP > 0)
        {
            FindAngleBetweenPoints(enemyBody.transform.position);
            if (!CheckIfLookingAtPlayer())
            {
                if (m_ScriptParentBehaviour.GetIfPlayerInSight() && !m_bStopRotation)
                {
                    gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fAngle + m_fRotationOffset);

                    if (m_fAngle > 0f && m_sprtShoulderSprite.Length > 1)
                    {
                        m_sprtRenderer.sprite = m_sprtShoulderSprite[1];
                    }
                    else
                    {
                        m_sprtRenderer.sprite = m_sprtShoulderSprite[0];
                    }
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
            //gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fDestroyedAngle);
            return false;
        }
    }

    /// <summary>
    /// Possible common function
    /// </summary>
    /// <param name="_inPointA"></param>
    private void FindAngleBetweenPoints(Vector3 _inPointA)
    {
        m_fAngle = Mathf.Atan2(_inPointA.y - transform.position.y, _inPointA.x - transform.position.x) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// possible common function
    /// </summary>
    /// <returns></returns>
    private bool CheckIfLookingAtPlayer()
    {
        int maskIgnoreProjectileAndBoss = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 14);
        RaycastHit2D checkSight = Physics2D.Raycast(gunFirePoint.position, gunFirePoint.up, m_fRange+5, ~maskIgnoreProjectileAndBoss);
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
        m_fRange = m_refCurrentPattern.GetRange();
    }

    public float GetHP()
    {
        return m_fHP;
    }
}
