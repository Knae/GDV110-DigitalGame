using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;

public class BossBehaviour : MonoBehaviour
{
    public Rigidbody2D m_rgdbdyBossBody;
    public Rigidbody2D m_rgdbdyPlayer;
    public Animator m_animrBossAnimator;
    public SpriteRenderer m_sprtrRenderer;

    [Header("Behaviour Constants")]
    [SerializeField] private float HP = 50f;
    [SerializeField] private float m_fMovementSpd = 1.0f;
    [SerializeField] private float m_fMinimumDistanceFromPlayer = 0.8f;
    [SerializeField] private float m_fMaximumDistanceFromPlayer = 1.1f;
    [SerializeField] private float m_fDelayAfterLosingSight = 2.0f;
    [SerializeField] private bool m_bUseNavMesh;

    [Header("WanderSpots")]
    [SerializeField] private Transform[] m_WanderSpot;

    [Header("Debug")]
    //Using isMoving to mark if boss is moving to player
    //If lost sight, then should stop
    [SerializeField] private bool m_bIsMoving = false;
    [SerializeField] private float distance = 0;
    [SerializeField] private Vector2 bossCurrentSpeed;
    [SerializeField] private Vector2 m_DirectionToPlayer = Vector2.zero;
    [SerializeField] private bool m_bPlayerSighted = false;
    [SerializeField] private bool m_bWander;
    [SerializeField] private float m_CurrentMechIntegrity;
    [SerializeField] private BOSSSTATE m_eCurrentState;

    private AINavigation m_ScriptAINavigate;
    private UnityEngine.AI.NavMeshAgent m_navMeshAgent;
    private float m_fTimePlayerOutOfSight;

   public enum BOSSSTATE
    {
        NONE,
        THRESHOLD1,
        THRESHOLD2,
        THRESHOLD3
    }

    public bool GetIfPlayerInSight()
    {
        return m_bPlayerSighted;
    }

    private void Start()
    {
        m_ScriptAINavigate = GetComponent<AINavigation>();
        m_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_navMeshAgent.speed = m_fMovementSpd;
        m_eCurrentState = BOSSSTATE.THRESHOLD1;
        m_bWander = true;
    }

    private void Update()
    {
        if (m_ScriptAINavigate == null)
        {
            m_bUseNavMesh = false;
        }

        if(m_bPlayerSighted)
        {
            if (m_DirectionToPlayer.x > 0)
            {
                m_animrBossAnimator.SetBool("FaceLeft", false);
                m_sprtrRenderer.flipX = true;
            }
            else if (m_DirectionToPlayer.x < 0)
            {
                m_animrBossAnimator.SetBool("FaceLeft", true);
                m_sprtrRenderer.flipX = false;
            }
        }
       
    }

