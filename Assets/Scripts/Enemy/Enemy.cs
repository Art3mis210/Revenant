using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,Patrol,Execution,Investigate,AttackPlayer
    }
    public bool PlayerFound;
    
    public EnemyState CurrentEnemyState;
    public float NoiseDetectionRadius;
    public float VisualDetectionRadius;
    public float VisualDetectionAngle;
    public PatrolPoints patrolPoint;
    public bool PatrolDir;
    public int StartingPoint;
    Transform CurrentPatrolPoint;

    NavMeshAgent EnemyAgent;
    Vector3 NoiseLocation;
    Vector3 NoiseLookAtLocation;
    bool Rotating;
    float Speed;
    bool SpeedChanging;
    Animator EnemyAnimator;
    RaycastHit hit;
    Rigidbody EnemyRigidbody;
    
    
    void Start()
    {
        EnemyAgent = GetComponent<NavMeshAgent>();
        NoiseLookAtLocation = Vector3.zero;
        //CurrentEnemyState = EnemyState.Idle;
        EnemyAnimator = GetComponent<Animator>();
        EnemyRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEnemyState != EnemyState.Execution && CurrentEnemyState != EnemyState.AttackPlayer)
        {
            if (NoiseManager.Noise.GetActiveNoise(NoiseDetectionRadius, transform.position))
            {
                CurrentEnemyState = EnemyState.Investigate;
                if (SpeedChanging == false)
                {
                    SpeedChanging = true;
                    StartCoroutine(ChangeSpeed(0f, 1f));
                }
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
            if(Vector3.Distance(transform.position,PlayerController.Player.transform.position)<=VisualDetectionRadius)
            {
                if (Mathf.Abs(Vector3.Angle(transform.forward, PlayerController.Player.transform.position-transform.position))<=VisualDetectionAngle)
                {
                    Debug.DrawRay(transform.position + transform.up + 0.2f*transform.forward,(PlayerController.Player.transform.position- transform.position), Color.red, 2f);
                    if(Physics.Raycast(transform.position + transform.up + 0.2f * transform.forward, (PlayerController.Player.transform.position - transform.position), out hit,VisualDetectionRadius))
                    {
                        if(hit.transform.gameObject.tag=="Player")
                        {
                            CurrentEnemyState = EnemyState.AttackPlayer;
                        }
                    }
                }
            }
        }
        if(CurrentEnemyState == EnemyState.Idle)
        {
            if (Speed != 0f)
            {
                if (SpeedChanging == false)
                {
                    SpeedChanging = true;
                    StartCoroutine(ChangeSpeed(0f, 1f));
                }
            }
        }
        if (CurrentEnemyState == EnemyState.AttackPlayer)
        {
            if (EnemyAgent.enabled == false)
                EnemyAgent.enabled = true;
            if (Vector3.Distance(transform.position, PlayerController.Player.transform.position) > VisualDetectionRadius / 2)
            {
                EnemyAgent.SetDestination(PlayerController.Player.transform.position);
                if (SpeedChanging == false && Speed != 2f)
                {
                    SpeedChanging = true;
                    StartCoroutine(ChangeSpeed(2f, 1f));
                }
            }
            else
            {
                EnemyAgent.ResetPath();
                if (SpeedChanging == false && Speed != 0f)
                {
                    SpeedChanging = true;
                    StartCoroutine(ChangeSpeed(0f, 1f));
                }
                if (!Rotating)
                 {
                     Rotating = true;
                     StartCoroutine(RotateTowardsTarget(PlayerController.Player.transform.position, 1f));
                 }
                Debug.DrawRay(transform.position + transform.up + 0.2f * transform.forward, (PlayerController.Player.transform.position - transform.position), Color.red, 1f);
                if (Physics.Raycast(transform.position + transform.up + 0.2f * transform.forward, (PlayerController.Player.transform.position - transform.position), out hit, VisualDetectionRadius))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        //Shoot At Player
                    }
                }
            }

        }
        if (CurrentEnemyState == EnemyState.Patrol)
        {
            if (Speed == 0f)
            {
                if (CurrentPatrolPoint == null)
                {
                    CurrentPatrolPoint = patrolPoint.GetNextPatrolPoint(ref StartingPoint, PatrolDir);
                    EnemyAgent.SetDestination(CurrentPatrolPoint.position);
                }
                if (SpeedChanging == false)
                {
                    SpeedChanging = true;
                    StartCoroutine(ChangeSpeed(1f, 1f));
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, CurrentPatrolPoint.position) < 5f)
                {
                    CurrentPatrolPoint = patrolPoint.GetNextPatrolPoint(ref StartingPoint, PatrolDir);
                    EnemyAgent.SetDestination(CurrentPatrolPoint.position);
                }
            }
        }
        if (CurrentEnemyState == EnemyState.Execution)
        {
            EnemyAgent.enabled = false;
        }

    }
    IEnumerator RotateTowardsTarget(Vector3 Target,float Duration)
    {
        Quaternion TargetRotation = Quaternion.LookRotation(Target-transform.position, transform.up);
        TargetRotation = new Quaternion(0, TargetRotation.y, 0, TargetRotation.w);
        float t = 0;
        while(t<Duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        Rotating = false;
    }
    IEnumerator ChangeSpeed(float TargetSpeed,float Duration)
    {
        float t = 0f;
        float CurrentSpeed = EnemyAnimator.GetFloat("Speed");
        while (t < Duration && SpeedChanging)
        {
            EnemyAnimator.SetFloat("Speed", CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, t / Duration));
            yield return null;
            t += Time.deltaTime;
        }
        Speed=TargetSpeed;
        SpeedChanging = false;
        if((int)Speed==0)
        {
            EnemyAgent.isStopped = true;
        }
        else
        {
            EnemyAgent.isStopped = false;
        }
    }
}
