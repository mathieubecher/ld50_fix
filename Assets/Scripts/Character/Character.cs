using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void EventHit(int _damage);
    public event EventHit OnHit;
    
    private Controller m_controller;
    private Rigidbody2D m_rigidBody;
    private LifeController m_lifeController;
    
    private bool m_isOnGround = false;
    private float m_timeInAir = 0f;
    
    private int m_jumpNumber = 0;
    private bool m_isJumping = false;
    private float m_jumpTimer = 0.0f;
    private float m_originGravityScale = 0.0f;
    
    private bool m_hit = false;
    private float m_invulnerableTimer = -1f;

    public Arm arm;
    public GameObject body;
    [Header("Navigation")]
    public float m_speedGround = 5.0f;
    public AnimationCurve m_jumpVerticalPosition;
    public AnimationCurve m_airControl;
    public int m_maxJump = 2;
    [Header("Hit")]
    public float m_lossControlTime = 0.2f;
    public float m_invulnerableOnHitTime = 0.6f;
    private float m_invulnerableOnTouchedTime = 0.2f;


    public GameObject doubleJumpVFX;
    
    [SerializeField]
    private Animator m_animator;
    
    public bool canJumping{get{return m_jumpNumber < m_maxJump + 1;}}
    
    
    void OnEnable()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_controller = GetComponent<Controller>();
        arm.character = this;
        
        m_controller.OnJump += ReceiveJumpInput;
        m_controller.OnAttack += ReceiveAttackInput;
        Restart.OnReplay += Replay;
    }
    void OnDisable()
    {
        m_controller.OnJump -= ReceiveJumpInput;
        m_controller.OnAttack -= ReceiveAttackInput;
        Restart.OnReplay -= Replay;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_originGravityScale = m_rigidBody.gravityScale;
        m_lifeController = GetComponent<LifeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_invulnerableTimer >= 0.0f) m_invulnerableTimer -= Time.deltaTime;
       
        Vector2 move = m_controller.moveInput;
        float direction = move.x;
        if (math.abs(direction) > 0.0f && !arm.isAttacking)
            body.transform.localScale = new Vector3(math.sign(direction), 1.0f, 1.0f);
    }

    void FixedUpdate()
    {
        Vector2 move = m_controller.moveInput;
        move.y = 0f;

        m_animator.SetBool("dead", m_lifeController.life <= 0);
        if (m_hit && (m_invulnerableTimer >= m_invulnerableOnHitTime - m_lossControlTime || m_lifeController.life <= 0)) return;
        
        if (m_isOnGround && (!m_isJumping || m_jumpTimer > 0.1f))
        {
            m_rigidBody.velocity = move * m_speedGround + m_rigidBody.velocity.y * Vector2.up;
            
            m_jumpNumber = 0;
            m_timeInAir = 0;
        }
        else
        {
            m_timeInAir += Time.deltaTime;
            if(m_timeInAir > 0.2f && m_jumpNumber == 0) m_jumpNumber = 1;
            
            float desiredHorizontalSpeed = move.x * m_speedGround;
            desiredHorizontalSpeed = math.lerp(m_rigidBody.velocity.x, desiredHorizontalSpeed, m_hit && math.abs(desiredHorizontalSpeed) < 0.1f ? 0f : m_airControl.Evaluate(m_jumpTimer));

            float desiredVerticalSpeed = m_rigidBody.velocity.y;
            
            
            if (m_isJumping)
            {
                if (m_jumpTimer + Time.deltaTime < m_jumpVerticalPosition.keys[m_jumpVerticalPosition.length - 1].time)
                {
                    m_rigidBody.gravityScale = 0.0f;
                    desiredVerticalSpeed = (m_jumpVerticalPosition.Evaluate(m_jumpTimer + Time.deltaTime) - m_jumpVerticalPosition.Evaluate(m_jumpTimer)) / Time.deltaTime;
                }
                else
                {
                    m_rigidBody.gravityScale = m_originGravityScale;
                    m_isJumping = false;
                }
                m_jumpTimer += Time.deltaTime;
            }
            m_rigidBody.velocity = desiredHorizontalSpeed * Vector2.right + desiredVerticalSpeed * Vector2.up;

        }
        m_animator.SetFloat("speed", math.abs(m_rigidBody.velocity.x));
        m_animator.SetBool("isOnGround", m_isOnGround && (!m_isJumping || m_jumpTimer > 0.1f));
    }
    private void Jump(bool reset = false)
    {
        if(reset) m_jumpNumber = 0;
        else if (!m_isOnGround)
        {
            Instantiate(doubleJumpVFX, transform.position, Quaternion.identity);
        }
        m_jumpNumber++;
        m_jumpTimer = 0f;
        m_isJumping = true;
        m_hit = false;
        m_animator.SetBool("isOnGround", false);
        m_animator.SetTrigger("Jump");
    }


    private List<Collider2D> m_grounds = new List<Collider2D>();
    void OnCollisionEnter2D(Collision2D _other)
    {
        m_isOnGround = false;
        foreach(var contact in _other.contacts)
        {
            if (Vector2.Dot(Vector2.up, contact.normal) > 0.7f)
            {
                Debug.Log("Enter " + _other.gameObject.name);
                m_grounds.Add(_other.collider);
                m_isOnGround = true;
                m_isJumping = false;
                m_hit = false;
                
                m_rigidBody.gravityScale = m_originGravityScale;
            }
        }
    }
    void OnCollisionExit2D(Collision2D _other)
    {
        if (m_grounds.Contains(_other.collider))
        {
            Debug.Log("Exit " + _other.gameObject.name);
            m_grounds.Remove(_other.collider);
            m_isOnGround = m_grounds.Count > 0;
        }
    }
    
    public bool Hit(Hitable _other, int _damage)
    {
        if (m_invulnerableTimer < 0.0f)
        {
            m_invulnerableTimer = m_invulnerableOnHitTime;
            m_rigidBody.gravityScale = m_originGravityScale;
            m_isJumping = false;
            m_hit = true;
            
            m_animator.SetTrigger("Hit");
            OnHit?.Invoke(_damage);

            Vector2 forceDir = (transform.position - _other.gameObject.transform.position).normalized;
            forceDir.y = 1.0f;
            m_rigidBody.velocity = forceDir * 15.0f;

            return true;
        }

        return false;
    }
    
    public void AttackTouched()
    {
        if(!m_isOnGround)
            Jump(true);

        m_invulnerableTimer = m_invulnerableOnTouchedTime;
    }
    
    public void AttackTouchedWall()
    {
        
    }

    void ReceiveAttackInput()
    {
        if (m_lifeController.life <= 0) return;
        arm.Attack(m_controller.armDirection);
    }
    
    void ReceiveJumpInput()
    {
        if (m_lifeController.life <= 0) return;
        if (m_isOnGround || (m_jumpNumber < m_maxJump && m_jumpTimer > 0.1f))
        {
            Jump();
        }
    }
    
    private void Replay()
    {
        transform.position = FindObjectOfType<RespawnPoint>().transform.position;
        transform.rotation = Quaternion.identity;
    }
    
}
