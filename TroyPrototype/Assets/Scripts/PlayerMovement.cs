using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    public GameObject playerBody;
    public GameObject gunObject;
    public Animator playerAnimator;
    public Camera gameCam;


    [Header("Debug")]
    public bool m_bDebugMode = false;

    [Header("Movement")]
    public float m_fMoveSpeed = 2.0f;

    [Header("Variables")]
    //HP VARIABLES
    public int HP;
    public int NumOHeart;

    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHearts;
    //damage

    //int damage = 1;

    ///////////////////////////////////////////////
    public float angle = 0.0f;

    private Vector3 movementvector = new Vector3(0, 0, 0);
    private Vector3 mousePos = new Vector3(0, 0, 0);

    // Update is called once per frame
    void Update()
    {
        //HP SETTINGS
        for (int i = 0; i < hearts.Length; i++)
        {
            if (HP > NumOHeart) //Keep HP to the maximum they have displayed
            {
                HP = NumOHeart;
            }

            if (i < HP) //Displays full hearts or emptyHearts
            {
                hearts[i].sprite = fullHearts;
            }
            else
            {
                hearts[i].sprite = emptyHearts;
            }

            if (i < NumOHeart)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        ////////////////

        mousePos = gameCam.ScreenToWorldPoint(Input.mousePosition);

        movementvector.x = Input.GetAxis("Horizontal") * m_fMoveSpeed;
        movementvector.y = Input.GetAxis("Vertical") * m_fMoveSpeed;
    }

    void FixedUpdate()
    {
        //lookDir = mousePos - gunObject.transform.position;
        angle = FindAngleBetweenPoints(mousePos, transform.position);
        gunObject.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        //process movement
        playerBody.transform.Translate(movementvector * Time.deltaTime);

        playerAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "ProjectileEnemy")
        {
            //NumOHeart -= damage;
            //NumOHeart -= collision.gameObject.GetComponent<BulletLifetime>().GetDamage();
            Destroy(collision.gameObject);
        }
    }

    private float FindAngleBetweenPoints(Vector3 _inPointA, Vector3 _inPointB)
    {
        float angle = Mathf.Atan2(_inPointA.y - _inPointB.y, _inPointA.x - _inPointB.x) * Mathf.Rad2Deg;
        return angle;
    }
}