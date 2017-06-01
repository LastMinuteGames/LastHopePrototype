﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEnemyChase : StateMachineBehaviour {
    EnemyTrash enemyTrash;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(enemyTrash == null)
        {
            enemyTrash = animator.transform.gameObject.GetComponent<EnemyTrash>();
        }
        Debug.Log("Chase Enter");
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Chase Update");
        if (enemyTrash.target != null)
        {
            if (enemyTrash.nav.remainingDistance >= enemyTrash.combatRange)
            {
                enemyTrash.nav.SetDestination(enemyTrash.target.position);
                enemyTrash.nav.Resume();
            }
            else
            {
                animator.SetBool("chase", false);
                animator.SetBool("combat", true);
                enemyTrash.nav.Stop();
            }
        }
        else
        {
            animator.SetBool("chase", false);
            animator.SetBool("iddle", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Chase exit");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
