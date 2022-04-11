using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RecoveryState : StateMachineBehaviour
{
    protected Head m_head;
    protected Animator m_animator;
    protected Hydra m_hydra;

    private float timer;
    private Vector3 m_recoveryPos;
    private Vector3 m_origin;
    private Character m_target;
    private bool m_exit;

    [SerializeField]
    private AnimationCurve m_recoveryCurve;
    public float slerpSpeed = 0.05f;
    public bool canHit = false ;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
        m_exit = false;
        
        if(!m_head) m_head = animator.GetComponent<Head>();
        if(!m_target) m_target = FindObjectOfType<Character>();
        
        m_origin = animator.transform.position;
        m_recoveryPos = m_head.hydra.GetValidHeadPosition(m_head);
        
        m_animator = animator;
        if(canHit) m_head.OnHit += Hit;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        float progress = m_recoveryCurve.Evaluate(timer);
        m_head.transform.position = math.lerp(m_origin, m_recoveryPos, progress);

        if (progress >= 1f && !m_exit)
        {
            m_exit = true;
            animator.SetTrigger("StatePerformed");
        } 
    }

    
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Follow player direction
        Vector3 targetDir = (m_target.transform.position - animator.transform.position).normalized;

        Quaternion rotation = m_head.transform.rotation;
        m_head.transform.rotation = Quaternion.identity;
        float angle = Vector3.SignedAngle(Vector3.left, targetDir, Vector3.forward);
        animator.ApplyBuiltinRootMotion();
        
        Quaternion desiredRotation = math.abs(angle) < 70f? Quaternion.Euler(0f, 0f, angle) : Quaternion.identity;
        m_head.transform.rotation = Quaternion.Slerp(rotation, desiredRotation, slerpSpeed);
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(canHit) m_head.OnHit -= Hit;
    }
    
    void Hit(Collider2D _other, int _damage)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("Character"))
            _other.GetComponent<Character>().Damaged(m_head, _damage);
    }
}
