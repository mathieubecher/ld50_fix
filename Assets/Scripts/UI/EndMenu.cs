using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    private Animator m_animator;
    private LifeController m_lifeControllerRef;
    private bool m_death = false;

    public List<GameObject> fives;
    public List<GameObject> ones;
    
    public GameObject beginScore;
    public GameObject panel;
    
    
    private List<GameObject> m_score;
    
    void OnEnable()
    {
        Restart.OnReplay += Replay;
    }


    void OnDisable()
    {
        Restart.OnReplay -= Replay;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_lifeControllerRef = FindObjectOfType<LifeController>();


        m_score = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_death && m_lifeControllerRef.life <= 0)
        {
            Dead();
            m_death = true;
        }
    }
    public void Dead()
    {
        m_animator.SetTrigger("Dead");
    }
    
    public void Replay()
    {
        m_animator.SetTrigger("Reset");
        m_death = false;
        foreach (var gobject in m_score)
        {
            Destroy(gobject);
        }

        m_score = new List<GameObject>();
    }

    public void DrawScore()
    {
        int score = Head.deadHeadCount;
        Vector3 refPos = beginScore.transform.position;
        for (int i = 0; i < score / 5; ++i)
        {
            GameObject five = Instantiate(fives[Random.Range(0,fives.Count)], panel.transform);
            float width = five.GetComponent<RectTransform>().rect.width * Screen.width / 1920f;
            five.transform.position = refPos + width / 2.0f * Vector3.right;
            refPos.x += (width + 0.0f);
            m_score.Add(five);
            

        }
        
        score %= 5;
        for (int i = 0; i < score; ++i)
        {
            GameObject one = Instantiate(ones[Random.Range(0,ones.Count)], panel.transform);
            float width = one.GetComponent<RectTransform>().rect.width * Screen.width / 1920f;
            one.transform.position = refPos + width / 2.0f * Vector3.right;
            refPos.x += (width + 0.0f);
            m_score.Add(one);

        }
    }
}
