using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectPlayer : MonoBehaviour
{
    public bool detectPlayer;

    public UnityEvent enterPlayer;
    public UnityEvent exitPlayer;
    public UnityEvent replay;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Restart.OnReplay += Replay;
    }
    private void OnDisable()
    {
        Restart.OnReplay -= Replay;
    }
    
    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        detectPlayer = true;
        enterPlayer?.Invoke();
        
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        exitPlayer?.Invoke();
        
    }

    private void Replay()
    {
        replay?.Invoke();
    }
}
