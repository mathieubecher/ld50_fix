using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour
{
    private float timeScaleEffectTimer = 0.0f;
    private Character m_character;
    private Sword m_sword;
    void OnEnable()
    {
        m_character = FindObjectOfType<Character>();
        m_sword = FindObjectOfType<Sword>();
        
        m_character.OnAttackTouched += StopTimeTouchEvent;
        m_character.OnAttackTouchedWall += StopTimeTouchEvent;
        m_character.OnHit += StopTimeHitEvent;
    }
    void OnDisable()
    {
        m_character.OnAttackTouched -= StopTimeHitEvent;
        m_character.OnAttackTouchedWall -= StopTimeHitEvent;
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
        StartCoroutine(StopTimeHit(0.3f));
        
    }
    void StopTimeTouchEvent()
    {
        StartCoroutine(StopTimeHit(0.1f));
    }
    IEnumerator StopTimeHit(float duration)
    {
        Time.timeScale = .0000001f;
        yield return new WaitForSeconds(duration * Time.timeScale);
        Time.timeScale = 1.0f;
    }

    [ContextMenu("Hit")]
    public void HitAll()
    {
        foreach (var head in FindObjectsOfType<Head>())
        {
            head.Hit(Vector3.right);
        }
    }
}
