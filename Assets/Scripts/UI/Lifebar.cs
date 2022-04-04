using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    private Image m_image;

    public List<Sprite> lifeValues;
    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
    }

    public void SetLife(int life)
    {
        m_image.sprite = lifeValues[math.clamp(life, 0 , lifeValues.Count-1)];
    }
}
