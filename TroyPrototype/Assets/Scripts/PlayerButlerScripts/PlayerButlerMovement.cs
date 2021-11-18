using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButlerMovement : MonoBehaviour
{
    public Rigidbody2D playerBody;
    public GameObject gunObject;
    public Animator playerTorsoAnimator;
    public Animator playerLegsAnimator;
    public Camera gameCam;
    public HealthBar healthbar;

    [Header("Movement")]
    [SerializeField] private float m_fMoveSpeed = 2.0f;

    [Header("Variables")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] public float currentHealth;
    [SerializeField] private float angle = 0.0f;

    [Header("Debug")]
    //[SerializeField] private bool m_bDebugMode = false;
    [SerializeField] private bool m_bGodMode = true;
    [SerializeField] private bool m_bHasHealthBar;

    [SerializeField] private bool canRespawn;

    private Vector2 movementvector = new Vector3(0, 0, 0);
    private Vector2 mousePos = new Vector3(0, 0, 0);


    void Start()
    {
        currentHealth = maxHealth;

        if (healthbar!=null)
        {
            healthbar.SetMaxHealth(maxHealth);
            m_bHasHealthBar = true;
        }
        else
        {
            m_bHasHealthBar = false;
        }

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

        //When HP is 0 then die and then respawn

        Respawn();


        if (Input.GetButtonDown("Fire1"))
        {
            TakeDamage(50);
            Debug.Log("TAKING DAMAGE");
        }
        
    }

    void FixedUpdate()
    {
        //lookDir = mousePos - gunObject.transform.position;
        angle = FindAngleBetweenPoints(mousePos, transform.position);
        gunObject.transform.rotation = Quaternion.Euler( 0f, 0f, angle);

        //process movement
        //playerBody.MovePosition(playerBody.position + (movementvector * Time.deltaTime));
        playerBody.velocity = movementvector;

        playerTorsoAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerTorsoAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        playerLegsAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerLegsAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ProjectileEnemy")
        {
            int damage = collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
    }

    private float FindAngleBetweenPoints(Vector3 _inPointA, Vector3 _inPointB)
    {
        float angle = Mathf.Atan2(_inPointA.y- _inPointB.y, _inPointA.x- _inPointB.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void TakeDamage(int _inDamage)
    {
        if (!m_bGodMode)
        {
            currentHealth -= _inDamage;
        }
    }

    public void AddHealth(int _addHealth)
    {
        if (!m_bGodMode)
        {
            currentHealth += _addHealth;
        }
    }

    void Respawn()
    {
        if (canRespawn = true)
        {
            if (currentHealth <= 0)
            {

                Debug.Log("YOU DIED");
                LevelManager.instance.Respawn();
                //currentHealth = maxHealth;
                Destroy(gameObject);

            }
        }
        else
        {
            canRespawn = false;
            Debug.Log("Cannot Respawn");
        }
    }
}