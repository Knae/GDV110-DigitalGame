using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerButlerMovement : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public GameObject gunObject; 
    public SpriteRenderer gunObjectSprite;
    public Animator playerTorsoAnimator;
    public Animator playerLegsAnimator;
    public Camera gameCam;
    public HealthBar healthbar;

    [Header("Movement")]
    [SerializeField] private float m_fMoveSpeed = 2.0f;

    [Header("Variables")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    [SerializeField] private float angle = 0.0f;

    [Header("Debug")]
    //[SerializeField] private bool m_bDebugMode = false;
    [SerializeField] private bool m_bGodMode = true;
    [SerializeField] private bool m_bHasHealthBar;
    [SerializeField] private bool canRespawn;

    private Vector2 movementvector = new Vector3(0, 0, 0);
    private Vector2 mousePos = new Vector3(0, 0, 0);
    private SpriteRenderer m_playerLegsRenderer;
    private SpriteRenderer m_playerTorsoRenderer;

    //respawn stuff
    private Vector3 respawnPoint;

    void Start()
    {
        currentHealth = maxHealth;
        if(healthbar!=null)
        {
            healthbar.SetMaxHealth(maxHealth);
            m_bHasHealthBar = true;
        }
        else
        {
            m_bHasHealthBar = false;
        }
        m_playerLegsRenderer =  transform.Find("PlayerLegs").GetComponent<SpriteRenderer>();
        m_playerTorsoRenderer = transform.Find("PlayerTorso").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bHasHealthBar)
        {
            healthbar.SetHealth(currentHealth);
        }

        mousePos = gameCam.ScreenToWorldPoint(Input.mousePosition);

        movementvector.x = Input.GetAxis("Horizontal") * m_fMoveSpeed;
        movementvector.y = Input.GetAxis("Vertical") * m_fMoveSpeed;

        Die();
        //Respawn();
    }

    void FixedUpdate()
    {
        //lookDir = mousePos - gunObject.transform.position;
        angle = FindAngleBetweenPoints(mousePos, transform.position);
        gunObject.transform.rotation = Quaternion.Euler( 0f, 0f, angle);
        if (Mathf.Abs(angle) > 90)
        {
            gunObjectSprite.flipY = true;
        }
        else
        {
            gunObjectSprite.flipY = false;
        }


        //process movement
        playerBody.velocity = movementvector;

        playerTorsoAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerTorsoAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        playerLegsAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerLegsAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));

        if (playerTorsoAnimator.GetFloat("Horizontal") > 0)
        {
            m_playerLegsRenderer.flipX = false;
            m_playerTorsoRenderer.flipX = false;
            playerLegsAnimator.SetBool("Flipped", false);
            //playerTorsoAnimator.SetBool("Flipped", false);
        }
        else if (playerTorsoAnimator.GetFloat("Horizontal") < 0)
        {
            m_playerLegsRenderer.flipX = true;
            m_playerTorsoRenderer.flipX = true;
            playerLegsAnimator.SetBool("Flipped", true);
            //playerTorsoAnimator.SetBool("Flipped", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ProjectileEnemy")
        {
            float damage = 0;
            if (collision.gameObject.GetComponent<BulletLifetime>()!=null)
            {
                damage = collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            }
            else if(collision.gameObject.GetComponent<ExplosiveProjectile>() != null)
            {
                damage = collision.gameObject.GetComponent<ExplosiveProjectile>().GetDamage();
            }
            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "SetSpawn")
        {
            canRespawn = true;
            Debug.Log("OAJHGO");
            respawnPoint = transform.position;
            Destroy(collision.gameObject);
        }
    }

    private float FindAngleBetweenPoints(Vector3 _inPointA, Vector3 _inPointB)
    {
        float angle = Mathf.Atan2(_inPointA.y- _inPointB.y, _inPointA.x- _inPointB.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void TakeDamage(float _inDamage)
    {
        if (!m_bGodMode)
        {
            currentHealth -= _inDamage;
        }
    }

    public void HealPlayer(float _input)
    {
        currentHealth += _input;
    }


    void Respawn()
    {

        if (currentHealth <= 0)
        {
            canRespawn = false;
            Debug.Log("YOU DIED");
            currentHealth = maxHealth;
            transform.position = respawnPoint;

        }

    }

    void Die()
    {
        if (currentHealth <= 0 && canRespawn == false)
        {
            SceneManager.LoadScene("Death");
        }
    }
}