using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlave : MonoBehaviour
{
    public Rigidbody2D objectPlayer;
    public Camera gameCamera;


    [Header("Camera Margins")]
    public float m_fMarginsX = 1.2f;
    public float m_fMarginsY = 1.2f;
    public float m_fMoveToLimit = 0.3f;
    public float m_fSmoothFollow = 1.0f;

    private bool m_bIsMoving = false;
    private Shaker m_ShakerScript;

    private void Start()
    {
        if(GetComponent<Shaker>() != null)
        {
            m_ShakerScript = GetComponent<Shaker>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Check both planes if camera exceeds either margins, then
        //compensate as necessary
        Vector3 targetLocation = gameCamera.transform.position;

        if (!m_bIsMoving)
        {
            if (CheckMarginX() || CheckMarginY() )
            {
                m_bIsMoving = true;
                targetLocation.x = Mathf.Lerp(targetLocation.x, objectPlayer.position.x, m_fSmoothFollow * Time.deltaTime);
                targetLocation.y = Mathf.Lerp(targetLocation.y, objectPlayer.position.y, m_fSmoothFollow * Time.deltaTime);
            }
        }
        else
        {
            if(!CheckIfCaughtUp())
            {
                targetLocation.x = Mathf.Lerp(targetLocation.x, objectPlayer.position.x, m_fSmoothFollow * Time.deltaTime);
                targetLocation.y = Mathf.Lerp(targetLocation.y, objectPlayer.position.y, m_fSmoothFollow * Time.deltaTime);
            }
            else
            {
                m_bIsMoving = false;
            }
        }
        if(m_ShakerScript!=null && m_ShakerScript._isShaking)
        {
            targetLocation += m_ShakerScript.randomPoint;
        }
        gameCamera.transform.position = targetLocation;
    }

    //these 2 functions check if the differences between then camera and the player
    //in a plane exceeds the marging values
    bool CheckMarginX()
    {
        return Mathf.Abs( (objectPlayer.position.x) - (gameCamera.transform.position.x) )> m_fMarginsX;
    }

    bool CheckMarginY()
    {

        return Mathf.Abs( (objectPlayer.position.y) - (gameCamera.transform.position.y) ) > m_fMarginsY;
    }

    bool CheckIfCaughtUp()
    {
        bool checkX = Mathf.Abs((objectPlayer.position.x) - (gameCamera.transform.position.x)) <= m_fMoveToLimit;
        bool checkY = Mathf.Abs((objectPlayer.position.y) - (gameCamera.transform.position.y)) <= m_fMoveToLimit;
        return checkX && checkY;
    }
}
