using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private Hitable m_parent;
    public Hitable parent {get => m_parent;}
    
    [SerializeField]
    private float m_vulnerabilityRatio = 1.0f;
    public float vulnerabilityRatio {get => m_vulnerabilityRatio;}



    void Awake()
    {
        if (!m_parent)
        {
            m_parent = GetComponentInParent<Hitable>();
            if (!m_parent)
            {
                Debug.LogWarning(gameObject.name +" Hitbox have to set on a parent containing Hitable", gameObject);
            }
        }
        if(!TryGetComponent<Collider2D>(out _)) Debug.LogWarning(gameObject.name +" Hitbox need at least one collider", gameObject);

        gameObject.layer = LayerMask.NameToLayer("HitPlayer");
    }
    
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.isTrigger)
        {
            m_parent.ReadHitEvent(this, _other);
        }
    }
}
