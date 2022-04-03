using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    private float timeScaleEffectTimer = 0.0f;
    private Character m_character;
    void OnEnable()
    {
        m_character = FindObjectOfType<Character>();
        m_character.OnAttackTouched += StopTimeHitEvent;
        m_character.OnHit += StopTimeHitEvent;
    }
    void OnDisable()
    {
        m_character.OnAttackTouched -= StopTimeHitEvent;
        m_character.OnHit -= StopTimeHitEvent;
    }

    void Update()
    {
        if (timeScaleEffectTimer > 0.0f) 
        {
            timeScaleEffectTimer -= Time.unscaledDeltaTime;
        }
    }
    void StopTimeHitEvent()
    {
        StartCoroutine(StopTimeHit());
        
    }
    IEnumerator StopTimeHit()
    {
        Time.timeScale = .0000001f;
        yield return new WaitForSeconds(0.1f * Time.timeScale);
        Time.timeScale = 1.0f;
        
    }
}
