using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogText : MonoBehaviour
{
    public Text m_text;
    // Start is called before the first frame update
    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    public void SetText(string _text)
    {
        m_text.enabled = true;
        m_text.text = _text;
    }

    public void Disable()
    {
        m_text.enabled = false;
    }
}
