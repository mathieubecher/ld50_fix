using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactible
{
    enum Effect
    {
        Heal,
    }

    [SerializeField] private Effect m_effect;

    [SerializeField] private int m_value;

    public override void EnterTriggerZone(Character _character)
    {
        base.EnterTriggerZone(_character);
        _character.GetComponent<LifeController>().TakeDamage(-m_value);
        Destroy(gameObject);
    }
}
