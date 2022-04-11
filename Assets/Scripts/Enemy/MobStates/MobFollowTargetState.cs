using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MobFollowTargetState : MobRootMotionState
{

    [Header("Follow")]
    [SerializeField] private bool m_return = true;
    [SerializeField] private float m_speed = 5.0f;
    [SerializeField] private float m_inertia = 0.0f;

    private Hitable m_hitable;
    private Rigidbody2D m_rigidBody;
    

    private Transform m_target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_hitable = animator.GetComponent<Hitable>();
        m_rigidBody = m_hitable.rigidbody;
        
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
        
        m_rigidBody.velocity = Vector3.zero;
    }
    
    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        float TOLERANCE = 0.01f;
        bool horizontal = math.abs(m_rigidBody.gravityScale) > TOLERANCE;
            
        Vector3 desiredDirection = Vector3.zero;
        if(!horizontal) desiredDirection = (m_target.position - animator.transform.position);
        else
        {
            desiredDirection = (m_target.position - animator.transform.position);
            desiredDirection.y = 0.0f;
        }
        float distance = desiredDirection.magnitude;
        desiredDirection.Normalize();
        
        if (m_return && math.abs(m_target.position - animator.transform.position).x > 1f)
        {
            Vector3 scale = m_hitable.body.localScale;
            scale.x = math.abs(scale.x) * math.sign(desiredDirection.x);
            Debug.Log(scale);
            m_hitable.body.localScale = scale;
        }
        Debug.Log(distance);
        if(distance > m_speed * Time.deltaTime)
            m_rigidBody.velocity = desiredDirection * m_speed + (horizontal ? Vector3.up * m_rigidBody.velocity.y : Vector3.zero);
        else
            m_rigidBody.velocity = new Vector2(0.0f, !horizontal ? m_rigidBody.velocity.y : 0.0f);
            

    }
}
