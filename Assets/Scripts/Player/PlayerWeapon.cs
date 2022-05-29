using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

public class PlayerWeapon : MonoBehaviour
{
    #region VariablesAndReferences
    public Transform WeaponParent;
    public Weapon CurrentWeapon;
    public float AimSensitivity;

    private Animator playerAnimator;
    public GameObject WeaponIK;

    public Vector3 Offset;
    public Vector3 HeadOffset;
    public bool Aiming;
    bool StealthPos;

    Transform Chest;
    Transform Head;
    public GameObject AimCamera;
    float Turn;
    float Recoil;

    public List<RectTransform> ReticlesList;
    public RectTransform Reticle;
    public RectTransform CanvasTransform;


    #endregion
    #region Delegates
    delegate void Weapons();
    Weapons WeaponsUpdate;
    #endregion
    public static PlayerWeapon playerWeapon
    {
        get;
        set;
    }
    PlayerController playerController;
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        Chest = playerAnimator.GetBoneTransform(HumanBodyBones.Spine);
        Head = playerAnimator.GetBoneTransform(HumanBodyBones.Head);
        playerController = GetComponent<PlayerController>();
        playerWeapon = this;
        if (playerController.CurrentInput == PlayerController.InputType.Mobile)
        {
            WeaponsUpdate = WeaponsUpdateGamepad;
        }
        else
        {
            WeaponsUpdate = WeaponsUpdateKeyboard;
            AimSensitivity = 100;
        }

    }
    private void OnAnimatorIK()
    {
        if (CurrentWeapon != null && CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            CurrentWeapon.WeaponIK(Aiming);
        }
    }
    void Update()
    {
        WeaponsUpdate();
    }
    #region Gamepad
    void WeaponsUpdateGamepad()
    {
        if (CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            if (playerController.Stealth && !StealthPos)
            {
                StealthPos = true;
                CurrentWeapon.EnableStealthPos(StealthPos);
            }
            else if (!playerController.Stealth && StealthPos)
            {
                StealthPos = false;
                CurrentWeapon.EnableStealthPos(StealthPos);
            }
        }
        if ((CrossPlatformInputManager.GetButtonDown("Aim")) && !playerController.InCover && CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            Aiming = !Aiming;
            AimCamera.SetActive(Aiming);
            if (Reticle.gameObject.activeInHierarchy)
                Reticle.gameObject.SetActive(false);
            Reticle = ReticlesList[CurrentWeapon.ReticleType];
            Reticle.gameObject.SetActive(Aiming);
            if (Aiming == true)
            {
                Turn = transform.rotation.eulerAngles.y;
                playerAnimator.SetFloat("Aiming", 1f);
            }
            else
                playerAnimator.SetFloat("Aiming", 0);
        }
        if (Aiming)
        {
            if (!playerController.InCover || playerController.DisableCoverMovement)
            {
                Turn += CrossPlatformInputManager.GetAxis("AimSide") * AimSensitivity * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(0, Turn, 0);
            }
            if (CrossPlatformInputManager.GetButton("Shoot"))
            {
                Recoil = CurrentWeapon.Shoot();
                WeaponIK.transform.position = new Vector3(WeaponIK.transform.position.x, WeaponIK.transform.position.y + Recoil, WeaponIK.transform.position.z);
            }
        }
    }
    #endregion
    #region Keyboard
    void WeaponsUpdateKeyboard()
    {
        if (CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            if (playerController.Stealth && !StealthPos)
            {
                StealthPos = true;
                CurrentWeapon.EnableStealthPos(StealthPos);
            }
            else if (!playerController.Stealth && StealthPos)
            {
                StealthPos = false;
                CurrentWeapon.EnableStealthPos(StealthPos);
            }
        }
        if (Input.GetMouseButtonDown(1) && !playerController.InCover && CurrentWeapon.transform.gameObject.activeInHierarchy)
        {
            Aiming = !Aiming;
            if (Reticle.gameObject.activeInHierarchy)
                Reticle.gameObject.SetActive(false);
            Reticle = ReticlesList[CurrentWeapon.ReticleType];
            Reticle.gameObject.SetActive(Aiming);
            AimCamera.SetActive(Aiming);
            if (Aiming == true)
            {
                Turn = transform.rotation.eulerAngles.y;
                playerAnimator.SetFloat("Aiming", 1f);
            }
            else
                playerAnimator.SetFloat("Aiming", 0);
        }
        if (Aiming)
        {
            if (!playerController.InCover || playerController.DisableCoverMovement)
            {
                Turn += CrossPlatformInputManager.GetAxis("Mouse X") * AimSensitivity * Time.deltaTime;
                transform.localRotation = Quaternion.Euler(0, Turn, 0);
            }
            if (Input.GetMouseButton(0))
            {
                Recoil = CurrentWeapon.Shoot();
                WeaponIK.transform.position = new Vector3(WeaponIK.transform.position.x, WeaponIK.transform.position.y + Recoil, WeaponIK.transform.position.z);
            }
        }
    }
    #endregion
    void LateUpdate()
    {
        if (Aiming && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
        {
            Chest.LookAt(WeaponIK.transform);
            Chest.rotation = Chest.rotation * Quaternion.Euler(Offset);
            if (playerController.InCover)
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
            if(Reticle.gameObject.activeInHierarchy)
                Reticle.gameObject.SetActive(false);
            Reticle = ReticlesList[CurrentWeapon.ReticleType];
            Reticle.gameObject.SetActive(Aiming);
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
        if (Reticle.gameObject.activeInHierarchy)
            Reticle.gameObject.SetActive(false);
        Reticle = ReticlesList[CurrentWeapon.ReticleType];
        Reticle.gameObject.SetActive(Aiming);
        if (CurrentWeapon != null)
            CurrentWeapon.gameObject.SetActive(false);
        playerAnimator.SetFloat("Aiming", 0);
    }
    public void DisableAim()
    {
        Aiming = false;
        AimCamera.SetActive(false);
        if (Reticle.gameObject.activeInHierarchy)
            Reticle.gameObject.SetActive(false);
        Reticle = ReticlesList[CurrentWeapon.ReticleType];
        Reticle.gameObject.SetActive(Aiming);
        playerAnimator.SetFloat("Aiming", 0);
    }
    public void MoveReticle(Vector3 MovePos)
    {
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(MovePos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasTransform.sizeDelta.x) - (CanvasTransform.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasTransform.sizeDelta.y) - (CanvasTransform.sizeDelta.y * 0.5f)));
        Reticle.anchoredPosition = WorldObject_ScreenPosition;
    }
    public void ThrowWeapon()
    {
        if(CurrentWeapon!=null)
        {
            if(CurrentWeapon.gameObject.activeInHierarchy)
            {
                Aiming = false;
                AimCamera.SetActive(false);
                if (Reticle.gameObject.activeInHierarchy)
                    Reticle.gameObject.SetActive(false);
                CurrentWeapon.gameObject.transform.parent = null;
                CurrentWeapon.gameObject.AddComponent<BoxCollider>().size=CurrentWeapon.transform.lossyScale;
                CurrentWeapon.gameObject.AddComponent<Rigidbody>();
                CurrentWeapon.enabled = false;
            }
        }
    }
}
