using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;
    private NavMeshAgent navMeshAgent;
    public GameObject player;
    private bool foundPlayer;
    private int currentWaypointIndex;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        foundPlayer = false;
        currentWaypointIndex = 0;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
    }

    private void Update()
    {
        if (foundPlayer)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            foundPlayer = true;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            Debug.Log("Player detected - attack!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            foundPlayer = false;
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            Debug.Log("Player out of range, resume patrol");
        }
    }
}
