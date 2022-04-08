using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Sword sword;
    [HideInInspector]
    public Character character;
    public bool isAttacking;
    
    private Animator m_animator;
    private bool m_alreadyTouch;
    private List<Hitable> m_touchedList;

    private Vector2 m_requireDirection;
    // Start is called before the first frame update
    void Init()
    {
        sword.gameObject.SetActive( false);
    }
    void OnEnable()
    {
        sword.OnHit += Hit;
        sword.OnHitWall += HitWall;
    }
    void OnDisable()
    {
        sword.OnHit -= Hit;
        sword.OnHitWall -= HitWall;
    }
    void Start()
    {
        m_touchedList = new List<Hitable>();
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
        m_alreadyTouch = false;
        m_touchedList = new List<Hitable>();
    }

    public void Attack(Vector2 _direction)
    {
        m_animator.SetTrigger("Attack");
        m_animator.SetFloat("yInput", _direction.y);
    }   
    
    public void Hit(Hitable _other)
    {
        if (!m_touchedList.Contains(_other))
        {
            _other.Hit(Vector3.right * character.transform.localScale.z);
            m_touchedList.Add(_other);
        }
        if (!m_alreadyTouch)
        {
            character.AttackTouched();
            m_alreadyTouch = true;
        }
    }
    public void HitWall()
    {
        if (!m_alreadyTouch)
        {
            character.AttackTouchedWall();
            m_alreadyTouch = true;
        }
    }
}
