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
    public LayerMask CoverLayer;
    bool LockRotation;

    [HideInInspector] public bool InCover;

    private CapsuleCollider playerCollider;
    RaycastHit hit;
    
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        LockRotation = false;
        playerWeapon = GetComponent<PlayerWeapon>();
        playerCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Cover();
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
                if(!playerWeapon.Aiming && !InCover)
                    StartCoroutine(RotatePlayerTowardsTarget(transform.localRotation, Quaternion.LookRotation(PlayerCamera.transform.TransformDirection(CrossPlatformInputManager.GetAxis("Horizontal"), 0 , CrossPlatformInputManager.GetAxis("Vertical"))), 0.2f));
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
    void Cover()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!PlayerAnimator.GetBool("Cover"))
            {
                Debug.DrawRay(transform.position, transform.forward, Color.red);
                if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, CoverLayer))
                {
                    InCover = true;
                }
                else if (Physics.Raycast(transform.position, transform.right, out hit, 2f, CoverLayer))
                {
                    InCover = true;
                }
                else if (Physics.Raycast(transform.position, -transform.right, out hit, 2f, CoverLayer))
                {
                    InCover = true;
                }
                else if (Physics.Raycast(transform.position, -transform.forward, out hit, 2f, CoverLayer))
                {
                    InCover = true;
                }
                if (InCover == true)
                {
                    Vector3 CoverLocation = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    PlayerAnimator.SetBool("Cover", true);
                    StartCoroutine(MovePlayerToTarget(CoverLocation - (playerCollider.radius * (CoverLocation - transform.position).normalized), 1f));
                    StartCoroutine(RotatePlayerTowardsTarget(transform.localRotation,Quaternion.LookRotation(-hit.normal), 0.2f));
                }
            }
            else
            {
                PlayerAnimator.SetBool("Cover", false);
                InCover = false;
            }
        }
    }
    IEnumerator RotatePlayerTowardsTarget(Quaternion start,Quaternion end,float Duration)
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
    IEnumerator MovePlayerToTarget(Vector3 end,float Duration)
    {
        float t = 0f;
        while (t < Duration)
        {
            transform.localPosition = Vector3.Lerp(transform.position, end, t / Duration);
            yield return null;
            t += Time.deltaTime;
        }
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
