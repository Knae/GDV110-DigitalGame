using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSpawnPoint : MonoBehaviour
{
    [SerializeField]
    public Text letThemKnowtext;
    public GameObject NearSpawnObj = null;
    

    private bool setSpawnAllowed;

    public Transform respawnPoint;
    public PlayerButlerMovement Player;

    // Start is called before the first frame update
    private void Start()
    {
        letThemKnowtext.gameObject.SetActive(false);

    }

    // Update is called once per frame
    private void Update()
    {
        if (setSpawnAllowed && Input.GetKeyDown(KeyCode.E))
        {
            SetSpawn();
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                letThemKnowtext.gameObject.SetActive(true);
                setSpawnAllowed = true;
                NearSpawnObj = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Player"))
            {
                letThemKnowtext.gameObject.SetActive(false);
                setSpawnAllowed = false;
                NearSpawnObj = null;
            }
        }


        private void SetSpawn()
        {
            if (NearSpawnObj != null)
            {
                
            }
        }

    
}
