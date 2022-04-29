using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExecution : MonoBehaviour
{
    Enemy EnemyController;
    Animator EnemyAnimator;
    Collider EnemyCollider;
    private void Start()
    {
        EnemyAnimator = transform.GetComponentInParent<Animator>();
        EnemyCollider = transform.GetComponentInParent<Collider>();
        EnemyController = transform.GetComponentInParent<Enemy>();
    }
    public void StartExecution(float Execution, Collider PlayerCollider)
    {
        EnemyController.CurrentEnemyState = Enemy.EnemyState.Execution;
        Physics.IgnoreCollision(EnemyCollider, PlayerCollider,true);
        EnemyAnimator.SetFloat("Executions", Execution);
        EnemyAnimator.SetTrigger("StartExecution");
    }
}
