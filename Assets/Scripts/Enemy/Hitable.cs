using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    public delegate void Touch(Collider2D _other, int _damage);

    public event Touch OnTouch;


    [SerializeField] protected bool m_canTouch = false;
    [SerializeField] protected int m_life = 1;

    public int life
    {
        get => m_life;
    }

    [SerializeField] protected int m_damage = 1;

    // Start is called before the first frame update
    void Start()
    {

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

    private Arm m_attackOrigin;
    private List<HitInfo> m_hitInfos = new List<HitInfo>();
    public virtual void Hit(Arm _attackOrigin, Vector3 _direction, int _damage)
    {
        if(m_hitInfos.Count == 0) _attackOrigin.OnAttackPerformed += PerfomAttack;
        m_attackOrigin = _attackOrigin;
        m_hitInfos.Add(new HitInfo(_direction, _damage));
    }

    public void PerfomAttack()
    {
        if (m_hitInfos.Count > 0)
        {
            HitInfo minDist = m_hitInfos[0];
            foreach (HitInfo hitInfo in m_hitInfos)
            {
                if (hitInfo.direction.magnitude < minDist.direction.magnitude) minDist = hitInfo;
            }

            m_life = math.max(0, m_life -= minDist.damage);
            if (m_life <= 0)
            {
                Dead();
            }
            m_hitInfos = new List<HitInfo>();
        }

        m_attackOrigin.OnAttackPerformed -= PerfomAttack;

    }
    
    
    public virtual void Dead()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.isTrigger)
        {
            if (m_canTouch)
            {
                ReadTouchEvent(_other);
            }
        }
    }

    protected virtual void ReadTouchEvent(Collider2D _other)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("Character"))
            _other.GetComponent<Character>().Hit(this, m_damage);
        OnTouch?.Invoke(_other, m_damage);
    }

    public void SetCanTouch(bool _canTouch)
    {
        m_canTouch = _canTouch;
    }
    
}
