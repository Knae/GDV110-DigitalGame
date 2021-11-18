using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGoonCall : MonoBehaviour
{
    public GameObject[] allGoons;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            allGoons = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject Enemy in allGoons)
            {
                if (Enemy.GetComponent<GoonBehaviour>().m_goonType == 0)
                {
                    Enemy.GetComponent<GoonBehaviour>().m_goonCalled = true;
                }
            }
        }
    }
}
