using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup_Gun_FlameThrower : MonoBehaviour
{
    [SerializeField]
    //public Text pickUpText;
    public GameObject m_ObjectNearby = null;

    private bool pickUpAllowed;

    // Use this for initialization
    private void Start()
    {
        //pickUpText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (pickUpAllowed && Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
            m_ObjectNearby = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
            m_ObjectNearby = null;
        }
    }

    private void PickUp()
    {
        if(m_ObjectNearby != null)
        {
            m_ObjectNearby.GetComponentInChildren<WeaponFiring>().SetWeaponToStream();
            Destroy(gameObject);
        }

    }
}
