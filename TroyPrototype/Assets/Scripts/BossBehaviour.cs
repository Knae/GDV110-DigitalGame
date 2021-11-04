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
    public Vector2 m_DirectionToPlayer = Vector2.zero;
 
    void FixedUpdate()
    {
        if(m_DirectionToPlayer.x>0)
        {
            BossAnimator.SetBool("FaceLeft",false);
        }
        else if(m_DirectionToPlayer.x<0)
        {
            BossAnimator.SetBool("FaceLeft", true);
        }

        if(!m_bIsMoving)
        {
            if(CheckIfTooFar())
            {
                m_bIsMoving = true;
                BossBody.MovePosition(BossBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
                BossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
            }
        }
        else
        {
             if(CheckIfCloseEnough())
            {
                m_bIsMoving = false;
                m_DirectionToPlayer = Vector2.zero;
                BossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
            }
            else
            {
                BossBody.MovePosition(BossBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
                BossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
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
