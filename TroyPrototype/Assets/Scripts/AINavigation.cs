using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
    [SerializeField] Transform target;

    [Header("Notable Variables")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Vector3 m_vec3DestinationPosition = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 m_vec3Position_StartingPosition;
    [SerializeField] bool m_bReachedLocation = false;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = 1;

        m_vec3DestinationPosition = this.transform.position;
        m_vec3Position_StartingPosition = this.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        agent.SetDestination(m_vec3DestinationPosition);
        float temp = Vector3.Distance(m_vec3DestinationPosition, transform.position);
        if ( temp <= (agent.stoppingDistance+0.5f))
        {
            m_bReachedLocation = true;
        }
        else
        {
            m_bReachedLocation = false;
        }
    }


    public void PlayerSpotted(int _inStopDistance)
    {
        m_vec3DestinationPosition = target.position;
        agent.stoppingDistance = _inStopDistance; 
    }

    public void PlayerLost()
    {
        m_vec3DestinationPosition = m_vec3Position_StartingPosition;
        agent.stoppingDistance = 1; 
    }

    public void GoToPosition(Vector2 _inPosition)
    {
        m_vec3DestinationPosition = _inPosition;
        agent.stoppingDistance = 1;
    }

    public bool GetIfReachedPosition()
    {
        return m_bReachedLocation;
    }
}
