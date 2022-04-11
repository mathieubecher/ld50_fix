using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
    private bool m_enter;
    public Controller m_controller;
    void OnEnable()
    {
        m_controller = FindObjectOfType<Controller>();
        m_enter = false;
    }
    void OnDisable()
    {     
        if(m_enter) m_controller.OnInteract -= Interact;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        
        Character character;
        if (_other.TryGetComponent(out character))
        {
            EnterTriggerZone(character);
        }
    }
    
    void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.isTrigger) return;
        
        if (_other.TryGetComponent<Character>(out _))
        {
            ExitTriggerZone();
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interact");
    }

    public virtual void EnterTriggerZone(Character _character)
    {
        m_enter = true;
        m_controller.OnInteract += Interact;
    }

    public virtual void ExitTriggerZone()
    {
        m_enter = false;
        m_controller.OnInteract -= Interact;
        
    }
}
