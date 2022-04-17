using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDefaultAction : ScriptableObject
{
    public bool enable = true;
    
    public virtual void Interact(InteractSystem _interactSystem)
    {
    }
    
    public virtual bool TryExitState(InteractSystem _interactSystem)
    {
        return false;
    }
}
