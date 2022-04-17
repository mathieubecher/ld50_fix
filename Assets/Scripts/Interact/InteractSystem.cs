using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractSystem : Interactible
{
    public DialogText uiText;
    public InteractListAction actions ;

    private bool finish = false;
    
    public override void Interact()
    {
        if (finish) return;
        if (!uiText) uiText = FindObjectOfType<DialogText>();
        
        finish = actions.TryExitState(this);
        if(!finish) actions.Interact(this);
        
        Debug.Log("Interact");
    }

    public override void EnterTriggerZone(Character _character)
    {
        base.EnterTriggerZone(_character);
    }

    public override void ExitTriggerZone()
    {
        base.ExitTriggerZone();
    }
}

[CustomEditor(typeof(InteractSystem))]
public class InteractSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InteractSystem interactSystem = (InteractSystem)target;
        if(!interactSystem.actions) interactSystem.actions =  CreateInstance<InteractListAction>();

        bool changed = DrawAction(null, interactSystem.actions);
        

        if (changed)
        {
            
            EditorUtility.SetDirty(interactSystem);
            EditorSceneManager.MarkSceneDirty(interactSystem.gameObject.scene);
        }
        
    }
    
    private bool DrawAction(InteractListAction _parent, InteractListAction _action)
    {
        bool changed = DrawActionWindow(_parent, _action, "Default");
        foreach (var interaction in _action.listActions)
        {
            switch (interaction)
            {
                case InteractListAction listAction:
                    changed |= DrawAction(_action, listAction);
                    break;
                case InteractDialogAction dialogAction:
                    changed |= DrawAction(_action, dialogAction);
                    break;
                default:
                    changed |= DrawAction(_action, interaction);
                    break;
            }

            if (changed) break;
        }

        if (GUILayout.Button("Add dialog"))
        {
            InteractDialogAction action = CreateInstance<InteractDialogAction>();
            _action.Add(action);
            changed = true;
        }
        return changed;
    }

    private bool DrawAction(InteractListAction _parent, InteractDefaultAction _action)
    {
        bool changed = DrawActionWindow(_parent, _action, "Default");
        if (_action.enable) 
        {
            EditorGUILayout.HelpBox("This text should not appear, please call a better programmer to help you.", MessageType.Info);
        }

        return changed;
    }
    private bool DrawAction(InteractListAction _parent, InteractDialogAction _action)
    {

        string name = "Dialog : " + _action.dialog.Split('\n')[0];
        bool changed = DrawActionWindow(_parent, _action, name.Substring(0, math.min(name.Length, 40)));
        if (_action.enable)
        {
             string dialog = EditorGUILayout.TextArea(_action.dialog);
             changed |= dialog != _action.dialog;
             _action.dialog = dialog;
        }

        return changed;
    }
    
    
    private bool DrawActionWindow(InteractListAction _parent, InteractDefaultAction _action, string _text)
    {
        if (!_parent) return false;
        
        bool changed = false;
        GUILayout.BeginHorizontal();
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        GUI.skin.button.margin = new RectOffset(0,0,0,0);
        if (GUILayout.Button(_text))
        {
            changed = true;
            _action.enable = !_action.enable;
        }
        
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        GUI.skin.button.fixedWidth = 20;
        
        if (GUILayout.Button("v"))
        {
            _parent.MoveDown(_action);
            changed = true;
        }
        
        if (GUILayout.Button("^"))
        {
            _parent.MoveUp(_action);
            changed = true;
        }
        if (GUILayout.Button("x"))
        {
            _parent.Remove(_action);
            changed = true;
        }
        
        GUI.skin.button.fixedWidth = 0;
        
        GUILayout.EndHorizontal();
        return changed;
    }

}