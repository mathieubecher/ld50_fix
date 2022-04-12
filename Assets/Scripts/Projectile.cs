using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float TOLERANCE = 0.01f;
    
    private Rigidbody2D m_rigidbody;
    [SerializeField] private float m_speed = 20.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_rigidbody.velocity = transform.right * m_speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Math.Abs(m_rigidbody.velocity.magnitude) < TOLERANCE) return;
        float angle = Vector2.SignedAngle(Vector2.right, m_rigidbody.velocity);
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.isTrigger)
        {
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.isKinematic = true;
        }
    }
}
