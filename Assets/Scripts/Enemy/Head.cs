
using UnityEngine;
using Random = UnityEngine.Random;

public class Head : Hitable
{
    
    public static int headSpawnCount = 0;
    public static int deadHeadCount = 0;
    
    public int life = 2;
    private Animator m_animator;
    [HideInInspector]
    public Hydra hydra;
    
    public GameObject deadHead;
    public GameObject deadHead2;
    public GameObject asset;
    public SpriteRenderer headAsset;

    private float m_lifeTimer;
    private Vector3 m_previousPos;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LineRenderer>().sortingOrder = headSpawnCount * 2 + 1;
        headSpawnCount++;
        headAsset.sortingOrder = headSpawnCount * 2;
            
        m_lifeTimer = 0f;
        
        hydra = FindObjectOfType<Hydra>();
        
        
        m_animator = GetComponent<Animator>();
        m_previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_lifeTimer += Time.deltaTime;

        
    }

    void LateUpdate()
    {
        m_previousPos = transform.position;
    }
    
    
    public override void Hit(Vector3 _direction)
    {
        life--;
        if (life <= 0)
        {
            hydra.CreateHead();
            var deadHeadInstance = Instantiate(Random.value > 0.5f?deadHead : deadHead2, transform.position, transform.rotation);
            deadHeadInstance.GetComponent<Rigidbody2D>().velocity = (transform.position - hydra.neckPosition.position).normalized * 20.0f;
            Destroy(asset);
            Destroy(m_animator);

            foreach (var collider in GetComponents<Collider2D>())
            {
                Destroy(collider);
            }
            deadHeadCount++;
            GetComponent<Collider2D>().enabled = false;

        }
        else m_animator.SetTrigger("Hit");
    }

    public Vector3 GetVelocity()
    {
        return (transform.position - m_previousPos) / Time.deltaTime;
    }

    public void Spawn(GameObject _gameObject)
    {
        Instantiate(_gameObject, transform.position, transform.rotation);
    }
}
