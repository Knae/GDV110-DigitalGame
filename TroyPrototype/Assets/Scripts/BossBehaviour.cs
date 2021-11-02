using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public Rigidbody2D BossBody;
    public Rigidbody2D Player;
    public Animator BossAnimator;

    [Header("Behaviour Constants")]
    public float HP = 15f;
    public float m_fMovementSpd = 1.0f;
    public float m_fMinimumDistanceFromPlayer = 0.8f;
    public float m_fMaximumDistanceFromPlayer = 1.1f;

    [Header("Debug")]
    public bool m_bIsMoving = false;
    public float distance = 0;
    //private Vector2 m_PositionToPlayer= Vector2.zero;
    public Vector2 m_DirectionToPlayer = Vector2.zero;
 
    void FixedUpdate()
    {
        BossAnimator.SetFloat("Horizontal",2*m_DirectionToPlayer.x);
        if(!m_bIsMoving)
        {
            if(CheckIfTooFar())
            {
                m_bIsMoving = true;
                //m_PositionToPlayer = Vector2.MoveTowards(transform.position, Player.position, m_fMovementSpd*Time.deltaTime);
                //BossBody.MovePosition(m_PositionToPlayer);
                //BossBody.transform.Translate(m_DirectionToPlayer*m_fMovementSpd*Time.deltaTime);\
                BossBody.MovePosition(BossBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
            }
        }
        else
        {
             if(CheckIfCloseEnough())
            {
                m_bIsMoving = false;
                m_DirectionToPlayer = Vector2.zero;
            }
            else
            {
                //m_PositionToPlayer = Vector2.MoveTowards(transform.position, Player.position, m_fMovementSpd * Time.deltaTime);
                //BossBody.MovePosition(m_PositionToPlayer);
                //BossBody.transform.Translate(m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime);
                BossBody.MovePosition(BossBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
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
        distance = CalculateDistance();
        return distance > m_fMaximumDistanceFromPlayer;
    }

    bool CheckIfCloseEnough()
    {
       distance = CalculateDistance();
        return distance <= m_fMinimumDistanceFromPlayer;
    }

    float CalculateDistance()
    {
       float temp = Vector2.Distance(Player.position, BossBody.position);
        Vector2 vectorDiff = BossBody.position - Player.position;

        if(vectorDiff.x ==0)
        {
            m_DirectionToPlayer.x = 0;
        }
        else
        {
            m_DirectionToPlayer.x = -(vectorDiff.x / (Mathf.Abs(vectorDiff.x)));
        }

        if (vectorDiff.y == 0)
        {
            m_DirectionToPlayer.y = 0;
        }
        else
        {
            m_DirectionToPlayer.y = -(vectorDiff.y / (Mathf.Abs(vectorDiff.y)));
        }

        return temp;
    }
}
