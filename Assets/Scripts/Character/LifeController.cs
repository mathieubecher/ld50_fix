using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private Character m_character;

    public int life = 5;
    // Start is called before the first frame update
    void Start()
    {
    }
    
    void OnEnable()
    {
        m_character = GetComponent<Character>();
        
        m_character.OnHit += ReceiveDamage;
    }


    void OnDisable()
    {
        m_character.OnHit -= ReceiveDamage;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ReceiveDamage()
    {
        life--;
        if (life == 0)
        {
            Debug.Log("Dead");
        }
    }
}
