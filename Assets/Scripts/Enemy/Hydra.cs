using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hydra : MonoBehaviour
{
    [SerializeField]
    private GameObject head;

    public Transform neckPosition;
    public float neckSize = 5.0f;
    public float spawnSpeed = 1.0f;

    private float m_nbHeadToSpwan = 1.0f;
    private float m_nextTimer = 0.0f;

    public float minAngle = 90.0f;
    public float maxAngle = 190.0f;
    private int m_nbHead = 1;
    
    void OnEnable()
    {
        Restart.OnReplay += Replay;
    }


    void OnDisable()
    {
        Restart.OnReplay -= Replay;
    }
    void Update()
    {
        if (m_nextTimer > 0.0f)
        {
            m_nextTimer -= Time.deltaTime;
            if ((m_nbHead == 0 || m_nextTimer <= 0.0f) && m_nbHeadToSpwan > 1.0f)
            {
                m_nbHeadToSpwan -= 1.0f;
                Instantiate(head, transform);
                m_nbHead++;
                if (m_nbHeadToSpwan > 1.0f)
                {
                    m_nextTimer = Random.value * spawnSpeed;
                }
            }
        }
    }

    public void CreateHead()
    {
        m_nbHead--;
        if (m_nextTimer <= 0.0f)
        {
            m_nextTimer = Random.value * spawnSpeed;
        }
        m_nbHeadToSpwan += 1.2f;
    }

    public Vector3 GetValidHeadPosition(Head _head)
    {
        Debug.DrawLine(neckPosition.position + Quaternion.AngleAxis(minAngle, Vector3.forward) * Vector2.right * neckSize, neckPosition.position, Color.red, 1.0f);
        Debug.DrawLine(neckPosition.position + Quaternion.AngleAxis(maxAngle, Vector3.forward) * Vector2.right * neckSize, neckPosition.position, Color.blue,1.0f);
        Vector3 pos = neckPosition.position + Quaternion.AngleAxis(math.lerp(minAngle, maxAngle, Random.value), Vector3.forward) * Vector2.right * neckSize;
        Debug.DrawLine(pos, neckPosition.position, Color.green,1.0f);
        return pos;
    }

    public void Replay()
    {
        m_nbHead = 0;
        m_nextTimer = 1.0f;
        m_nbHeadToSpwan = 1.8f;
    }
}
