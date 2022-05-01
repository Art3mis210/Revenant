using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player
    {
        get;
        private set;
    }
    #region MovementVariables
    private Animator PlayerAnimator;
    private bool MovementSpeedChanging;
    private float Horizontal;
    private float Vertical;
    private float MouseX;
    private float MouseY;
    #endregion

    private PlayerWeapon playerWeapon;
    public GameObject PlayerCamera;
    bool LockRotation;

    #region CoverVariables
    public LayerMask CoverLayer;
    bool ChangingCoverDir;
    float CoverDir;
    Vector3 CoverPos;
    Quaternion CoverRot;
    bool CoverMovement;
    bool CoverAimLock;
    float PlayerSpeed;
    [HideInInspector] public bool DisableCoverMovement;
    [HideInInspector] public bool InCover;
    public bool EnableMovement;
    #endregion

    private CapsuleCollider playerCollider;
    RaycastHit hit;
    
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        LockRotation = false;
        playerWeapon = GetComponent<PlayerWeapon>();
        playerCollider = GetComponent<CapsuleCollider>();
        EnableMovement = true;
        Player = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(EnableMovement)
        {   if (!InCover)
                Movement();
            else
                CoverMove();
            Cover();
        }
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
                    StartCoroutine(RotatePlayerTowardsTarget(transform.localRotation, Quaternion.LookRotation(PlayerCamera.transform.TransformDirection(CrossPlatformInputManager.GetAxis("Horizontal"), 0 , CrossPlatformInputManager.GetAxis("Vertical"))), 0.2f));
                else
                {
                    PlayerAnimator.SetFloat("Vertical", Vertical);
                    PlayerAnimator.SetFloat("Horizontal", Horizontal);
                    PlayerAnimator.SetFloat("MoveDir", Vertical + Horizontal);
                }
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
        if(Input.GetKey(KeyCode.LeftShift) || CrossPlatformInputManager.GetButton("Sprint"))
        {
            if (PlayerSpeed > 0)
            {
                PlayerAnimator.SetBool("Sprint", true);
                NoiseManager.Noise.CreateNoise(transform.position);
                if (!MovementSpeedChanging && (int)PlayerAnimator.GetFloat("Speed") != 2)
                {
                    MovementSpeedChanging = true;
                    StartCoroutine(ChangeMovementSpeed(2, 1f));
                }
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
        if (!playerWeapon.Aiming)
        {
            if (Input.GetKeyDown(KeyCode.Q) || CrossPlatformInputManager.GetButtonDown("Cover"))
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
                        //PlayerAnimator.applyRootMotion = false;
                        StartCoroutine(MovePlayerToTarget(CoverLocation - (playerCollider.radius * (CoverLocation - transform.position).normalized), 1f));
                        StartCoroutine(RotatePlayerTowardsTarget(transform.localRotation, Quaternion.LookRotation(-hit.normal), 0.2f));
                        CoverMovement = true;
                    }
                }
                else
                {
                    //PlayerAnimator.applyRootMotion = true;
                    PlayerAnimator.SetBool("Cover", false);
                    PlayerAnimator.SetBool("CoverAim", false);
                    InCover = false;

                }
            }
        }
    }
    void CoverMove()
    {
        if (CoverMovement)
        {
            Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            if (Horizontal > 0)
            {
                if (CoverDir <= 0 && !ChangingCoverDir)
                {
                    ChangingCoverDir = true;
                    StartCoroutine(ChangeCoverDir(1, 1f));
                }
                Debug.DrawRay(transform.position + 0.25f * transform.right, transform.forward, Color.red, 1f);
                if (Physics.Raycast(transform.position + 0.25f * transform.right, transform.forward, out hit, 2f, CoverLayer))
                {
                    if (DisableCoverMovement)
                        DisableCoverMovement = false;
                    PlayerAnimator.SetFloat("CoverPos", Horizontal);
                }
                else
                {
                    if (!DisableCoverMovement)
                    {
                        DisableCoverMovement = true;
                        StartCoroutine(StopCoverMovement(1f));
                    }
                }
            }
            else if (Horizontal < 0)
            {
                if (CoverDir >= 0 && !ChangingCoverDir)
                {
                    ChangingCoverDir = true;
                    StartCoroutine(ChangeCoverDir(-1, 1f));
                }
                Debug.DrawRay(transform.position - 0.25f * transform.right, transform.forward, Color.red, 1f);
                if (Physics.Raycast(transform.position - 0.25f * transform.right, transform.forward, out hit, 2f, CoverLayer))
                {
                    if (DisableCoverMovement)
                        DisableCoverMovement = false;
                    PlayerAnimator.SetFloat("CoverPos", Horizontal);
                }
                else
                {
                    if (!DisableCoverMovement)
                    {
                        DisableCoverMovement = true;
                        StartCoroutine(StopCoverMovement(1f));
                    }
                }
            }
            else
            {
                PlayerAnimator.SetFloat("CoverPos", Horizontal);
            }
            PlayerAnimator.SetFloat("CoverDir", CoverDir);
        }
        if(DisableCoverMovement)
        {
            
            if(CrossPlatformInputManager.GetButtonDown("Aim") && !CoverAimLock)
            {
                if (playerWeapon.CurrentWeapon.gameObject.activeInHierarchy)
                {
                    CoverAimLock = true;
                    PlayerAnimator.SetBool("CoverAim", !PlayerAnimator.GetBool("CoverAim"));
                    if (PlayerAnimator.GetBool("CoverAim"))
                    {
                        CoverPos = transform.position;
                        CoverRot = transform.rotation;
                        CoverMovement = false;
                    }
                    else
                    {
                        Invoke("ReturnToCover", 0.4f);
                    }
                }
            }
        }

    }
    void ReturnToCover()
    {
        StartCoroutine(RotatePlayerTowardsTarget(transform.rotation, CoverRot, 1f));
        StartCoroutine(MovePlayerToTarget(CoverPos, 1f));
        CoverMovement = true;
    }
    public void DisableCoverAimLock()
    {
        CoverAimLock = false;
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
        float CurrentSpeed=PlayerAnimator.GetFloat("Speed");
        while(t<Duration)
        {
            //CurrentSpeed = ;
            PlayerAnimator.SetFloat("Speed",CurrentSpeed=Mathf.Lerp(CurrentSpeed, RequiredSpeed, t / Duration));
            yield return null;
            t += Time.deltaTime;
        }
        PlayerSpeed = RequiredSpeed;
        MovementSpeedChanging = false;
    }
    public void ChangeMovement(int Status)
    {
        if (Status == 0)
            EnableMovement = false;
        else
            EnableMovement = true;
    }
    IEnumerator ChangeCoverDir(float NewDir,float Duration)
    {
        float t = 0f;
        while (t < Duration)
        {
            CoverDir=Mathf.Lerp(CoverDir,NewDir,t/Duration);
            yield return null;
            t += Time.deltaTime;
        }
        ChangingCoverDir = false;
    }
    IEnumerator StopCoverMovement(float Duration)
    {
        float t = 0f;
        float CoverPos = PlayerAnimator.GetFloat("CoverPos");
        while (t < Duration)
        {
            PlayerAnimator.SetFloat("CoverPos", CoverPos = Mathf.Lerp(CoverPos, 0, t / Duration));
            yield return null;
            t += Time.deltaTime;
        }
    }
}
