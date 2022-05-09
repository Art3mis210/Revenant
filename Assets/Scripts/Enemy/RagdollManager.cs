using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private Rigidbody MainRigidbody;
    private Collider MainCollider;
    public Collider ExecutionCollider;
    [SerializeField] private Rigidbody[] Rigidbodies;
    private Collider[] Colliders;
    private Animator animator;
    [SerializeField] private RuntimeAnimatorController GetUpController;
    public GameObject Root;
    Vector3[] RagdollBonesPos;
    Quaternion[] RagdollBonesRot;
    public HumanBodyBones bones;

    void Start()
    {
        MainRigidbody = transform.GetComponent<Rigidbody>();
        MainCollider = transform.GetComponent<Collider>();
        Rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        Colliders = transform.GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();
        EnableRagdoll(false);
        RagdollBonesPos = new Vector3[12];
        RagdollBonesRot = new Quaternion[12];
    }

    public void EnableRagdoll(bool Status)
    {
        foreach(Rigidbody rb in Rigidbodies)
        {
            if (rb != MainRigidbody)
                rb.isKinematic = !Status;
        }
        foreach (Collider col in Colliders)
        {
            if (col != MainCollider)
            {
                if(col.gameObject.layer==7)
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
        ExecutionCollider.enabled = !Status;
        if(Status)
        {
            Root.transform.parent = null;
            
            Invoke("TurnOff", 5f);
            
        }

    }
    void TurnOff()
    {
        for (int i = 1; i < 12; i++)
        {
            RagdollBonesPos[i] = Rigidbodies[i].transform.position;
        }
        for (int i = 1; i < 12; i++)
        {
            RagdollBonesRot[i] = Rigidbodies[i].transform.rotation;
        }
        transform.position = new Vector3(Root.transform.GetChild(0).position.x, transform.position.y, Root.transform.GetChild(0).transform.position.z);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Root.transform.GetChild(0).rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Root.transform.parent = transform;
        animator.runtimeAnimatorController = GetUpController;
        animator.Rebind();
        EnableRagdoll(false);
        timeLerped = 0f;

        //gameObject.SetActive(false);
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

            /*Rigidbodies[1].transform.rotation = Quaternion.Slerp(RagdollBonesRot[1], animator.GetBoneTransform(HumanBodyBones.Hips).rotation, timeLerped / TimetoLerp);
            Rigidbodies[2].transform.rotation = Quaternion.Slerp(RagdollBonesRot[2], animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).rotation, timeLerped / TimetoLerp);
            Rigidbodies[3].transform.rotation = Quaternion.Slerp(RagdollBonesRot[3], animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).rotation, timeLerped / TimetoLerp);
            Rigidbodies[4].transform.rotation = Quaternion.Slerp(RagdollBonesRot[4], animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).rotation, timeLerped / TimetoLerp);
            Rigidbodies[5].transform.rotation = Quaternion.Slerp(RagdollBonesRot[5], animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).rotation, timeLerped / TimetoLerp);
            Rigidbodies[6].transform.rotation = Quaternion.Slerp(RagdollBonesRot[6], animator.GetBoneTransform(HumanBodyBones.Spine).rotation, timeLerped / TimetoLerp);
            Rigidbodies[7].transform.rotation = Quaternion.Slerp(RagdollBonesRot[7], animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).rotation, timeLerped / TimetoLerp);
            Rigidbodies[8].transform.rotation = Quaternion.Slerp(RagdollBonesRot[8], animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).rotation, timeLerped / TimetoLerp);
            Rigidbodies[9].transform.rotation = Quaternion.Slerp(RagdollBonesRot[9], animator.GetBoneTransform(HumanBodyBones.Head).rotation, timeLerped / TimetoLerp);
            Rigidbodies[10].transform.rotation = Quaternion.Slerp(RagdollBonesRot[10], animator.GetBoneTransform(HumanBodyBones.RightUpperArm).rotation, timeLerped / TimetoLerp);
            Rigidbodies[11].transform.rotation = Quaternion.Slerp(RagdollBonesRot[11], animator.GetBoneTransform(HumanBodyBones.RightLowerArm).rotation, timeLerped / TimetoLerp);
            */
            for (int i = 1; i < 12; i++)
            {
                RagdollBonesPos[i] = Rigidbodies[i].transform.position;
            }
            /*for (int i = 1; i < 12; i++)
            {
                RagdollBonesRot[i] = Rigidbodies[i].transform.rotation;
            }*/
            timeLerped += Time.deltaTime;
        }
        
    }
}
