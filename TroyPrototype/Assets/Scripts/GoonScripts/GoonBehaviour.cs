using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
public class GoonBehaviour : MonoBehaviour
{
    public Rigidbody2D m_rgdbdyGoonBody;
    public Rigidbody2D m_rgdbdyPlayer;
    public Animator m_animrGoonAnimator;
    public SpriteRenderer m_sprtrRenderer;

    public int m_goonType = 0;
    public bool m_goonCalled = false;

    [Header("Behaviour Constants")]
    [SerializeField] private float HP = 15f;
    [SerializeField] private float m_fMovementSpd = 1.0f;
    [SerializeField] private float m_fMinimumDistanceFromPlayer = 0.8f;
    [SerializeField] private float m_fMaximumDistanceFromPlayer = 1.1f;
    [SerializeField] private float m_fDelayAfterLosingSight = 2.0f;
    [SerializeField] private bool m_bUseNavMesh;

    [Header("WanderSpots")]
    [SerializeField] private Transform[] m_WanderSpot;

    [Header("Debug")]
    //Using isMoving to mark if baoss is moving to player
    //If lost sight, then should stop
    [SerializeField] private bool m_bIsMoving = false;
    [SerializeField] private float distance = 0;
    [SerializeField] private Vector2 goonCurrentSpeed;
    [SerializeField] private Vector2 m_DirectionToPlayer = Vector2.zero;
    [SerializeField] private bool m_bPlayerSighted = false;
    [SerializeField] private bool m_bWander;

    private AINavigation m_ScriptAINavigate;
    private UnityEngine.AI.NavMeshAgent m_navMeshAgent;
    private float m_fTimePlayerOutOfSight;

    public bool GetIfPlayerInSight()
    {
        return m_bPlayerSighted;
    }

    private void Start()
    {
        m_ScriptAINavigate = GetComponent<AINavigation>();
        m_navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        m_bWander = true;
    }

    private void Update()
    {
        if (m_ScriptAINavigate == null)
        {
            m_bUseNavMesh = false;
        }

        if (m_DirectionToPlayer.x > 0)
        {
           // m_animrGoonAnimator.SetBool("FaceLeft", false);
           m_sprtrRenderer.flipX = false;
        }
        else if (m_DirectionToPlayer.x < 0)
        {
           //m_animrGoonAnimator.SetBool("FaceLeft", true);
           m_sprtrRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        m_navMeshAgent.speed = m_fMovementSpd;
        m_rgdbdyGoonBody.velocity = Vector2.zero;

        if (m_goonCalled == false)
        {
            if (!m_bUseNavMesh)
            {
                m_ScriptAINavigate.enabled = false;
                m_navMeshAgent.enabled = false;
                if (!m_bIsMoving)
                {
                    if (CheckIfTooFar())
                    {
                        //m_bIsMoving = true;
                        m_rgdbdyGoonBody.MovePosition(m_rgdbdyGoonBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
                        m_animrGoonAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
                    }
                }
                else
                {
                    if (CheckIfCloseEnough())
                    {
                        m_bIsMoving = false;
                        m_DirectionToPlayer = Vector2.zero;
                        m_animrGoonAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
                    }
                    else
                    {
                        m_rgdbdyGoonBody.MovePosition(m_rgdbdyGoonBody.position + (m_DirectionToPlayer * m_fMovementSpd * Time.deltaTime));
                       m_animrGoonAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
                    }
                }
            }
            else
            {
                m_ScriptAINavigate.enabled = true;
                m_navMeshAgent.enabled = true;
                goonCurrentSpeed = m_navMeshAgent.velocity;
                if (goonCurrentSpeed.magnitude > 1)
                {
                    m_animrGoonAnimator.SetFloat("Horizontal", goonCurrentSpeed.x + (Mathf.Abs(goonCurrentSpeed.y) * goonCurrentSpeed.normalized.x));
                }
                else
                {
                    m_animrGoonAnimator.SetFloat("Horizontal", 0);
                }
            }
        }
        else
        {
            m_ScriptAINavigate.enabled = true;
            m_navMeshAgent.enabled = true;
            goonCurrentSpeed = m_navMeshAgent.velocity;
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
    //private void OnCollisionEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Projectile")
    //    {
    //        TakeDamage(collision.gameObject.GetComponent<BulletLifetime>().GetDamage());
    //        Destroy(collision.gameObject);
    //    }
    //}

    private bool CheckIfTooFar()
    {
        distance = CalculateDistance();
        return distance > m_fMaximumDistanceFromPlayer;
    }

    private bool CheckIfCloseEnough()
    {
        distance = CalculateDistance();
        return distance <= m_fMinimumDistanceFromPlayer;
    }

    private float CalculateDistance()
    {
        float temp = Vector2.Distance(m_rgdbdyPlayer.position, m_rgdbdyGoonBody.position);
        Vector2 vectorDiff = m_rgdbdyPlayer.position - m_rgdbdyGoonBody.position;

        if (vectorDiff.x == 0)
        {
            m_DirectionToPlayer.x = 0;
        }
        else
        {
            m_DirectionToPlayer.x = (vectorDiff.x / (Mathf.Abs(vectorDiff.x)));
        }

        if (vectorDiff.y == 0)
        {
            m_DirectionToPlayer.y = 0;
        }
        else
        {
            m_DirectionToPlayer.y = (vectorDiff.y / (Mathf.Abs(vectorDiff.y)));
        }

        return temp;
    }

    public void CheckIfPlayerinView()
    {
        if(m_bUseNavMesh)
        {
            int maskIgnoreProjectileAndBoss = (1 << 9) | (1 << 10) | (1 << 12) | (1 << 14);
            Vector2 vectorToPlayer = m_rgdbdyPlayer.position - m_rgdbdyGoonBody.position;
            RaycastHit2D checkSight = Physics2D.Raycast(this.transform.position, vectorToPlayer, m_fMaximumDistanceFromPlayer+10, ~maskIgnoreProjectileAndBoss);
            //Debug.DrawRay(gunFirePoint.position, gunFirePoint.up * (m_fRange+10), Color.yellow);
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

    private IEnumerator GoToLastKnowLocation()
    {
        m_ScriptAINavigate.PlayerSpotted(1);
        m_bPlayerSighted = false;
        print(".....moving....");

        yield return null;

        while(/*!(m_bPlayerSighted || */!m_ScriptAINavigate.GetIfReachedPosition() )
        {
            CalculateDistance();
           // m_animrGoonAnimator.SetFloat("Horizontal", m_DirectionToPlayer.x);
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

    public void TakeDamage(int _input)
    {
        HP -= _input;
        if(HP <=0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
