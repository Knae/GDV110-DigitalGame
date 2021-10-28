using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public Rigidbody2D BossGunL;
    public Rigidbody2D BossGunR;
    public Rigidbody2D Player;

    [Header("Behaviour Constants")]
    public float HP = 15f;
    public float m_fMovementSpd = 1.0f;
    public float m_fMinimumDistanceFromPlayer = 0.8f;
    public float m_fMaximumDistanceFromPlayer = 1.1f;

    private bool m_bIsMoving = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!m_bIsMoving)
        {
            if(CheckIfTooFar())
            {
                m_bIsMoving = true;
                transform.position = Vector3.MoveTowards(transform.position, Player.position, m_fMovementSpd*Time.deltaTime);
            }
        }
        else
        {
             if(CheckIfCloseEnough())
            {
                m_bIsMoving = false;
            }
            else
            {
               transform.position = Vector3.MoveTowards(transform.position, Player.position, m_fMovementSpd * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            HP -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    bool CheckIfTooFar()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        return distance > m_fMaximumDistanceFromPlayer;
    }

    bool CheckIfCloseEnough()
    {
        float distance = Vector3.Distance(Player.position, transform.position);
        return distance <= m_fMinimumDistanceFromPlayer;
    }

}
