using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private Character m_character;
    private int m_life = 5;
    public int life => m_life;
    public delegate void CharacterTakeDamage(int life);
    public static event CharacterTakeDamage OnCharacterLifeChange;

    private int m_startLife;
    // Start is called before the first frame update
    void Start()
    {
        m_startLife = life;
    }
    
    void OnEnable()
    {
        m_character = GetComponent<Character>();
        Restart.OnReplay += Replay;
        m_character.OnHit += TakeDamage;
    }


    void OnDisable()
    {
        m_character.OnHit -= TakeDamage;
        Restart.OnReplay -= Replay;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(int _damage)
    {
        m_life = math.clamp(m_life - _damage, 0, m_startLife);
        Debug.Log(_damage);
        OnCharacterLifeChange?.Invoke(life);
        if (life == 0)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }
    

    private void Replay()
    {
        m_life = m_startLife;
        OnCharacterLifeChange?.Invoke(life);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
