using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrphelinHitBox : MonoBehaviour
{
    
    [SerializeField] private int m_damage = 1;
    [SerializeField] private bool m_destroyAtHit;
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.isTrigger)
        {
            ReadHitEvent( _other);
        }
    }

    private void ReadHitEvent(Collider2D _other)
    {

        if (_other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            _other.GetComponent<Character>().Damaged(transform, m_damage);
            
            if(m_destroyAtHit) Destroy(gameObject);
        }
        
    }
}
