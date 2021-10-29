using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapons : MonoBehaviour
{
    public GameObject enemyBody;
    public GameObject gunBody;
    public Transform gunFirePoint;
    public GameObject projectilePrefab;
    [Header("Weapon Stats")]
    public int HP = 10;
    public float m_fDestroyedAngle = 90.0f;
    public float m_fBulletForce = 1.5f;
    public float m_fFiringPeriod = 3.0f;
    public float m_fFiringCooldown = 2.0f;
    public float m_fFiringDelay = 0.2f;
    public float m_fRange = 1.0f;

    private float m_fCurrentDelay = 0f;
    private float m_fFiringTime = 0.0f;
    private STATES currentState = STATES.NOTFIRING;
    private float m_fAngle = 0f;

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
        FaceGunsToEnemy();
        if (HP<=0)
        {
            currentState = STATES.DESTROYED;
        }
        else
        {
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ProjectilePlayer")
        {
            HP -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    void FixedUpdate()
    {
        GunsAreThinking(CheckIfPlayerInRange());        
    }

    void fireProjectiles()
    {
        if(m_fCurrentDelay>=m_fFiringDelay)
        {
            GameObject newBullet = Instantiate(projectilePrefab, gunFirePoint.position, gunFirePoint.rotation);
            Rigidbody2D newBulletBody = newBullet.GetComponent<Rigidbody2D>();
            newBulletBody.AddForce(gunFirePoint.up * m_fBulletForce, ForceMode2D.Impulse);
        }
    }

    bool CheckIfPlayerInRange()
    {
        float distance = Vector3.Distance(enemyBody.transform.position,transform.position);
        return distance <= m_fRange;
    }
    void GunsAreThinking(bool _playerInRange)
    {
        switch (currentState)
        {
            case STATES.NOTFIRING:
            {
                if(_playerInRange)
                {
                    currentState = STATES.FIRING;
                    m_fCurrentDelay = 0;
                    }
                break;
            }
            case STATES.FIRING:
            {
                if(m_fCurrentDelay>=m_fFiringDelay)
                {
                    fireProjectiles();
                    m_fCurrentDelay = 0;
                }

                if(!_playerInRange)
                {
                   currentState = STATES.NOTFIRING;
                }
                else if(m_fFiringTime >= m_fFiringPeriod)
                {
                    m_fFiringTime = 0;
                    m_fCurrentDelay = 0;
                    currentState = STATES.COOLINGDOWN;
                }
                break;
            }
            case STATES.COOLINGDOWN:
            {
                if(m_fCurrentDelay >= m_fFiringCooldown)
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

    void FaceGunsToEnemy()
    {
        if(HP>0)
        {
            m_fAngle = FindAngleBetweenPoints(enemyBody.transform.position, transform.position);
            gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fAngle);
        }
        else
        {   
            gunBody.transform.rotation = Quaternion.Euler(0f, 0f, m_fDestroyedAngle);
        }
    }

    private float FindAngleBetweenPoints(Vector3 _inPointA, Vector3 _inPointB)
    {
        float angle = Mathf.Atan2(_inPointA.y - _inPointB.y, _inPointA.x - _inPointB.x) * Mathf.Rad2Deg;
        return angle;
    }
}
