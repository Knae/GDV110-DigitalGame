using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadScene : MonoBehaviour
{
    //public Object sceneToLoad;
    public string sceneName = "Level1Lobby";

    bool bTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {

            //Destroy(collision.gameObject);
            //SceneManager.LoadScene("LevelMiddle");
            bTriggered = true;

            //SceneManager.LoadScene(sceneToLoad.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {

            //Destroy(collision.gameObject);
            //SceneManager.LoadScene("LevelMiddle");
            bTriggered = false;

            //SceneManager.LoadScene(sceneToLoad.name);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && bTriggered == true)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
