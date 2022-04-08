using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    
    public delegate void Hit(Hitable _other);
    public event Hit OnHit;
    public delegate void HitWall();
    public event HitWall OnHitWall;

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
            OnHit(hit.parent);
        }
        else
        {
            OnHitWall();
        }
    }
}
