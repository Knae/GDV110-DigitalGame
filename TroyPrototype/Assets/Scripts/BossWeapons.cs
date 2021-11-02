using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapons : MonoBehaviour
{
    public Rigidbody2D enemyBody;
    public GameObject gunBody;
    public Transform gunFirePoint;
    public GameObject projectilePrefab;
    [Header("Weapon Stats")]
    public int HP = 50;
    public float m_fDestroyedAngle = 90.0f;
    public float m_fBulletForce = 1.5f;
    public float m_fFiringPeriod = 3.0f;
    public float m_fFiringCooldown = 2.0f;
    public float m_fFiringDelay = 0.2f;
    public float m_fRange = 1.0f;
    [Header("Debug Variables")]
    public float m_fAngle = 0f;
    public float m_fAngleToTarget = 0f;
    public float m_fDistanceFromPlayer = 0f;
    public bool m_bFacingEnemy = false;
    public STATES currentState = STATES.NOTFIRING;

    private float m_fCurrentDelay = 0f;
    private float m_fFiringTime = 0.0f;

    public enum STATES
    {
        NONE,
        NOTFIRING,
        FIRING,
        COOLINGDOWN,
        DESTROYED
    }
    // Update is called once per frame
    private void Update()
    {
        if (HP <= 0 && currentState != STATES.DESTROYED)
        {
            currentState = STATES.DESTROYED;
        }
        else
        {
            m_bFacingEnemy = FaceGunsToEnemy();
            m_fCurrentDelay += 1.0f * Time.deltaTime;
            if (currentState == STATES.FIRING)
            {
                m_fFiringTime += 1 * Time.deltaTime;
            }
            else if (currentState == STATES.NOTFIRING)
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

    //private void OnCollisionEnter2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            HP -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    void FixedUpdate()
    {
        GunsAreThinking(CheckIfPlayerInRange());
    }

    void fireWeapon()
    {
        if (m_fCurrentDelay >= m_fFiringDelay)
        {
            GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
            Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
            newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
        }
    }

    bool CheckIfPlayerInRange()
    {
        //float distance = Vector3.Distance(enemyBody.transform.position,transform.position);
        //return distance <= m_fRange;
        m_fDistanceFromPlayer = Vector3.Distance(enemyBody.position, transform.position);
        return m_fDistanceFromPlayer <= m_fRange;
    }
    void GunsAreThinking(bool _playerInRange)
    {
        switch (currentState)
        {
            case STATES.NOTFIRING:
                {
                    if (_playerInRange && m_bFacingEnemy)
                    {
                        currentState = STATES.FIRING;
                        m_fCurrentDelay = 0;
                    }
                    break;
                }
            case STATES.FIRING:
                {
                    if (m_fCurrentDelay >= m_fFiringDelay)
                    {
                        fireWeapon();
                        m_fCurrentDelay = 0;
                    }

                    if (!_playerInRange || !m_bFacingEnemy)
                    {
                        currentState = STATES.NOTFIRING;
                    }
                    else if (m_fFiringTime >= m_fFiringPeriod)
                    {
                        m_fFiringTime = 0;
                        m_fCurrentDelay = 0;
                        currentState = STATES.COOLINGDOWN;
                    }
                    break;
                }
            case STATES.COOLINGDOWN:
                {
                    if (m_fCurrentDelay >= m_fFiringCooldown)
                    {
                        currentState = STATES.NOTFIRING;
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
        if (HP > 0)
        {
            FindAngleBetweenPoints(enemyBody.transform.position);
            if (!CheckIfLookingAtPlayer())
            {
                gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fAngle);
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fDestroyedAngle);
            return false;
        }
    }

    private void FindAngleBetweenPoints(Vector3 _inPointA)
    {
        m_fAngle = Mathf.Atan2(_inPointA.y - transform.position.y, _inPointA.x - transform.position.x) * Mathf.Rad2Deg;
    }

    private bool CheckIfLookingAtPlayer()
    {
        RaycastHit2D checkSight = Physics2D.Raycast(gunFirePoint.position, gunFirePoint.up, m_fRange+10, ~LayerMask.GetMask("Projectiles"));
        Debug.DrawRay(gunFirePoint.position, gunFirePoint.up * (m_fRange+10), Color.yellow);
        if (checkSight.collider != null && checkSight.collider.gameObject.tag == "Player")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
