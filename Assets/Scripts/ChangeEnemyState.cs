using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyState : StateMachineBehaviour
{
    public Enemy.EnemyState NewState;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.GetComponent<Enemy>().CurrentEnemyState=NewState;
        
    }

}
