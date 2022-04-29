using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public LayerMask ClimbLayer;
    
    Rigidbody PlayerRigidbody;
    Animator playerAnimator;

    GameObject Obstacle;
    float ObstacleHeight;
    RaycastHit hit;
    Vector3 ObstacleNormal;
    Vector3 FirstHitPoint;
    bool Abort;
    bool Climb;
    bool EnableClimbIK;
    float ClimbIKWeight;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        Climb = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C) && PlayerRigidbody.isKinematic==false)
        {
            ClimbObstacle();
        }
    }
    void ClimbObstacle()
    {
        Abort = false;
        Debug.DrawRay(transform.position + (transform.up * 1.8f), transform.forward, Color.red, 5f);
        Debug.DrawRay(transform.position + (transform.up * 2.2f), transform.forward, Color.red, 5f);
        /*for (float i = 0.1f; i <= 2.5f; i += 0.1f)
        {
            Debug.DrawRay(transform.position + (transform.up * i), transform.forward, Color.red, 5f);
            if (Physics.Raycast(transform.position + (transform.up * i), transform.forward, out hit, 5f, ClimbLayer))
            {
                if (i <= 0.1f)
                {
                    Obstacle = hit.transform.gameObject;
                    ObstacleNormal = hit.normal;
                    FirstHitPoint = hit.point;
                }
                else
                {
                    if (i == 2.5f)
                    {
                        Abort = true;
                        break;
                    }
                    if (hit.transform.gameObject != Obstacle)
                    {
                        Abort = true;
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Edge at i");
                if (i > 1f)
                {
                    ObstacleHeight = i;
                    break;
                }
            } 
        }
        Debug.Log(Abort);
        if (!Abort && ObstacleHeight >= 1f && Obstacle!=null)
        {
            Debug.Log(ObstacleHeight);
            StartCoroutine(RotateAndPositionPlayer(0.5f));
        }*/
    }
    IEnumerator RotateAndPositionPlayer(float Duration)
    {
        float t = 0;
        FirstHitPoint = new Vector3(FirstHitPoint.x, transform.position.y, FirstHitPoint.z);
        Vector3 ClimbPos = FirstHitPoint-(1f*(FirstHitPoint - transform.position).normalized);
        if (ObstacleHeight > 2.1f)
            ClimbPos += transform.up * (ObstacleHeight - 2.1f);
        Quaternion ClimbRot = Quaternion.LookRotation(-ObstacleNormal);
        ClimbRot = new Quaternion(0, ClimbRot.y, 0, ClimbRot.w);
        while (t<Duration)
        {
            transform.position = Vector3.Lerp(transform.position, ClimbPos, t/Duration);
            transform.rotation = Quaternion.Slerp(transform.rotation, ClimbRot, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        Climb = true;
        RightHandPos = FirstHitPoint + new Vector3(0, ObstacleHeight, 0) + 0.5f * transform.right;
        LeftHandPos = FirstHitPoint + new Vector3(0, ObstacleHeight, 0) - 0.1f * transform.right;
        StartCoroutine(ChangeIKWeight(1,0.5f));
        PlayerRigidbody.isKinematic = true;
        playerAnimator.SetTrigger("Climb");
        Obstacle = null;
    }
    Vector3 RightHandPos;
    Vector3 LeftHandPos;
    public void EnableRigidbody()
    {
        PlayerRigidbody.isKinematic = false;
        Climb = false;
        StartCoroutine(ChangeIKWeight(0, 0.25f));
    }
    void OnAnimatorIK()
    {
        if(ClimbIKWeight>0)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand,ClimbIKWeight); 
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, ClimbIKWeight);

            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand,RightHandPos);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandPos);
        }
    }
    IEnumerator ChangeIKWeight(float End,float Duration)
    {
        float t = 0;
        while(t<Duration)
        {
            ClimbIKWeight = Mathf.Lerp(ClimbIKWeight, End, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
}
