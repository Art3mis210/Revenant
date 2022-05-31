using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Civilians : MonoBehaviour
{
    NavMeshAgent EnemyAgent;
    Vector3 WanderPosition;
    void Start()
    {
        EnemyAgent = GetComponent<NavMeshAgent>();
        WanderPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (WanderPosition == Vector3.zero)
        {
            EnemyAgent.SetDestination(WanderPosition = RandomNavSphere(transform.position, Random.Range(100, 200)));
        }
        if (Vector3.Distance(transform.position, WanderPosition) < 5f)
        {
            EnemyAgent.SetDestination(WanderPosition = RandomNavSphere(transform.position, Random.Range(10, 20)));
        }
    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, 3);
        return navHit.position;
    }
}
