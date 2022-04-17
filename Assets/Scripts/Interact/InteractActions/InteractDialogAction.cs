using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDialogAction : InteractDefaultAction
{
    public string dialog = "";
    
    public override void Interact(InteractSystem _interactSystem)
    {
        _interactSystem.uiText.SetText(dialog);
    }
    
    public override bool TryExitState(InteractSystem _interactSystem)
    {
        _interactSystem.uiText.Disable();
        return true;
    }
}
