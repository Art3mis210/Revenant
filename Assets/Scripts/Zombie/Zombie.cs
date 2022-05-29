using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public ZombieState CurrentState;
    public float NoiseDetectionRadius;
    Vector3 NoiseLocation;
    Vector3 NoiseLookAtLocation;

    [SerializeField] LayerMask PlayerLayer;
    Animator EnemyAnimator;
    NavMeshAgent EnemyAgent;
    bool Rotating;
    bool SpeedChanging;
    float Speed;
    Vector3 WanderPosition;
    Vector3 EatingPoint;
    bool Eating;
    public enum ZombieState
    {
        Wander,Eat,Attack,Execution
    }
    private void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        EnemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentState != ZombieState.Attack && CurrentState != ZombieState.Execution)
        {
            if (NoiseManager.Noise.GetActiveNoise(NoiseDetectionRadius, transform.position))
            {
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
                    CurrentState = ZombieState.Attack;
                }
            }
        }
        if(CurrentState == ZombieState.Wander)
        {
            if (EatingPoint != Vector3.zero)
                EatingPoint = Vector3.zero;
            if (Speed<=0f)
            {
                if(!SpeedChanging)
                {
                    SpeedChanging = true;
                    StartCoroutine(ChangeSpeed(1, 1f));
                    EnemyAgent.SetDestination(WanderPosition=RandomNavSphere(transform.position, Random.Range(10, 30)));
                    
                }
            }
            if (Speed > 0f)
            {
                if(WanderPosition==Vector3.zero)
                {
                    EnemyAgent.SetDestination(WanderPosition = RandomNavSphere(transform.position, Random.Range(10, 30)));
                }
                if (Vector3.Distance(transform.position, WanderPosition) < 5f)
                {
                    EnemyAgent.SetDestination(WanderPosition = RandomNavSphere(transform.position, Random.Range(10, 20)));
                }
            }
        }
        else if (CurrentState == ZombieState.Execution)
        {
            EnemyAgent.speed = 0;
            EnemyAgent.enabled = false;
        }
        else if (CurrentState == ZombieState.Eat)
        {
            if (WanderPosition != Vector3.zero)
                WanderPosition = Vector3.zero;
            if (!Eating)
            {
                if (EatingPoint == Vector3.zero)
                {
                    WanderPosition = Vector3.zero;
                    EatingPoint = ZombieEatingPoints.Reference.GetZombieEatingPoint(transform.position);
                    Debug.Log(EatingPoint);
                    EnemyAgent.SetDestination(EatingPoint);
                    if (!SpeedChanging)
                    {
                        SpeedChanging = true;
                        StartCoroutine(ChangeSpeed(1f, 1f));
                    }
                }
                else if (Vector3.Distance(transform.position, EatingPoint) < 1f)
                {
                    if (Speed > 0)
                    {
                        if (!SpeedChanging)
                        {
                            SpeedChanging = true;
                            StartCoroutine(ChangeSpeed(0f, 1f));
                        }
                    }
                    else
                    {
                        Eating = true;
                        EnemyAnimator.SetFloat("EatPose", Random.Range(0, 2));
                        EnemyAnimator.SetBool("Eat", true);
                    }
                }
            }
        }
        else if(CurrentState == ZombieState.Attack)
        {
            EnemyAnimator.SetBool("Eat", false);
            EnemyAgent.SetDestination(PlayerController.Player.transform.position);
            if(Vector3.Distance(PlayerController.Player.transform.position,transform.position)<1f)
            {
                if (Speed > 0f)
                {
                    if (SpeedChanging == false)
                    {
                        SpeedChanging = true;
                        StartCoroutine(ChangeSpeed(0f, 1f));
                    }
                }
            }
            else
            {
                if(Speed<2f)
                {
                    if (SpeedChanging == false)
                    {
                        SpeedChanging = true;
                        StartCoroutine(ChangeSpeed(2f, 1f));
                    }
                }
            }
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
    IEnumerator RotateTowardsTarget(Vector3 Target, float Duration)
    {
        Quaternion TargetRotation = Quaternion.LookRotation(Target - transform.position, transform.up);
        TargetRotation = new Quaternion(0, TargetRotation.y, 0, TargetRotation.w);
        float t = 0;
        while (t < Duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        Rotating = false;
    }
    IEnumerator ChangeSpeed(float TargetSpeed, float Duration)
    {
        float t = 0f;
        float CurrentSpeed = EnemyAnimator.GetFloat("Speed");
        while (t < Duration && SpeedChanging)
        {
            EnemyAnimator.SetFloat("Speed", CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, t / Duration));
            yield return null;
            t += Time.deltaTime;
        }
        Speed = TargetSpeed;
        SpeedChanging = false;
        if ((int)Speed == 0)
        {
            EnemyAgent.isStopped = true;
            EnemyAgent.speed = 0f;
        }
        else
        {
            EnemyAgent.isStopped = false;
            EnemyAgent.speed = 0.1f;
        }
    }
    RaycastHit hit;
    public void HitPlayer(float Damage)
    {
        Debug.DrawRay(transform.position + transform.up, transform.forward,Color.black,1f);
        if(Physics.Raycast(transform.position+transform.up,transform.forward,out hit,2f,PlayerLayer))
        {
            hit.transform.gameObject.GetComponent<BulletHit>().HitDetection(hit.point, transform.forward,Damage);
        }
    }
    public void BitePlayer()
    {
        PlayerController.Player.GetComponent<BulletHit>().HitDetection(transform.position, transform.forward, 50f);
    }
}
