using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeTester : MonoBehaviour
{
    public Shaker Shaker;
    public float duration = 1.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Shaker.Shake(duration);
        }
    }
}
