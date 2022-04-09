using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField]
    private Hitable m_parent;

    public Hitable parent {get => m_parent;}

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
}
