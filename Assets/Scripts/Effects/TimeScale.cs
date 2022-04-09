using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    private float timeScaleEffectTimer = 0.0f;

    public int target = 60;
     
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
        
    }
     
    void Update()
    {
        if(Application.targetFrameRate != target)
            Application.targetFrameRate = target;
    
        if (timeScaleEffectTimer > 0.0f) 
        {
            timeScaleEffectTimer -= Time.unscaledDeltaTime;
        }
    }
    
    public void FreezeTime(float _duration)
    {
        StartCoroutine(FreezeTimeCoroutine(_duration));
    }
    IEnumerator FreezeTimeCoroutine(float duration)
    {
        Time.timeScale = .0000001f;
        yield return new WaitForSeconds(duration * Time.timeScale);
        Time.timeScale = 1.0f;
    }
}
