using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public EnemyExecution.EnemyType CurrentEnemyType;
    #region References
    private Rigidbody MainRigidbody;
    private Collider MainCollider;
    public Collider ExecutionCollider;
    [SerializeField] private Rigidbody[] Rigidbodies;
    private Collider[] Colliders;
    private Animator animator;
    [SerializeField] private RuntimeAnimatorController GetUpController;
    [SerializeField] private RuntimeAnimatorController CrawlController;
    public GameObject Root;
    public Transform RootParent;
    Vector3[] RagdollBonesPos;
    public HumanBodyBones bones;
    public LayerMask GroundLayer;
    public float Health;
    public bool DisableRagdollMode;
    Enemy enemyController;
    Zombie ZombieController;

    #endregion

    void Start()
    {
        MainRigidbody = transform.GetComponent<Rigidbody>();
        MainCollider = transform.GetComponent<Collider>();
        Rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        Colliders = transform.GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();
        EnableRagdoll(false);
        RagdollBonesPos = new Vector3[12];
        RootParent = Root.transform.parent;
        if (CurrentEnemyType == EnemyExecution.EnemyType.Human)
            enemyController = GetComponent<Enemy>();
        else
            ZombieController = GetComponent<Zombie>();
    }

    public void EnableRagdoll(bool Status)
    {
        if (!DisableRagdollMode)
        {
            foreach (Rigidbody rb in Rigidbodies)
            {
                if (rb != MainRigidbody)
                    rb.isKinematic = !Status;
            }
            foreach (Collider col in Colliders)
            {
                if (col != MainCollider)
                {
                    if (col.gameObject.layer == 7)
                    {
                        col.enabled = !Status;
                    }
                    else
                        col.enabled = Status;
                }

            }
            MainRigidbody.isKinematic = Status;
            MainCollider.enabled = !Status;
            animator.enabled = !Status;
            if(ExecutionCollider!=null)
                ExecutionCollider.enabled = !Status;
            if (Status)
            {
                if(enemyController!=null)
                    enemyController.AimIK = false;
                Root.transform.parent = null;

                Invoke("TurnOff", 5f);

            }
        }

    }
    void TurnOff()
    {
        Root.transform.parent = RootParent;
        if (Health > 0)
        {
            for (int i = 1; i < 12; i++)
            {
                RagdollBonesPos[i] = Rigidbodies[i].transform.position;
            }
            transform.position = new Vector3(Root.transform.GetChild(0).position.x, transform.position.y, Root.transform.GetChild(0).transform.position.z);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Root.transform.GetChild(0).rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            if (ZombieController != null && Health < 10 && CrawlController!=null)
                animator.runtimeAnimatorController = CrawlController;
            else
                animator.runtimeAnimatorController = GetUpController;
            animator.Rebind();
            if (Physics.Raycast(Rigidbodies[9].transform.position, Rigidbodies[9].transform.up, GroundLayer))
            {
                animator.SetFloat("GetUpPose", 1);
            }
            else
            {
                animator.SetFloat("GetUpPose", 0);
            }
            EnableRagdoll(false);
            timeLerped = 0f;
            if (enemyController != null)
            {
                enemyController.CurrentEnemyState = Enemy.EnemyState.AttackPlayer;
                EnemyAlert.Reference.AlertNearbyEnemies(transform.position, 4f);
            }
            else
            {
                ZombieController.CurrentState = Zombie.ZombieState.Attack;
            }
        } 
        else
        {
            Root.transform.parent = RootParent;
            gameObject.SetActive(false);
        }

    }
    public void ChangeHealth(int DeltaHealth)
    {
        Health += DeltaHealth;
    }
    public float TimetoLerp;
    public float timeLerped;
    void LateUpdate()
    {
        if (timeLerped < TimetoLerp)
        {
            Rigidbodies[1].transform.position = Vector3.Lerp(RagdollBonesPos[1], animator.GetBoneTransform(HumanBodyBones.Hips).position, timeLerped/TimetoLerp);
            Rigidbodies[2].transform.position = Vector3.Lerp(RagdollBonesPos[2], animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).position, timeLerped / TimetoLerp);
            Rigidbodies[3].transform.position = Vector3.Lerp(RagdollBonesPos[3], animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position, timeLerped / TimetoLerp);
            Rigidbodies[4].transform.position = Vector3.Lerp(RagdollBonesPos[4], animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).position, timeLerped / TimetoLerp);
            Rigidbodies[5].transform.position = Vector3.Lerp(RagdollBonesPos[5], animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).position, timeLerped / TimetoLerp);
            Rigidbodies[6].transform.position = Vector3.Lerp(RagdollBonesPos[6], animator.GetBoneTransform(HumanBodyBones.Spine).position, timeLerped / TimetoLerp);
            Rigidbodies[7].transform.position = Vector3.Lerp(RagdollBonesPos[7], animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).position, timeLerped / TimetoLerp);
            Rigidbodies[8].transform.position = Vector3.Lerp(RagdollBonesPos[8], animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position, timeLerped / TimetoLerp);
            Rigidbodies[9].transform.position = Vector3.Lerp(RagdollBonesPos[9], animator.GetBoneTransform(HumanBodyBones.Head).position, timeLerped / TimetoLerp);
            Rigidbodies[10].transform.position = Vector3.Lerp(RagdollBonesPos[10], animator.GetBoneTransform(HumanBodyBones.RightUpperArm).position, timeLerped / TimetoLerp);
            Rigidbodies[11].transform.position = Vector3.Lerp(RagdollBonesPos[11], animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position, timeLerped / TimetoLerp);

            for (int i = 1; i < 12; i++)
            {
                RagdollBonesPos[i] = Rigidbodies[i].transform.position;
            }
            timeLerped += Time.deltaTime;
        }
        
    }
}
