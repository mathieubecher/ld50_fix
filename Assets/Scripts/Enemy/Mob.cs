using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Hitable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Hit(Vector3 _direction)
    {
        Destroy(gameObject);
    }
}
