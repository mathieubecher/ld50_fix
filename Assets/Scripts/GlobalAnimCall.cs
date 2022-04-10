using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAnimCall : MonoBehaviour
{
    protected static TimeScale m_timeScaleRef;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FreezeTime(float _duration)
    {
        if(!m_timeScaleRef) m_timeScaleRef = FindObjectOfType<TimeScale>();
        m_timeScaleRef.FreezeTime(_duration);
    }
    
    public void Spawn(GameObject _gameObject)
    {
        Instantiate(_gameObject, transform.position, transform.rotation);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    
}
