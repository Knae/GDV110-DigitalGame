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

    //[Header("Debug")]
    //[SerializeField] private bool m_bDebugMode = false;

    [Header("Movement")]
    [SerializeField] private float m_fMoveSpeed = 2.0f;

    [Header("Variables")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float angle = 0.0f;

    private Vector2 movementvector = new Vector3(0, 0, 0);
    private Vector2 mousePos = new Vector3(0, 0, 0);


    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = gameCam.ScreenToWorldPoint(Input.mousePosition);

        movementvector.x = Input.GetAxis("Horizontal") * m_fMoveSpeed;
        movementvector.y = Input.GetAxis("Vertical") * m_fMoveSpeed;
    }

    void FixedUpdate()
    {
        //lookDir = mousePos - gunObject.transform.position;
        angle = FindAngleBetweenPoints(mousePos, transform.position);
        gunObject.transform.rotation = Quaternion.Euler( 0f, 0f, angle);

        //process movement
        playerBody.MovePosition(playerBody.position + (movementvector * Time.deltaTime));

        playerTorsoAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerTorsoAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
        playerLegsAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerLegsAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ProjectileEnemy")
        {
         
            currentHealth -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);

            healthbar.SetHealth(currentHealth);
        }
    }


    private float FindAngleBetweenPoints(Vector3 _inPointA, Vector3 _inPointB)
    {
        float angle = Mathf.Atan2(_inPointA.y- _inPointB.y, _inPointA.x- _inPointB.x) * Mathf.Rad2Deg;
        return angle;
    }
}