using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public float AimSensitivity;

    private Animator playerAnimator;
    public AvatarMask PlayerAvatarMask;
    public GameObject WeaponIK;

    public Vector3 Offset;
    public Vector3 HeadOffset;
    public bool Aiming;

    Transform Chest;
    Transform Head;
    public GameObject AimCamera;
    float Turn;

    PlayerController playerController;
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        Chest = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
        Head = playerAnimator.GetBoneTransform(HumanBodyBones.Head);
        playerController = GetComponent<PlayerController>();

    }
    private void OnAnimatorIK()
    {
        if (CurrentWeapon != null && CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            CurrentWeapon.WeaponIK(Aiming);
        }
    }
    private void Update()
    {
        if((CrossPlatformInputManager.GetButtonDown("Aim")) && !playerController.InCover && CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            Aiming = !Aiming;
            AimCamera.SetActive(Aiming);
            if (Aiming == true)
            {
                Turn = transform.rotation.eulerAngles.y;
               // AimCamera.GetComponent<CinemachineVirtualCamera>().LookAt = CurrentWeapon.transform;
               // AimCamera.GetComponent<CinemachineVirtualCamera>().Follow = CurrentWeapon.transform;
            }
        }
        if (Aiming)
        {
            if (!playerController.InCover || playerController.DisableCoverMovement)
            {
                //if (!CameraLocker.instance.CameraLock)
                {
                    Turn += CrossPlatformInputManager.GetAxis("AimSide") * AimSensitivity * Time.deltaTime;
                    transform.localRotation = Quaternion.Euler(0, Turn, 0);
                }
            }
        }
    }
    void LateUpdate()
    {
        if (Aiming && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            Chest.LookAt(WeaponIK.transform);
            Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
            if(playerController.InCover)
            {
                
                Head.LookAt(WeaponIK.transform);
                Head.rotation = Head.rotation * Quaternion.Euler(HeadOffset);
            }
        }
    }
    public void CoverAim()
    {
        if (CurrentWeapon.gameObject.activeInHierarchy)
        {
            Aiming = !Aiming;
            AimCamera.SetActive(Aiming);
            if (Aiming == true)
            {
                Turn = transform.rotation.eulerAngles.y;
            }
        }
    }
    public void DisableCurrentWeapon()
    {
        Aiming = false;
        AimCamera.SetActive(false);
        CurrentWeapon.gameObject.SetActive(false);
    }
}
