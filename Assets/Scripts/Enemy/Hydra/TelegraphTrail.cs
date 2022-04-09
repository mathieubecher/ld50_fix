using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphTrail : MonoBehaviour
{
    [SerializeField]
    private float m_duration = 5f;

    [SerializeField] 
    private AnimationCurve m_position;
    
    private Vector3 m_originPos;

    private float m_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if(m_time > m_duration) Destroy(gameObject);
        
        
        transform.position = -transform.right * m_position.Evaluate(m_time) + m_originPos;
        
    }
}
