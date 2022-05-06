using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExecution : MonoBehaviour
{
    public enum EnemyType
    {
        Human,Zombie
    }
    public EnemyType CurrentEnemyType;
    Zombie ZombieController;
    Enemy EnemyController;
    Animator EnemyAnimator;
    CapsuleCollider EnemyCollider;
    private void Start()
    {
        EnemyAnimator = transform.GetComponentInParent<Animator>();
        EnemyCollider = transform.GetComponentInParent<CapsuleCollider>();
        if (CurrentEnemyType == EnemyType.Human)
            EnemyController = transform.GetComponentInParent<Enemy>();
        else
            ZombieController = transform.GetComponentInParent<Zombie>();
    }
    public void StartExecution(float Execution)
    {
        if (CurrentEnemyType == EnemyType.Human)
            EnemyController.CurrentEnemyState = Enemy.EnemyState.Execution;
        //else
            //Zombie State Change
        EnemyAnimator.SetFloat("Executions", Execution);
        EnemyAnimator.SetTrigger("StartExecution");
    }
    public void StartInterrogation(float Execution)
    {
        EnemyController.CurrentEnemyState = Enemy.EnemyState.Execution;
        EnemyAnimator.SetInteger("InterrogatePos", 0);
        EnemyAnimator.SetTrigger("Interrogate");
    }
    public void InterrogationKill()
    {
        EnemyAnimator.SetInteger("InterrogatePos", 1);
    }
    public void InterrogationRelease()
    {
        EnemyAnimator.SetInteger("InterrogatePos", -1);
        ReduceEnemyCollider(false);
    }
    public void ReduceEnemyCollider(bool Status)
    {
        if(Status)
        {
            EnemyCollider.radius = 0.15f;
        }
        else
        {
            EnemyCollider.radius = 0.32f;
        }
    }
}
