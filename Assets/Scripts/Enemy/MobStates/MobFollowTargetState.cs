using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobFollowTargetState : MobRootMotionState
{

    private Rigidbody2D m_rigidBody;
    [Header("Follow")]
    [SerializeField] private bool m_return = true;
    [SerializeField] private Transform m_returnBody;
    [SerializeField] private float m_speed = 5.0f;
    [SerializeField] private float m_inertia = 0.0f;
    

    private Transform m_target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_rigidBody = animator.GetComponent<Rigidbody2D>();
        
        m_target = FindObjectOfType<Character>().transform;
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        
    }
    
    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);

        Vector3 desiredVelocity = (m_target.position - animator.transform.position).normalized * m_speed;
        m_rigidBody.velocity = desiredVelocity;

    }
}
