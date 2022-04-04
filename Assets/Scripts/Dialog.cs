using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : Interactible
{
    public List<GameObject> dialogs;
    public Transform baseDialogPoint;

    private GameObject m_currentDialog;

    private int m_currentDialogId = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        //Debug.Log("Interact");
        m_currentDialogId++;
        if(m_currentDialog) Destroy(m_currentDialog);
        if(m_currentDialogId < dialogs.Count) m_currentDialog = Instantiate(dialogs[m_currentDialogId], baseDialogPoint);
    }

    public override void EnterTriggerZone()
    {
        base.EnterTriggerZone();
        if(m_currentDialogId >= dialogs.Count) return;
        
        if(m_currentDialog) Destroy(m_currentDialog);
        m_currentDialog = Instantiate(dialogs[m_currentDialogId], baseDialogPoint);

    }

    public override void ExitTriggerZone()
    {
        base.ExitTriggerZone();
        
    }
}
