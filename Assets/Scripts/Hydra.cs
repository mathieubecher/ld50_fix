using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : MonoBehaviour
{
    [SerializeField]
    private GameObject head;

    public Transform neckPosition;
    public float neckSize = 5.0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CreateHead()
    {
        Instantiate(head, transform);
        Instantiate(head, transform);
    }
}
