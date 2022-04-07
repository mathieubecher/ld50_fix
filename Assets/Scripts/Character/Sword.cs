using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    
    public delegate void Hit(Collider2D _other);
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
        Hitable hit;
        if (_other.gameObject.TryGetComponent(out hit))
        {
            OnHit(_other);
        }
        else
        {
            OnHitWall();
        }
    }
}
