using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRootMotionState : StateMachineBehaviour
{
    
    [SerializeField] protected bool m_canTouchValueAtStart = false;
    [Header("Root Motion")]
    [SerializeField] protected bool m_applyRootPosition = true;
    [SerializeField] protected bool m_applyRootRotation = true;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_canTouchValueAtStart) animator.GetComponent<Hitable>().SetCanHitPlayer();
        else animator.GetComponent<Hitable>().SetCantHitPlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
    
    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.DrawLine(animator.transform.position, animator.rootPosition, Color.red, 1.0f);
        if(m_applyRootPosition) animator.transform.position = animator.rootPosition;
        if(m_applyRootRotation) animator.transform.rotation = animator.rootRotation;
    }
}
