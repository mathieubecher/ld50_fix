using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Serializable]
    struct SpawnerInfo
    {
        public GameObject gameObject;
        public Vector2 positionOffset;
        public bool applyVelocity; 
        public Vector2 startVelocity;
    }
    
    [SerializeField]
    private List<SpawnerInfo> m_spawnerInfos;

    // Update is called once per frame
    public void Spawn(int _id)
    {
        if(_id >= m_spawnerInfos.Count) return;
        
        var info = m_spawnerInfos[_id];
        if (!info.gameObject) return;
        
        var instance = Instantiate(info.gameObject, transform.position + transform.rotation * info.positionOffset, transform.rotation);

        if (!info.applyVelocity) return;
        
        Rigidbody2D rb;
        if (instance.TryGetComponent(out rb))
        {
            rb.velocity = instance.transform.rotation * info.startVelocity;
        }
    }
}
