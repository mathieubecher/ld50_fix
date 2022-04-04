using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NeckRenderer : MonoBehaviour
{
    private LineRenderer m_lineRenderer;
    private Hydra m_hydra;
    private Head m_head;

    [SerializeField]
    private float m_headDirectionWeight = 7f;
    [SerializeField]
    private float m_baseDirectionWeight = 5f;
    [SerializeField]
    private int nbPoint = 15;
    
    [SerializeField]
    private AnimationCurve m_deadCurve;
    
    private float m_deadTimer;
    private float m_distance;
    // Start is called before the first frame update
    void Start()
    {
        m_hydra = FindObjectOfType<Hydra>();
        m_lineRenderer = GetComponent<LineRenderer>();
        m_head = GetComponent<Head>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_head.life > 0) DrawLivingNeck();
        else DrawDeadNeck();
    }

    private void DrawDeadNeck()
    {
        m_deadTimer += Time.deltaTime;
        float ratio = m_deadCurve.Evaluate(m_deadTimer);
        if (ratio < 0.01f)
        {
            Destroy(gameObject);
            return;
        }

        int resolution = nbPoint * 10;
        m_lineRenderer.positionCount = resolution;
        float reverseRatio = 1.0f - ratio;
        float size = m_distance * ratio;

        Vector3[] listPositions = new Vector3[resolution];
        
        Vector2 direction = (transform.position - m_hydra.neckPosition.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        for (int i = 0; i < resolution; ++i)
        {
            float posX = i / (float)resolution * size;
            float amplitude = i / (float)resolution * ratio * 2.0f;

            float waveCount = (i/(float)resolution) * 2 *  math.PI;
            float posY = math.sin(waveCount + m_deadTimer * 20.0f) * amplitude;
            Vector2 pos = Quaternion.Euler(0f,0f, angle) * new Vector2(posX, posY) + m_hydra.neckPosition.position;
            
            listPositions[resolution - 1 - i] = pos;
        }
        m_lineRenderer.SetPositions(listPositions);
    }

    private void DrawLivingNeck()
    {
        m_lineRenderer.positionCount = nbPoint;
        Vector3 P3 = m_hydra.neckPosition.position;
        Vector3 P0 = transform.position;
        m_distance = (P0 - P3).magnitude;
        Vector3 P2 = P3 + -m_hydra.neckPosition.right * m_baseDirectionWeight * m_distance;
        Vector3 P1 = P0 + Vector3.right * m_headDirectionWeight * m_distance;
        
        Vector3[] listPositions = new Vector3[nbPoint];
        for (int i = 0; i < nbPoint; ++i)
        {
            float t = i / (float) (nbPoint-1);
            Vector3 pos = P0 * math.pow(1 - t, 3) + 
                          3 * P1 * t * math.pow(1 - t, 2) +
                          3 * P2 * math.pow(t, 2) * (1 - t) + 
                          P3 * math.pow(t, 3);
            
            listPositions[i] = pos;
        }
        
        m_lineRenderer.SetPositions(listPositions);
    }
}
