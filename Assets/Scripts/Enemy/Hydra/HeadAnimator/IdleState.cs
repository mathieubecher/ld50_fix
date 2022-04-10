using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : StateMachineBehaviour
{
    public float idleMinDuration = 2.0f;
    public float idleMaxDuration = 5.0f;
    public float slerpSpeed = 0.05f;
    
    private float m_idleTimer;
    
    protected Head m_head;
    protected Character m_target;
    protected Animator m_animator;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("StatePerformed"); 
        if(!m_head) m_head = animator.GetComponent<Head>();
        if(!m_target) m_target = FindObjectOfType<Character>();
        
        m_animator = animator;
        m_head.OnHit += Hit;
        m_idleTimer = math.lerp(idleMinDuration, idleMaxDuration, Random.value);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_head.hydra.detectePlayer) return;
        m_idleTimer -= Time.deltaTime;
        if(m_idleTimer < 0.0f) animator.SetTrigger("StatePerformed");
    }
    
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Follow player direction
        Vector3 targetDir = (m_target.transform.position - animator.transform.position).normalized;

        Quaternion rotation = m_head.transform.rotation;
        m_head.transform.rotation = Quaternion.identity;
        float angle = Vector3.SignedAngle(Vector3.left, targetDir, Vector3.forward);

        Vector3 position = animator.transform.position;
        animator.ApplyBuiltinRootMotion();
        
        Quaternion desiredRotation = angle > -120f && angle < 90f ? Quaternion.Euler(0f, 0f, angle) : Quaternion.identity;
        m_head.transform.rotation = Quaternion.Slerp(rotation, desiredRotation, slerpSpeed);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_head.OnHit -= Hit;
    }

    void Hit(Collider2D _other, int _damage)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("Character"))
            _other.GetComponent<Character>().Hit(m_head, _damage);
    }
}
