using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Sword sword;
    public bool isAttacking;
    
    private Animator m_animator;

    private Vector2 m_requireDirection;
    // Start is called before the first frame update
    void Init()
    {
        sword.gameObject.SetActive( false);
    }

    void Start()
    {
        
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadAttack()
    {
        m_animator.ResetTrigger("Attack");
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    public void Attack(Vector2 _direction)
    {
        m_animator.SetTrigger("Attack");
        m_requireDirection = _direction;
    }
}
