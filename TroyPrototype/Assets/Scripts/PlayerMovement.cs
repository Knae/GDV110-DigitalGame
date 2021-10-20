using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerBody;
    public Rigidbody2D gunObject;
    public Animator playerAnimator;
    public Camera gameCam;

    [Header("Debug")]
    public bool m_bDebugMode = false;

    [Header("Movement")]
    public float m_fMoveSpeed = 2.0f;

    [Header("Variables")]
    public int HP = 10;
    Vector3 movementvector = new Vector3(0, 0, 0);
    Vector2 mousePos = new Vector2(0, 0);

    // Update is called once per frame
    void Update()
    {
        mousePos = gameCam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - gunObject.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        gunObject.rotation = angle;

        movementvector.x = Input.GetAxis("Horizontal") * m_fMoveSpeed;
        movementvector.y = Input.GetAxis("Vertical") * m_fMoveSpeed;

        //process movement
        playerBody.transform.Translate(movementvector * Time.deltaTime);

        playerAnimator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        playerAnimator.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ProjectileEnemy")
        {
            
            Destroy(collision.gameObject);
            HP--;
        }
    }
}
