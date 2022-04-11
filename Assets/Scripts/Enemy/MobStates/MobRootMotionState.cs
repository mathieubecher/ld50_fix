using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRootMotionState : StateMachineBehaviour
{
    
    [SerializeField] protected bool m_canTouchValueAtStart = false;
    [Header("Root Motion")]
    [SerializeField] protected bool m_applyRootPosition = true;
    [SerializeField] protected bool m_applyRootRotation = true;
    
    private Hitable m_hitable;
    private Rigidbody2D m_rigidBody;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_hitable = animator.GetComponent<Hitable>();
        m_rigidBody = m_hitable.rigidbody;
        if(m_canTouchValueAtStart) animator.GetComponent<Hitable>().SetCanHitPlayer();
        else animator.GetComponent<Hitable>().SetCantHitPlayer();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        m_rigidBody.velocity = new Vector2(0.0f, m_rigidBody.gravityScale>0? m_rigidBody.velocity.y : 0.0f);
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
