using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public enum InputType
    {
        Mobile,Keyboard
    }
    public InputType CurrentInput;
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
    [HideInInspector] public bool Stealth;
    #endregion

    #region OtherComponents
    private PlayerWeapon playerWeapon;
    public GameObject PlayerCamera;
    bool LockRotation;
    private CapsuleCollider playerCollider;
    RaycastHit hit;
    #endregion

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

    #region Delegates
    delegate void MovementReference();
    MovementReference Movement;

    delegate void CoverReference();
    MovementReference Cover;

    delegate void CoverMoveReference();
    MovementReference CoverMove;
    #endregion

    private void Awake()
    {
        Player = this;
    }
    void Start()
    {
        if(CurrentInput==InputType.Mobile)
        {
            Movement = MovementGamepad;
            Cover = CoverGamepad;
            CoverMove = CoverMoveGamepad;
        }
        else
        {
            Movement = MovementKeyboard;
            Cover = CoverKeyboard;
            CoverMove = CoverMoveKeyboard;
        }
        PlayerAnimator = GetComponent<Animator>();
        LockRotation = false;
        playerWeapon = GetComponent<PlayerWeapon>();
        playerCollider = GetComponent<CapsuleCollider>();
        EnableMovement = true;
        ;
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
    #region Gamepad
    void MovementGamepad()
    {
        if (CrossPlatformInputManager.GetAxis("Vertical")!=0f || CrossPlatformInputManager.GetAxis("Horizontal")!=0f)
        {
            if(!LockRotation)
            {
                LockRotation = true;
                Vertical = CrossPlatformInputManager.GetAxis("Vertical");
                Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
                if(!playerWeapon.Aiming)
                    StartCoroutine(RotatePlayerTowardsTarget(transform.localRotation, Quaternion.LookRotation(PlayerCamera.transform.TransformDirection(CrossPlatformInputManager.GetAxis("Horizontal"), 0 , CrossPlatformInputManager.GetAxis("Vertical"))), 0.2f));
                if (!Stealth)
                    NoiseManager.Noise.CreateNoise(transform.position, 0.25f);
                if (PlayerAnimator.GetFloat("Speed") < 1f && !MovementSpeedChanging)
                {
                    PlayerAnimator.SetFloat("Speed", 0f);
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
                StartCoroutine(ChangeMovementSpeed(0f, 0.75f));
            }
            LockRotation = false;
        }
        if(Input.GetKey(KeyCode.LeftShift) || CrossPlatformInputManager.GetButton("Sprint"))
        {
            if (PlayerSpeed > 0)
            {
                PlayerAnimator.SetBool("Sprint", true);
                if(!Stealth)
                    NoiseManager.Noise.CreateNoise(transform.position,1f);
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
        if(playerWeapon.Aiming)
        {
            PlayerAnimator.SetFloat("Vertical", CrossPlatformInputManager.GetAxis("Vertical"));
            PlayerAnimator.SetFloat("Horizontal", CrossPlatformInputManager.GetAxis("Horizontal"));
            PlayerAnimator.SetFloat("MoveDir", -Mathf.Abs(CrossPlatformInputManager.GetAxis("Vertical")) + Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal")));
        }

        if(!InCover)
        {
            if (CrossPlatformInputManager.GetButtonDown("Stance"))
            {
                Stealth = !PlayerAnimator.GetBool("Stealth");
                PlayerAnimator.SetBool("Stealth", Stealth);
                
            }

        }

    }
    void CoverGamepad()
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
    void CoverMoveGamepad()
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
    #endregion
    #region Keyboard
    void MovementKeyboard()
    {
        if (CrossPlatformInputManager.GetAxis("Vertical") != 0f || CrossPlatformInputManager.GetAxis("Horizontal") != 0f)
        {
            if (!LockRotation)
            {
                LockRotation = true;
                Vertical = CrossPlatformInputManager.GetAxis("Vertical");
                Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
                if (!playerWeapon.Aiming)
                    StartCoroutine(RotatePlayerTowardsTarget(transform.localRotation, Quaternion.LookRotation(PlayerCamera.transform.TransformDirection(CrossPlatformInputManager.GetAxis("Horizontal"), 0, CrossPlatformInputManager.GetAxis("Vertical"))), 0.2f));
                if (PlayerAnimator.GetFloat("Speed") < 1f && !MovementSpeedChanging)
                {
                    PlayerAnimator.SetFloat("Speed", 0f);
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
                StartCoroutine(ChangeMovementSpeed(0f, 0.75f));
            }
            LockRotation = false;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (PlayerSpeed > 0)
            {
                PlayerAnimator.SetBool("Sprint", true);
                if (!Stealth)
                    NoiseManager.Noise.CreateNoise(transform.position,2f);
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
        if (playerWeapon.Aiming)
        {
            PlayerAnimator.SetFloat("Vertical", CrossPlatformInputManager.GetAxis("Vertical"));
            PlayerAnimator.SetFloat("Horizontal", CrossPlatformInputManager.GetAxis("Horizontal"));
            PlayerAnimator.SetFloat("MoveDir", -Mathf.Abs(CrossPlatformInputManager.GetAxis("Vertical")) + Mathf.Abs(CrossPlatformInputManager.GetAxis("Horizontal")));
        }

        if (!InCover)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Stealth = !PlayerAnimator.GetBool("Stealth");
                PlayerAnimator.SetBool("Stealth", Stealth);

            }

        }

    }
    void CoverKeyboard()
    {
        if (!playerWeapon.Aiming)
        {
            if (Input.GetKeyDown(KeyCode.Q))
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
    void CoverMoveKeyboard()
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
        if (DisableCoverMovement)
        {
            if (Input.GetMouseButtonDown(1) && !CoverAimLock)
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
    #endregion

    #region ControlIndependentFunctions
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
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, RequiredSpeed, t / Duration);
            PlayerAnimator.SetFloat("Speed",CurrentSpeed);
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
    #endregion
}
