using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : MonoBehaviour
{
    //public CircleCollider2D m_ThisCollider;
    public Camera m_SceneCamera;

    [Header("ProjectileSettings")]
    [SerializeField] private int m_iDamage = 5;
    [SerializeField] private float m_fRange = 0.0f;
    [SerializeField] private float m_fExploRadius = 0.7f;
    [SerializeField] private float m_fExplosiveForce = 5.0f;
    [SerializeField] private float m_fFuseTime = 1.3f;
    [SerializeField] private float m_fDistanceTraveled = 0f;
    [Header("Debug Variables")]
    [SerializeField] private const float m_iSpriteRadius = 32.0f;
    [SerializeField] private Shaker m_refCameraShaker;
    [SerializeField] private bool m_bPrimed = false;
    [SerializeField] private bool m_bWillHurtFriendlies = false;
    [SerializeField] private Vector3 m_PrevPos = Vector3.zero;

    private void Start()
    {
        m_PrevPos = transform.position;
        m_SceneCamera = Camera.main;
        m_refCameraShaker = m_SceneCamera.GetComponent<Shaker>();
    }

    void FixedUpdate()
    {
        if(!m_bPrimed)
        {
            m_fDistanceTraveled += (transform.position - m_PrevPos).magnitude;
            m_PrevPos = transform.position;
            if (m_fDistanceTraveled >= m_fRange)
            {
                PrimeForDetonation();
            }
        }
        else
        {
            m_fFuseTime -=Time.deltaTime;
            if (m_fFuseTime <= 0.0f)
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!m_bPrimed)
        {
            PrimeForDetonation();
        }
    }

    private void Explode()
    {
        int maskToDetect = (1 << 10) | (1 << 11) | (1 << 12);
        Vector3 grenadePosition = transform.position;
        Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(transform.position, m_fExploRadius/2, maskToDetect);
        foreach (var element in affectedObjects)
        {
            if(element.tag == "Player")
            {
                PlayerButlerMovement refPlayerScript = element.gameObject.GetComponent<PlayerButlerMovement>();
                if(refPlayerScript !=null)
                {
                    refPlayerScript.TakeDamage(m_iDamage);
                }
            }
            else if(m_bWillHurtFriendlies)
            {
                if (element.tag == "Enemy")
                {
                    GoonBehaviour refGoonScript = element.gameObject.GetComponent<GoonBehaviour>();
                    if (refGoonScript != null)
                    {
                        refGoonScript.TakeDamage(m_iDamage);
                    }
                }
                else if (element.tag == "Destructible")
                {
                    DestructibleObject refDestructible = element.gameObject.GetComponent<DestructibleObject>();
                    if (refDestructible != null)
                    {
                        refDestructible.TakeDamage(m_iDamage);
                    }
                }
            }
            Rigidbody2D elementBody = element.GetComponent<Rigidbody2D>();
            if (elementBody != null)
            {
                Vector2 forceDirection = element.transform.position - grenadePosition;
                float distance = forceDirection.magnitude;
                float force = Mathf.Lerp(0, m_fExplosiveForce, (m_fExploRadius - distance));
                Vector2 forceToAdd = forceDirection.normalized * force;
                elementBody.AddForce(forceToAdd, ForceMode2D.Impulse);
            }

            //Rigidbody2D targetBody = element.GetComponent<Rigidbody2D>();
            //if(targetBody!=null)
            //{
            //    targetBody.
            //}
        }

        if(m_refCameraShaker!=null)
        {
            m_refCameraShaker.Shake(0.5f);
        }

        Destroy(gameObject);
    }

    public void SetFuseTime(float _input)
    {
        m_fFuseTime = _input;
    }

    public void SetExploRadius(float _input)
    {
        m_fExploRadius = _input;
    }

    public void SetBulletRange(float _input)
    {
        m_fRange = _input;
    }

    public void SetExplosiveForce(float _input)
    {
        m_fExplosiveForce = _input;
    }
    public void SetDamage(int _input)
    {
        m_iDamage = _input;
    }

    public void AssignCameraSource(Camera _input)
    {
        m_SceneCamera = _input;
    }

    public int GetDamage()
    {
        return m_iDamage;
    }

    public void SetToKillFriendlies()
    {
        m_bWillHurtFriendlies = true;
    }

    public void SetToIgnoreFriendlies()
    {
        m_bWillHurtFriendlies = false;
    }

    private void PrimeForDetonation()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        m_bPrimed = true;
    }
}
