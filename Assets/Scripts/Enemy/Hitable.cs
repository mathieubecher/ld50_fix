using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    public delegate void Touch(Collider2D _other, int _damage);
    public event Touch OnTouch;


    [SerializeField] protected bool m_canTouch = false;
    [SerializeField] protected int m_life = 1;
    [SerializeField] protected int m_damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Hit(Vector3 _direction)
    {
        m_life--;
        if (m_life <= 0)
        {
            Dead();
        }
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
