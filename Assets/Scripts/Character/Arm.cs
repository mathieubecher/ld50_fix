using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField]
    private Weapon m_weapon;
    
    public Character character;
    public bool isAttacking;
    
    private Animator m_animator;
    private bool m_alreadyTouch;
    private List<HitBox> m_touchedList;

    private Vector2 m_requireDirection;
    
    public delegate void AttackPerformed();
    public event AttackPerformed OnAttackPerformed;
    // Start is called before the first frame update

    void OnEnable()
    {
        m_weapon.OnHit += Hit;
        m_weapon.OnHitWall += HitWall;
    }
    void OnDisable()
    {
        m_weapon.OnHit -= Hit;
        m_weapon.OnHitWall -= HitWall;
    }

    void Init()
    {
        m_weapon.gameObject.SetActive( false);
    }
    
    void Start()
    {
        m_touchedList = new List<HitBox>();
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
        m_touchedList = new List<HitBox>();
        OnAttackPerformed?.Invoke();
        OnAttackPerformed = null;
    }

    public void Attack(Vector2 _direction)
    {
        m_animator.SetTrigger("Attack");
        m_animator.SetFloat("yInput", _direction.y);
    }   
    
    public void Hit(HitBox _hitBox, int _damage)
    {
        if (!m_touchedList.Contains(_hitBox))
        {
            _hitBox.parent.Hit(this, _hitBox.transform.position - transform.position, (int)math.floor(_damage * _hitBox.vulnerabilityRatio));
            m_touchedList.Add(_hitBox);
        }
        if (!m_alreadyTouch)
        {
            character.AttackTouched();
            m_alreadyTouch = true;
            FindObjectOfType<TimeScale>().FreezeTime(0.1f);
        }
    }
    public void HitWall()
    {
        if (!m_alreadyTouch)
        {
            character.AttackTouchedWall();
            m_alreadyTouch = true;
            FindObjectOfType<TimeScale>().FreezeTime(0.05f);
        }
    }
}
