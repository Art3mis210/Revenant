using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    Animator PlayerAnimator;
    public int FrontBulletHit;
    public bool EnableBulletHit;
    public float Health;
    Rigidbody PlayerRigidbody;
    Collider PlayerCollider;
    Rigidbody[] rigidbodies;
    Collider[] colliders;
    [SerializeField] ParticleSystem ShoulderHit;
    public GameObject BulletHitDetection;
    HitMeter[] hitMeter;
    private void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        PlayerCollider = GetComponent<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        PlayerAnimator = GetComponent<Animator>();
        EnableBulletHit = true;
        EnableRagdoll(false);
        hitMeter = BulletHitDetection.GetComponentsInChildren<HitMeter>();

    }
    public void EnableRagdoll(bool Status)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            if (rb != PlayerRigidbody)
            rb.isKinematic = !Status;
        }
        foreach (Collider col in colliders)
        {
            if (col != PlayerCollider)
            {
                if (col.gameObject.layer == 7)
                {
                    col.enabled = !Status;
                }
                else
                    col.enabled = Status;
            }
        }
        PlayerRigidbody.isKinematic = Status;
        PlayerCollider.enabled = !Status;
        PlayerAnimator.enabled = !Status;;
        if (Status)
        {
            PlayerWeapon.playerWeapon.ThrowWeapon();
        }
    }
    public void BulletHitReaction(Vector3 hitPoint,Vector3 hitForward)
    {
        Health--;
        
        if (EnableBulletHit)
        {
            if (Health > 0)
            {
                if (!PlayerController.Player.InCover && PlayerController.Player.EnableMovement == true)
                {
                    if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, transform.forward)) <= 45)
                    {
                        Debug.Log("Front");
                        hitMeter[0].HitDetection();
                        FrontBulletHit++;
                        if (FrontBulletHit > 10)
                        {
                            FrontBulletHit = 0;
                            PlayerAnimator.SetTrigger("BulletHit");
                            ShoulderHit.gameObject.SetActive(true);
                            EnableBulletHit = false;
                        }
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, transform.forward)) <=90 && Mathf.Abs(Vector3.Angle(hitPoint - hitForward, transform.right)) <= 90)
                    {
                        Debug.Log("Front Right");
                        hitMeter[1].HitDetection();
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, transform.right)) <= 45)
                    {
                        Debug.Log("Right");
                        hitMeter[2].HitDetection();
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, -transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(hitPoint - hitForward, transform.right)) <= 90)
                    {
                        Debug.Log("back Right");
                        hitMeter[3].HitDetection();
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, -transform.forward)) <= 45)
                    {
                        Debug.Log("back");
                        hitMeter[4].HitDetection();
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, -transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(hitPoint - hitForward, -transform.right)) <= 90)
                    {
                        Debug.Log("back left");
                        hitMeter[5].HitDetection();
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, -transform.right)) <= 45)
                    {
                        Debug.Log("left");
                        hitMeter[6].HitDetection();
                    }
                    else if (Mathf.Abs(Vector3.Angle(hitPoint - hitForward, transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(hitPoint - hitForward, -transform.right)) <= 90)
                    {
                        Debug.Log("forward back");
                        hitMeter[7].HitDetection();
                    }


                }
            }
            else
            {
                EnableRagdoll(true);
            }
        }
    }
    private void Update()
    {
        BulletHitDetection.transform.rotation = Quaternion.Euler(BulletHitDetection.transform.rotation.eulerAngles.x, BulletHitDetection.transform.rotation.eulerAngles.y, PlayerController.Player.PlayerCamera.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y);
    }
}
