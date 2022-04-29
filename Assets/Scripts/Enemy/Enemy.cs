using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle=0,Execution=1
    }
    public EnemyState CurrentEnemyState;
    public float NoiseDetectionRadius;

    NavMeshAgent EnemyAgent;

    Vector3 NoiseLocation;
    Vector3 NoiseLookAtLocation;
    bool Rotating;
    
    void Start()
    {
        EnemyAgent = GetComponent<NavMeshAgent>();
        NoiseLookAtLocation = Vector3.zero;
        CurrentEnemyState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEnemyState == EnemyState.Idle)
        {
            if (NoiseManager.Noise.GetActiveNoise(NoiseDetectionRadius, transform.position))
            {
                NoiseLocation = NoiseManager.Noise.NoiseLocation;
                NoiseLookAtLocation.x = NoiseLocation.x;
                NoiseLookAtLocation.y = transform.position.y;
                NoiseLookAtLocation.z = NoiseLocation.z;
                if (!Rotating)
                {
                    Rotating = true;
                    StartCoroutine(RotateTowardsTarget(NoiseLookAtLocation, 0.75f));
                }

            }
        }
    }
    IEnumerator RotateTowardsTarget(Vector3 Target,float Duration)
    {
        Quaternion TargetRotation = Quaternion.LookRotation(Target-transform.position, transform.up);
        float t = 0;
        while(t<Duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        Rotating = false;
    }
}
