using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    public static bool detectPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        detectPlayer = true;
    }

    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
    }
}
