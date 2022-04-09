using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchState : StateMachineBehaviour
{
    protected Head m_head;
    private float m_timer;
    
    [SerializeField]
    private float m_duration = 1.0f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_head) m_head = animator.GetComponent<Head>();
        m_timer = m_duration;
        m_head.OnTouch += Touch;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_timer -= Time.deltaTime;
        if(m_timer < 0.0f) animator.SetTrigger("StatePerformed");
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_head.OnTouch -= Touch;
    }
    void Touch(Collider2D _other, int _damage)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("Character"))
            _other.GetComponent<Character>().Hit(m_head, _damage);
    }
}