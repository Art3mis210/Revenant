using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    private Rigidbody MainRigidbody;
    private Collider MainCollider;
    public Collider ExecutionCollider;
    private Rigidbody[] Rigidbodies;
    private Collider[] Colliders;
    private Animator animator;

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
                col.enabled = Status;
        }
        MainRigidbody.isKinematic = Status;
        MainCollider.enabled = !Status;
        animator.enabled = !Status;
        ExecutionCollider.enabled = !Status;

    }
}