    private void FixedUpdate()
    {
        m_navMeshAgent.speed = m_fMovementSpd;
        m_rgdbdyBossBody.velocity = Vector2.zero;

        if(!m_bUseNavMesh)
        {
            m_ScriptAINavigate.enabled = false;
            m_navMeshAgent.enabled = false;
            if (!m_bIsMoving)
            {
                if (CheckIfTooFar())
                {
                    //m_bIsMoving = true;
                    m_rgdbdyBossBody.MovePosition(m_rgdbdyBossBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
                    m_animrBossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
                }
            }
            else
            {
                if (CheckIfCloseEnough())
                {
                    m_bIsMoving = false;
                    m_DirectionToPlayer = Vector2.zero;
                    m_animrBossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
                }
                else
                {
                    m_rgdbdyBossBody.MovePosition(m_rgdbdyBossBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
                    m_animrBossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
                }
            }
        }
        else
        {
            m_ScriptAINavigate.enabled = true;
            m_navMeshAgent.enabled = true;
            bossCurrentSpeed = m_navMeshAgent.velocity;
            if(bossCurrentSpeed.magnitude > 1 )
            {
                //Make sure that horizontal has a value as long as currentSpeed is not zero in either axis
                m_animrBossAnimator.SetFloat("Horizontal", bossCurrentSpeed.x + (Mathf.Abs(bossCurrentSpeed.y)* bossCurrentSpeed.normalized.x) );
            }
            else
            {
                m_animrBossAnimator.SetFloat("Horizontal", 0);
            }
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ProjectilePlayer")
        {
            HP -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    private bool CheckIfTooFar()
    {
        CalculateDistance();
        return distance > m_fMaximumDistanceFromPlayer;
    }

    private bool CheckIfCloseEnough()
    {
        CalculateDistance();
        return distance <= m_fMinimumDistanceFromPlayer;
    }

    private void CalculateDistance()
    {
        distance = Vector2.Distance(m_rgdbdyPlayer.position, m_rgdbdyBossBody.position);
        m_DirectionToPlayer = (m_rgdbdyPlayer.position - m_rgdbdyBossBody.position).normalized;
    }

    public void CheckIfPlayerinView()
    {
        if(m_bUseNavMesh)
        {
            int maskIgnoreProjectileAndBoss = (1<<8) | (1 << 9) | (1 << 10) | (1 << 12) | (1 << 14);
            CalculateDistance();
            //Vector2 vectorToPlayer = m_rgdbdyPlayer.position - m_rgdbdyBossBody.position;
            RaycastHit2D checkSight = Physics2D.Raycast(this.transform.position, m_DirectionToPlayer, m_fMaximumDistanceFromPlayer+5, ~maskIgnoreProjectileAndBoss);
            Debug.DrawRay(transform.position, m_DirectionToPlayer * (m_fMaximumDistanceFromPlayer + 5), Color.blue);
            if (checkSight.collider != null && checkSight.collider.gameObject.tag == "Player")
            {
                m_ScriptAINavigate.PlayerSpotted((int)(m_fMinimumDistanceFromPlayer));
                m_bIsMoving = true;
                m_bPlayerSighted = true;
                m_bWander = false;
                m_fTimePlayerOutOfSight = 0;
                StopCoroutine(GoToLastKnowLocation());
                //m_fTimePlayerOutOfSight = 0;
                print("Found player, moving to where player is.");
            }
            else if(m_bPlayerSighted)
            {
                m_bPlayerSighted = false;
                StartCoroutine( GoToLastKnowLocation() );
                print("Lost sight of the player! Moving to last location");
                //m_ScriptAINavigate.PlayerSpotted((int)(0));
            }
        }
    }

    public BOSSSTATE GetBossState()
    {
        return m_eCurrentState;
    }
    private IEnumerator GoToLastKnowLocation()
    {
        m_ScriptAINavigate.PlayerSpotted(1);
        m_bPlayerSighted = false;
        print(".....moving....");

        yield return null;

        while(/*!(m_bPlayerSighted || */!m_ScriptAINavigate.GetIfReachedPosition() )
        {
            CalculateDistance();
            m_animrBossAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
            yield return null;
        }

        if(m_ScriptAINavigate.GetIfReachedPosition())
        {
            m_bIsMoving = false;
            print("Reached last known location. Waiting....");
            float delay = 0f;
            //while ( /*!(m_bPlayerSighted) && */(m_fTimePlayerOutOfSight < m_fDelayAfterLosingSight) )
            while ( /*!(m_bPlayerSighted) && */(delay < m_fDelayAfterLosingSight))
            {
                //m_fTimePlayerOutOfSight += 1 * Time.deltaTime;
                //BossAnimator.SetFloat("Horizontal", 0);
                delay += 1 * Time.deltaTime;
                yield return null;
            }

            if(!(m_bPlayerSighted))
            {
                m_ScriptAINavigate.PlayerLost();
                m_bWander = true;
                StartCoroutine( Wander() );
                print("Lost the player!");
            }
        }

        yield break;
    }

    private IEnumerator Wander()
    {
       while(!m_ScriptAINavigate.GetIfReachedPosition())
        {
            yield return null;
        }

        Vector2 chosenPatrolPoint = Vector2.zero;
        if( m_WanderSpot.Length > 0 )
        {
            print("Start Wandering...");
            WaitForSeconds delay = new WaitForSeconds(3);
            while (!m_bPlayerSighted)
            {
                while(chosenPatrolPoint == Vector2.zero)
                {
                   int randomIndex =  ((int)Random.value) * 10 % (m_WanderSpot.Length);
                    chosenPatrolPoint = m_WanderSpot[randomIndex].position;
                }

                m_ScriptAINavigate.GoToPosition(chosenPatrolPoint);

                yield return null;

                while(!m_ScriptAINavigate.GetIfReachedPosition())
                {
                    yield return null;
                }

                if(m_ScriptAINavigate.GetIfReachedPosition())
                {
                    chosenPatrolPoint = Vector2.zero;
                }

                yield return delay;
            }
        }
        else
        {
            print("No patrol points to wander.");
            m_bWander = false;
            yield break;
        }
    }
}
