using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public float AimSensitivity;
    private Animator playerAnimator;
    public GameObject WeaponIK;
    public Vector3 Offset;
    public bool Aiming;
    Transform Chest;
    public GameObject AimCamera;
    float Turn;
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        Chest = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
    }
    private void OnAnimatorIK()
    {
        if(CurrentWeapon!=null && CurrentWeapon.transform.gameObject.activeInHierarchy)
            CurrentWeapon.WeaponIK(Aiming);
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Aiming = !Aiming;
            AimCamera.SetActive(Aiming);
            if (Aiming == true)
            {
               // AimCamera.GetComponent<CinemachineVirtualCamera>().LookAt = CurrentWeapon.transform;
               // AimCamera.GetComponent<CinemachineVirtualCamera>().Follow = CurrentWeapon.transform;
            }
        }
        if (Aiming)
        {
            Turn += CrossPlatformInputManager.GetAxis("Mouse X") * AimSensitivity;
            transform.localRotation = Quaternion.Euler(0, Turn, 0);
        }
    }
    void LateUpdate()
    {
        if (Aiming && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            Chest.LookAt(WeaponIK.transform);
            Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
        }
    }
}
