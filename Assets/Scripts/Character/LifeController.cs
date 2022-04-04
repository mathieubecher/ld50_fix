using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private Character m_character;
    public delegate void CharacterTakeDamage(int life);
    public static event CharacterTakeDamage OnCharacterTakeDamage;
    public int life = 5;

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
    
    private void TakeDamage()
    {
        life--;
        OnCharacterTakeDamage?.Invoke(life);
        if (life == 0)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    private void Replay()
    {
        life = m_startLife;
        OnCharacterTakeDamage?.Invoke(life);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
