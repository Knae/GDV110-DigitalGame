using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LoadScene : MonoBehaviour
{
    //public Object sceneToLoad;
    public string sceneName = "Level1Lobby";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //Destroy(collision.gameObject);
            //SceneManager.LoadScene("LevelMiddle");
            SceneManager.LoadScene(sceneName);

            //SceneManager.LoadScene(sceneToLoad.name);
        }
    }
}
