using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AnimalAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float randomWalkRadius = 5f;
    [SerializeField] private float followDistance = 10f;
    [SerializeField] private float waitTimeAtPoint = 2f;
    [SerializeField] private bool canFollowPlayer = true;

    private Vector3 randomDestination;
    private bool isWaiting = false;

    private void Start()
    {
        canFollowPlayer = Random.value > 0.5f;
        PickRandomDestination();
    }

    private void Update()
    {
        if (canFollowPlayer && PlayerInRange())
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f && !isWaiting)
            {
                StartCoroutine(WaitBeforeNextDestination());
            }
        }
    }

    private bool PlayerInRange()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance <= followDistance;
    }

    private void PickRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * randomWalkRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, randomWalkRadius, NavMesh.AllAreas))
        {
            randomDestination = hit.position;
            agent.SetDestination(randomDestination);
        }
    }

    private IEnumerator WaitBeforeNextDestination()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        PickRandomDestination();
        isWaiting = false;
    }
}
