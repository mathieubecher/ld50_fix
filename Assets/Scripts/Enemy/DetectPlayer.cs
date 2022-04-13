using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectPlayer : MonoBehaviour
{

    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private string m_triggerTargetDistance = "TargetDistance";
    [SerializeField]
    private float m_exitDistance = 20f;
    
    public UnityEvent enterPlayer;
    public UnityEvent exitPlayer;
    public UnityEvent replay;

    private bool m_detectPlayer;
    public Transform m_target;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Restart.OnReplay += Replay;
    }
    private void OnDisable()
    {
        Restart.OnReplay -= Replay;
    }

    private void Update()
    {
        if (!m_animator) return;
        
        if (m_detectPlayer)
        {
            float distance = (transform.position - m_target.position).magnitude;
            
            m_animator.SetFloat(m_triggerTargetDistance, (transform.position - m_target.position).magnitude);
            if (distance > m_exitDistance) m_detectPlayer = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        m_detectPlayer = true;
        m_target = _other.transform;
        enterPlayer?.Invoke();
        
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        exitPlayer?.Invoke();
        
    }

    private void Replay()
    {
        replay?.Invoke();
    }
}
