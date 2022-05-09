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

    void Start()
    {
        MainRigidbody = transform.GetComponent<Rigidbody>();
        MainCollider = transform.GetComponent<Collider>();
        Rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        Colliders = transform.GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();
        EnableRagdoll(false);
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
        transform.position = new Vector3(Root.transform.GetChild(0).position.x, transform.position.y, Root.transform.GetChild(0).transform.position.z);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Root.transform.GetChild(0).rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        Root.transform.parent = transform;
        animator.runtimeAnimatorController = GetUpController;
        animator.Rebind();
        EnableRagdoll(false);

        //gameObject.SetActive(false);
    }
}
