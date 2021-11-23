using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGrenadeLauncher : MonoBehaviour
{
    public enum State
    {
        NONE,
        INACTIVE,
        PHASE1,
        PHASE2,
        PHASE3,
        PHASE4,
        PHASE5
    }

    [Header("Boss Grenade Launch Point")]
    public GameObject EnemyBody;
    public Transform FirePoint;
    public BulletWheel TurretWheel;
    public ClusterBomb ManySmallBooms;
    public ExplosiveProjectile BasicBoom;
    public Camera worldScreen;
    [Header("Base Settings")]
    [SerializeField] public float m_fFireAtRange = 4.0f;
    [SerializeField] public float m_fAngleOffset = -90.0f;
    [Header("Basic Grenade")]
    [SerializeField] public float m_fScatterRange = 0.25f;
    [SerializeField] public float m_fGrenadeRange = 4.0f;
    [SerializeField] public float m_fAIRangeVariation = 1.5f;
    [SerializeField] public float m_fBulletForce_Grenade = 1.0f;
    [SerializeField] public float m_fDmgRadius = 1.0f;
    [SerializeField] public float m_fExplosiveForce = 3.0f;
    [SerializeField] public float m_fFuseTime = 1.2f;
    [SerializeField] public float m_fTimeBetweenShots_Grenade = 2.0f;
    [SerializeField] public float m_fBulletDamage_Grenade = 10.0f;
    [Header("Cluster Bombs")]
    [SerializeField] private int   m_iFirePointsAmount_Cluster = 8;
    [SerializeField] private float m_fStartDelay_Cluster = 0f;
    [SerializeField] private float m_fRotateSpeed_Cluster = 100.0f;
    [SerializeField] private float m_fBombDamage_Cluster = 5.0f;
    [SerializeField] private float m_fExplosiveForce_Cluster = 5.0f;
    [SerializeField] private float m_fExplosiveRadius_Cluster = 0.5f;
    [SerializeField] private float m_fProjectileForce_Cluster = 1.0f;
    [SerializeField] private float m_fProjectileRange_Cluster = 5.0f;
    [Header("BulletWheel")]
    [SerializeField] private int m_iFirePointsAmount_Wheel = 2;
    [SerializeField] private float m_fTimeBetweenWheels = 6.0f;
    [SerializeField] private float m_fLifespan_Wheel = 5.0f;
    [SerializeField] private float m_fStartDelay_Wheel = 0f;
    [SerializeField] private float m_fRotateSpeed_Wheel = 100.0f;
    [SerializeField] private float m_fFiringInterval_Wheel = 0.3f;
    [SerializeField] private float m_fProjectileDamage_Wheel = 1.0f;
    [SerializeField] private float m_fProjectileForce_Wheel = 1.5f;
    [SerializeField] private float m_fProjectileRange_Wheel = 3.0f;

    [Header("Phase 2 Settings")]
    [SerializeField] private bool m_bEnableGrenades = false;

    [Header("Phase 3 Settings")]
    [Header("Grenades")]


    [Header("Cluster")]
    [SerializeField] private bool m_bEnableClusters = false;

    [Header("Phase 4 Settings")]
    [Header("Bullet Wheel")]
    [SerializeField] private bool m_bEnableWheel = false;


    [Header("Sound")]
    public AudioSource sourceGrenade;
    public AudioClip clipGrenade;
    public AudioSource sourceCluster;
    public AudioClip clipCluster;

    [Header("Debug")]
    State m_eGrenadeLauncherState = State.NONE;
    private float m_fCountupGrenades = 0.0f;
    //private float m_fCountupRadialProjectiles = 0.0f;
    private float m_fCountupTurret = 0.0f;
    private int m_iGrenadeCount = 1;

    //Start is called before the first frame update
    private void Start()
    {
        m_eGrenadeLauncherState = State.PHASE1;
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_bEnableGrenades)
        {
            m_fCountupGrenades += Time.deltaTime;
        }

        if(m_bEnableWheel)
        {
            m_fCountupTurret += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 playerCurrentPosition = EnemyBody.transform.position;
        float angleFacingPlayer = FindAngleToPlayer(playerCurrentPosition);
        FirePoint.rotation = Quaternion.Euler(0f, 0f, angleFacingPlayer + m_fAngleOffset);
    }

    public void ResetTimers()
    {
        m_fCountupGrenades = 0.0f;
        //m_fCountupRadialProjectiles = 0.0f;
        m_fCountupTurret = 0.0f;
    }

    public void FireWeapons()
    {
        if(m_bEnableGrenades)
        {
            if(m_iGrenadeCount<2)
            {
                FireGrenade();
            }
            else if(m_bEnableClusters)
            {
                FireClusterBomb();
            }
        }

        if(m_bEnableWheel)
        {
            FireBulletWheel();
        }
    }

    private void FireGrenade()
    {
        if (m_fCountupGrenades >= m_fTimeBetweenShots_Grenade)
        {
            float distanceToTarget = Vector3.Distance(transform.position, EnemyBody.transform.position);
            float targetRange = Random.Range(distanceToTarget - m_fAIRangeVariation, distanceToTarget + m_fAIRangeVariation);

            //if (true)
            //{
                ExplosiveProjectile newBomb = Instantiate(BasicBoom, FirePoint.position, FirePoint.rotation);
                //newBomb.AssignCameraSource(camera);
                newBomb.SetBulletRange(targetRange);
                newBomb.SetExploRadius(m_fDmgRadius);
                newBomb.SetExplosiveForce(m_fExplosiveForce);
                newBomb.SetFuseTime(m_fFuseTime);
                Rigidbody2D newBulletBody = newBomb.GetComponent<Rigidbody2D>();
                Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
            newBulletBody.AddForce((FirePoint.up + directionRandomizer) * m_fBulletForce_Grenade, ForceMode2D.Impulse); 

             //}

            m_fCountupGrenades = 0.0f;
            if(m_bEnableClusters)
            {
                m_iGrenadeCount++;
            }

            if (sourceGrenade != null)
            {
                sourceGrenade.PlayOneShot(clipGrenade);
            } 
        }
    }

    private void FireClusterBomb()
    {
        if (m_fCountupGrenades >= m_fTimeBetweenShots_Grenade)
        {
            float distanceToTarget = Vector3.Distance(transform.position, EnemyBody.transform.position);
            float targetRange = Random.Range(distanceToTarget - m_fAIRangeVariation, distanceToTarget + m_fAIRangeVariation);

            ClusterBomb newCluster = SetUpClusterBomb(targetRange);
            Rigidbody2D newBulletBody = newCluster.GetComponent<Rigidbody2D>();
            Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
            newBulletBody.AddForce((FirePoint.up + directionRandomizer) * m_fProjectileForce_Cluster, ForceMode2D.Impulse);
            m_fCountupGrenades = 0.0f;
            m_iGrenadeCount = 1;
            if (sourceGrenade != null)
            {
                sourceGrenade.PlayOneShot(clipGrenade);
            }
        }
    }

    private void FireBulletWheel()
    {
        if (m_fCountupTurret >= m_fTimeBetweenWheels)
        {
            float distanceToTarget = Vector3.Distance(transform.position, EnemyBody.transform.position);
            float targetRange = Random.Range(distanceToTarget - m_fAIRangeVariation, distanceToTarget + m_fAIRangeVariation);

            BulletWheel newWheel = SetUpBulletWheel(targetRange,m_fLifespan_Wheel);
            Rigidbody2D newBulletBody = newWheel.GetComponent<Rigidbody2D>();
            Vector3 directionRandomizer = new Vector3(Random.Range(-m_fScatterRange, m_fScatterRange), Random.Range(-m_fScatterRange, m_fScatterRange), 0);
            newBulletBody.AddForce((FirePoint.up + directionRandomizer) * m_fProjectileForce_Wheel, ForceMode2D.Impulse);
            m_fCountupTurret = 0.0f;
            //if (sourceGrenade != null)
            //{
            //    sourceGrenade.PlayOneShot(clipGrenade);
            //}
        }
    }

    private void SpawnBulletWheelUnderBoss()
    {
        //make it last 10 minutes and shoot bullets that go very far
        BulletWheel newWheel = SetUpBulletWheel(0.0f, 3000.0f);
        newWheel.m_iNumberOfFirePoints = 4;
        newWheel.m_fRotateSpeed = 150.0f;
        newWheel.m_fFiringInterval = 0.2f;
        newWheel.m_fProjectileDamage = 1.5f;
        newWheel.m_fProjectileForce = 2.0f;
        newWheel.m_fProjectileRange = 100.0f;
    }

    public void AdvanceState()
    {
        switch (m_eGrenadeLauncherState)
        {
            case State.PHASE1:
            {
                ChangeState(State.PHASE2);
                break;
            }
            case State.PHASE2:
            {
                ChangeState(State.PHASE3);
                break;
            }
            case State.PHASE3:
            {
                ChangeState(State.PHASE4);
                break;
            }
            case State.PHASE4:
            {
                SpawnBulletWheelUnderBoss();
                break;
            }
            case State.NONE:
            case State.INACTIVE:
            default:
            {
                print("No more phases to advance to!");
                break;
            }
        }
    }

    public void ChangeState(State _input)
    {
        switch (_input)
        {
            case State.PHASE2:
            {
                m_bEnableGrenades = true;
                m_bEnableWheel = true;
                m_eGrenadeLauncherState = State.PHASE2;
                break;
            }
            case State.PHASE3:
            {
                m_bEnableClusters = true;
                m_iFirePointsAmount_Wheel = 2;
                m_eGrenadeLauncherState = State.PHASE3;
                break;
            }
            case State.PHASE4:
            {
                m_iFirePointsAmount_Wheel = 4;
                m_fLifespan_Wheel = 7.0f;
                m_eGrenadeLauncherState = State.PHASE4;
                break;
            }
            case State.PHASE5:
            {
                m_iFirePointsAmount_Wheel = 4;
                m_fLifespan_Wheel = 7.0f;
                m_eGrenadeLauncherState = State.PHASE5;
                break;
            }
            case State.NONE:
            case State.INACTIVE:
            default:
            {
                break;
            }
        }
    }

    private ClusterBomb SetUpClusterBomb(float _inputDelay)
    {
        ClusterBomb newCluster = Instantiate(ManySmallBooms, FirePoint.position, FirePoint.rotation);
        //newBomb.AssignCameraSource(camera);
        newCluster.m_iNumberOfFirePoints = m_iFirePointsAmount_Cluster;
        newCluster.m_fActivationDelay = _inputDelay;
        newCluster.m_fRotateSpeed = m_fRotateSpeed_Cluster;
        newCluster.m_fBombDamage = m_fBombDamage_Cluster;
        newCluster.m_fExplosiveForce  = m_fExplosiveForce_Cluster;
        newCluster.m_fExplosiveRadius  = m_fExplosiveRadius_Cluster;
        newCluster.m_fProjectileRange = m_fProjectileRange_Cluster;

        return newCluster;
    }

    private BulletWheel SetUpBulletWheel(float _inputDelay, float _inputLifespan)
    {
        BulletWheel newWheel = Instantiate(TurretWheel, FirePoint.position, FirePoint.rotation);
        newWheel.m_iNumberOfFirePoints = m_iFirePointsAmount_Wheel;
        newWheel.m_fActivationDelay = _inputDelay;
        newWheel.m_fRotateSpeed = m_fRotateSpeed_Wheel;
        newWheel.m_fFiringInterval = m_fFiringInterval_Wheel;
        newWheel.m_fProjectileDamage = m_fProjectileDamage_Wheel;
        newWheel.m_fProjectileForce = m_fProjectileForce_Wheel;
        newWheel.m_fProjectileRange = m_fProjectileRange_Wheel;
        newWheel.m_fLifeTime = _inputLifespan;

        return newWheel;
    }

    private float FindAngleToPlayer(Vector3 _inPointA)
    {
        return Mathf.Atan2(_inPointA.y - transform.position.y, _inPointA.x - transform.position.x) * Mathf.Rad2Deg;
    }
}
