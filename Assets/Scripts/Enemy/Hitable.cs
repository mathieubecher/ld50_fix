using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    public delegate void HitEvent(Collider2D _other, int _damage);
    public event HitEvent OnHit;

    [SerializeField] protected Animator m_animator;
    [SerializeField] protected Rigidbody2D m_rigidbody;

    public Rigidbody2D rigidbody => m_rigidbody;

    public Transform body;
    
    private Arm m_attackOrigin;
    private List<HitInfo> m_hitInfos = new List<HitInfo>();

    [SerializeField] protected bool m_canHit = false;
    [SerializeField] protected int m_life = 1;
    [SerializeField] protected int m_damage = 1;
    
    [Header("Anim Triggers")]
    [SerializeField] protected string m_triggerHit = "Hit";
    [SerializeField] protected string m_triggerDamaged = "Damaged";
    [SerializeField] protected string m_triggerDead = "Dead";

    public int life
    {
        get => m_life;
    }


    // Start is called before the first frame update
    void Start()
    {
        if(!m_animator) m_animator = GetComponent<Animator>();
        if(!m_rigidbody) m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private struct HitInfo
    {
        public HitInfo(Vector3 _direction, int _damage)
        {
            direction = _direction;
            damage = _damage;
        }
        public Vector3 direction;
        public int damage;
    }
    
    public virtual void Hit(Arm _attackOrigin, Vector3 _direction, int _damage)
    {
        if (m_hitInfos.Count == 0)
        {
            m_attackOrigin = _attackOrigin;
            _attackOrigin.OnAttackPerformed += OriginAttackPerfomed;
        }
        m_hitInfos.Add(new HitInfo(_direction, _damage));
    }

    
    protected void OriginAttackPerfomed()
    {
        if (m_hitInfos.Count > 0)
        {
            HitInfo minDist = m_hitInfos[0];
            foreach (HitInfo hitInfo in m_hitInfos)
            {
                if (hitInfo.direction.magnitude < minDist.direction.magnitude) minDist = hitInfo;
                OnDamaged();
            }

            m_life = math.max(0, m_life -= minDist.damage);
            if (m_life <= 0)
            {
                OnDead();
            }
            m_hitInfos = new List<HitInfo>();
        }
    }

    protected virtual void OnDamaged()
    {
        if (m_triggerDamaged != "")
        {
            m_animator.SetTrigger(m_triggerDamaged);
        }
    }
    
    
    protected virtual void OnDead()
    {
        if (m_triggerDead != "")
        {
            m_animator.SetTrigger(m_triggerDead);
        }
    }
    

    public virtual void ReadHitEvent(HitBox _hitbox, Collider2D _other)
    {
        
        if(_other.gameObject.layer == LayerMask.NameToLayer("Character"))
            _other.GetComponent<Character>().Damaged(transform, m_damage);
        
        OnHit?.Invoke(_other, m_damage);
    }

    public void SetCanHitPlayer()
    {
        m_canHit = true;
    }
    public void SetCantHitPlayer()
    {
        m_canHit = false;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
