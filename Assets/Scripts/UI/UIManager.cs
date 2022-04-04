using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Lifebar lifeBar;

    void OnEnable()
    {
        LifeController.OnCharacterTakeDamage += ReceiveDamage;
    }
    void OnDisable()
    {
        LifeController.OnCharacterTakeDamage -= ReceiveDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReceiveDamage(int _life)
    {
        lifeBar.SetLife(_life);
    }
}
