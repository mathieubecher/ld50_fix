using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtRestart : MonoBehaviour
{
    void OnEnable()
    {
        Restart.OnReplay += Replay;
    }


    void OnDisable()
    {
        Restart.OnReplay -= Replay;
    }
    
    public void Replay()
    {
        Destroy(gameObject);
    }
}
