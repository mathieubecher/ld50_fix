using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.gameObject.layer == LayerMask.NameToLayer("Head"))
            Debug.Log("Hit");
    }
}
