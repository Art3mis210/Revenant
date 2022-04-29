using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform LeftHand;
    public Transform RightHand;
    public Vector3 AimPos;
    public Vector3 AimRot;
    public Vector3 NonAimPos;
    public Vector3 NonAimRot;
    bool AimPosEnabled;
    bool Aiming;
    private void OnEnable()
    {
        if (playerAnimator == null)
            playerAnimator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        if (Aiming && !AimPosEnabled)
        {
            AimPosEnabled = true;
            StartCoroutine(ChangePos(AimPos, 1f));
            StartCoroutine(ChangeRot(AimRot, 1f));
        }
        else if (!Aiming && AimPosEnabled)
        {
            AimPosEnabled = false;
            StartCoroutine(ChangePos(NonAimPos, 1f));
            StartCoroutine(ChangeRot(NonAimRot, 1f));
        }
    }
    public void WeaponIK(bool Aiming)
    {
        if(playerAnimator)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand.rotation);

            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, RightHand.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, RightHand.rotation);
            if (this.Aiming != Aiming)
                this.Aiming = Aiming;
        }
    }

    IEnumerator ChangePos(Vector3 EndPos,float Duration)
    {
        float t = 0;
        while(t<Duration)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, EndPos, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
    IEnumerator ChangeRot(Vector3 EndRot,float Duration)
    {
        float t = 0;
        while (t < Duration)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(EndRot), t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
    }
    /*IEnumerator ChangeHandWeight(float targetfloat Duration)
    {
        float t = 0;
        while(t<Duration)
        {

            yield return null;
        }
    }*/
}
