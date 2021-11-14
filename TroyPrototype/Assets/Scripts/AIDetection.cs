using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDetection : MonoBehaviour
{
    public GameObject m_ParentObject;

    private BossBehaviour m_ScriptParentBehaviour;
    private bool m_bPlayerInRange;

    private void Start()
    {
        m_ScriptParentBehaviour = m_ParentObject.GetComponent<BossBehaviour>();
        m_bPlayerInRange = false;
    }

    private void Update()
    {
        if(m_bPlayerInRange)
        {
            m_ScriptParentBehaviour.CheckIfPlayerinView();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        m_bPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_bPlayerInRange = false;
    }
}
