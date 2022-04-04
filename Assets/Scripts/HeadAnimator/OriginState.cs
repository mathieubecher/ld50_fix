using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginState : StateMachineBehaviour
{
    public float originMaxDuration = 5.0f;
    private float originTimer;
    protected Head m_head;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_head) m_head = animator.GetComponent<Head>();
        originTimer = Random.value * originMaxDuration;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        originTimer -= Time.deltaTime;
        if(originTimer < 0.0f) animator.SetTrigger("StatePerformed");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
