using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public void StartAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MozartTest2");
    }

    public void No()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}
