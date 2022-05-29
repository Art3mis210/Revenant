using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
#region References
    public float BulletSpeed;
    public int LoadedBullets;
    public int MagLength;
    public int TotalBullets;
    public float Recoil;
    public float TimeToShoot;
    bool Shooting;
    bool ShootEnabled;
    public Transform Muzzle;
    private Animator playerAnimator;
    public Transform LeftHand;
    public Transform RightHand;
    public Vector3 AimPos;
    public Vector3 AimRot;
    public Vector3 NonAimStandPos;
    public Vector3 NonAimStandRot;
    public Vector3 NonAimPos;
    public Vector3 NonAimRot;
    public Vector3 NonAimStealthPos;
    public Vector3 NonAimStealthRot;
    public int ReticleType;
    bool AimPosEnabled;
    bool Aiming;
    bool StealthPos;
    RaycastHit hit;
    #endregion
    private void OnEnable()
    {
        if (playerAnimator == null)
            playerAnimator = GetComponentInParent<Animator>();
        if(Aiming)
        {
            transform.localPosition = AimPos;
            transform.localRotation = Quaternion.Euler(AimRot);
        }
        else if(PlayerController.Player!=null)
        {
            if(PlayerController.Player.Stealth)
                EnableStealthPos(true);
        }
        else
        {
            transform.localPosition = NonAimStandPos;
            transform.localRotation = Quaternion.Euler(NonAimStandRot);
        }
    }
    private void Update()
    {
        if (Aiming)
        {
            Debug.DrawRay(Muzzle.position, Muzzle.forward, Color.red, 0.1f);    
            if (Physics.Raycast(Muzzle.position, Muzzle.forward, out hit))
            {
                PlayerWeapon.playerWeapon.MoveReticle(hit.point);
            }
        }
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
    public float Shoot()
    {
        if(LoadedBullets!=0 && ShootEnabled)
        {
            if(!Shooting)
            {
                Shooting = true;
                StartCoroutine(ShootBullets());
                return Recoil;
            }
        }
        return 0;
    }
    IEnumerator ShootBullets()
    {
        Bullet BulletToFire=BulletPool.Reference.GetBulletFromPool();
        if (BulletToFire != null)
        {
            BulletToFire.transform.position = Muzzle.transform.position;
            BulletToFire.transform.rotation = Muzzle.transform.rotation;
            BulletToFire.gameObject.SetActive(true);

            BulletToFire.GetComponent<Rigidbody>().AddForce(BulletSpeed * Muzzle.transform.forward);
            LoadedBullets--;
        }
        yield return new WaitForSeconds(TimeToShoot);
        Shooting = false;
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
        ShootEnabled = Aiming;

    }
    public void EnableStealthPos(bool Status)
    {
        if (Status)
        {
            NonAimPos = NonAimStealthPos;
            NonAimRot = NonAimStealthRot;
            if(!Aiming)
            {
                StartCoroutine(ChangePos(NonAimPos, 1f));
                StartCoroutine(ChangeRot(NonAimRot, 1f));
            }
        }
        else
        {
            NonAimPos = NonAimStandPos;
            NonAimRot = NonAimStandRot;
            if (!Aiming)
            {
                StartCoroutine(ChangePos(NonAimPos, 1f));
                StartCoroutine(ChangeRot(NonAimRot, 1f));
            }
        }

    }
    
}
