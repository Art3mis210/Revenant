using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    private Animator PlayerAnimator;
    private bool MovementSpeedChanging;
    private float Horizontal;
    private float Vertical;
    private float MouseX;
    private float MouseY;
    private PlayerWeapon playerWeapon;

    public GameObject PlayerCamera;
    bool LockRotation;
    
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        LockRotation = false;
        playerWeapon = GetComponent<PlayerWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        if (CrossPlatformInputManager.GetAxis("Vertical")!=0 || CrossPlatformInputManager.GetAxis("Horizontal")!=0)
        {
            if(!LockRotation && (Vertical!= CrossPlatformInputManager.GetAxis("Vertical") || Horizontal!= CrossPlatformInputManager.GetAxis("Horizontal") || MouseX!= CrossPlatformInputManager.GetAxis("Mouse X") || MouseY != CrossPlatformInputManager.GetAxis("Mouse Y")))
            {
                LockRotation = true;
                Vertical = CrossPlatformInputManager.GetAxis("Vertical");
                Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
                MouseX = CrossPlatformInputManager.GetAxis("Mouse X");
                MouseY = CrossPlatformInputManager.GetAxis("Mouse Y");
                if(!playerWeapon.Aiming)
                    StartCoroutine(RotatePlayerTowardsCamera(transform.localRotation, Quaternion.LookRotation(PlayerCamera.transform.TransformDirection(CrossPlatformInputManager.GetAxis("Horizontal"), 0 , CrossPlatformInputManager.GetAxis("Vertical"))), 0.2f));
                if (PlayerAnimator.GetFloat("Speed") < 1f && !MovementSpeedChanging)
                {
                    MovementSpeedChanging = true;
                    StartCoroutine(ChangeMovementSpeed(1, 0.75f));
                }
            }
        }
        else
        {
            if (!MovementSpeedChanging && (int)PlayerAnimator.GetFloat("Speed") != 0)
            {
                MovementSpeedChanging = true;
                StartCoroutine(ChangeMovementSpeed(0, 0.75f));
            }
            LockRotation = false;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            PlayerAnimator.SetBool("Sprint", true);
            if (!MovementSpeedChanging && (int)PlayerAnimator.GetFloat("Speed") != 2)
            {
                MovementSpeedChanging = true;
                StartCoroutine(ChangeMovementSpeed(2, 1f));
            }
        }    
        else
        {
            PlayerAnimator.SetBool("Sprint", false);
            if (!MovementSpeedChanging && (int)PlayerAnimator.GetFloat("Speed") == 2)
            {
                MovementSpeedChanging = true;
                StartCoroutine(ChangeMovementSpeed(1, 1f));
            }
        }

    }
    IEnumerator RotatePlayerTowardsCamera(Quaternion start,Quaternion end,float Duration)
    {
        float t = 0f;
        end = new Quaternion(0, end.y, 0, end.w);
        while (t < Duration)
        {
            transform.localRotation = Quaternion.Slerp(start,end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
        LockRotation = false;
    }
    IEnumerator ChangeMovementSpeed(float RequiredSpeed,float Duration)
    {
        float t = 0f;
        float CurrentSpeed;
        while(t<Duration)
        {
            CurrentSpeed = PlayerAnimator.GetFloat("Speed");
            PlayerAnimator.SetFloat("Speed", Mathf.Lerp(CurrentSpeed, RequiredSpeed, t / Duration));
            yield return null;
            t += Time.deltaTime;
        }
        MovementSpeedChanging = false;

    }
}
