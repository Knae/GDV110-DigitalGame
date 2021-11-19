using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGoonCall : MonoBehaviour
{
    public GameObject[] allGoons;
    public bool bButtonPressed = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (bButtonPressed == false)
            {
                allGoons = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject Enemy in allGoons)
                {
                    if (Enemy.GetComponent<GoonBehaviour>().m_goonType == 0)
                    {
                        Enemy.GetComponent<GoonBehaviour>().m_goonCalled = true;
                    }
                }

                bButtonPressed = true;
            }
            else
            {
                for (int i = 0; i < allGoons.Length; i++)
                {
                    allGoons[i] = null;
                }
                allGoons = null;

                bButtonPressed = false;
            }
        }
    }
}
