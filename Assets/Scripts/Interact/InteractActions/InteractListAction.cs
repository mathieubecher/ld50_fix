using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractListAction : InteractDefaultAction
{
    public List<InteractDefaultAction> listActions = new List<InteractDefaultAction>();
    private int m_position = 0;
    private InteractDefaultAction m_activeAction;
    
    public override void Interact(InteractSystem _interactSystem)
    {
        m_activeAction = listActions[m_position];
        m_activeAction.Interact(_interactSystem);
    }

    public override bool TryExitState(InteractSystem _interactSystem)
    {
        if (m_activeAction && !m_activeAction.TryExitState(_interactSystem)) return false;
        if(m_activeAction) ++m_position;
        return m_position >= listActions.Count;
    }

    public void Add(InteractDefaultAction _action)
    {
        listActions.Add(_action);
    }

    public void Remove(InteractDefaultAction _action)
    {
        listActions.Remove(_action);
    }

    public void MoveUp(InteractDefaultAction _action)
    {
        int id = listActions.IndexOf(_action);
        if (id == 0) return;
        listActions[id] = listActions[id - 1];
        listActions[id - 1] = _action;


    }

    public void MoveDown(InteractDefaultAction _action)
    {
        
        int id = listActions.IndexOf(_action);
        if (id + 1 == listActions.Count) return;
        
        listActions[id] = listActions[id + 1];
        listActions[id + 1] = _action;


    }
}
