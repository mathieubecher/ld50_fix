using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField]
    private Character m_character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeHit()
    {
        m_character.InvokeHit();
    }
    
    public void Spawn(GameObject _gameObject)
    {
        Instantiate(_gameObject, transform.position, transform.rotation);
    }
}
