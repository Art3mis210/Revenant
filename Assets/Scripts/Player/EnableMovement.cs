using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMovement : StateMachineBehaviour
{


    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerController>().ChangeMovement(1);
        animator.GetComponent<PlayerExecution>().Knife.SetActive(false);
        animator.GetComponent<BulletHit>().EnableBulletHit = true;
    }

}
