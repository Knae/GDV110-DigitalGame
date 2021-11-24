using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MozartTest2");
    }

    public void Quit()
    {
        Debug.Log("Quiting Button");
        Application.Quit();
    }

}
