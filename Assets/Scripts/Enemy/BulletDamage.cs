using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int HealthChange;
    RagdollManager ragdoll;
    private void Start()
    {
        ragdoll = transform.GetComponentInParent<RagdollManager>();
    }
    public void ModifyHealth(int Damage)
    {
        ragdoll.ChangeHealth(HealthChange-Damage);
    }
}
