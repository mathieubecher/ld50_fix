using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    protected Head m_head;
    protected Animator m_animator;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Touch");
        if(!m_head) m_head = animator.GetComponent<Head>();
        
        m_animator = animator;
        m_head.OnHit += Hit;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_head.OnHit -= Hit;
    }

    void Hit(Collider2D _other, int _damage)
    {
        if (!m_animator) return;
        
        if(_other.gameObject.layer != LayerMask.NameToLayer("Character")) m_animator.SetTrigger("Touch");
    }
}
