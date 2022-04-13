using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OrphelinHitBox : MonoBehaviour
{
    
    [SerializeField] private int m_damage = 1;
    [SerializeField] private bool m_destroyAtHit;
    [SerializeField] private bool m_isFriendly;

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (m_isFriendly)
        {
            Debug.Log("Touch");
            HitBox hitBox;
            if (_other.TryGetComponent(out hitBox))
            {
                hitBox.parent.DirectHit( hitBox.transform.position - transform.position, (int)math.floor(m_damage * hitBox.vulnerabilityRatio));
                if(m_destroyAtHit) Destroy(gameObject);
            }
        }
        else if (!_other.isTrigger && _other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            _other.GetComponent<Character>().Damaged(transform, m_damage);
            
            if(m_destroyAtHit) Destroy(gameObject);
        }
    }
}
