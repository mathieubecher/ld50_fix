using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitable : MonoBehaviour
{
    public delegate void Touch(Collider2D _other);
    public event Touch OnTouch;
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
    
    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.isTrigger)
        {
            OnTouch?.Invoke(_other);
        }
        
    }
}
