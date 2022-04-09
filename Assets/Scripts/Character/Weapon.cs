using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public delegate void Hit(HitBox _other, int _damage);
    public event Hit OnHit;
    public delegate void HitWall();
    public event HitWall OnHitWall;

    [SerializeField] 
    private int m_damage = 1;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        HitBox hit;
        if (_other.gameObject.TryGetComponent(out hit))
        {
            OnHit?.Invoke(hit, m_damage);
        }
        else
        {
            OnHitWall?.Invoke();
        }
    }


}
