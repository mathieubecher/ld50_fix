using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    private Animator m_animator;
    private LifeController m_lifeControllerRef;
    private bool m_death = false;
    
    void OnEnable()
    {
        Restart.OnReplay += Replay;
    }


    void OnDisable()
    {
        Restart.OnReplay -= Replay;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_lifeControllerRef = FindObjectOfType<LifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_death && m_lifeControllerRef.life <= 0)
        {
            Dead();
            m_death = true;
        }
    }
    public void Dead()
    {
        m_animator.SetTrigger("Dead");
    }
    
    public void Replay()
    {
        m_animator.SetTrigger("Reset");
        m_death = false;
    }
}
