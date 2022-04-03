using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void EventAttackTouched();
    public event EventAttackTouched OnAttackTouched;
    public delegate void EventHit();
    public event EventHit OnHit;
    
    private Controller m_controller;
    private Rigidbody2D m_rigidBody;
    
    private bool m_isOnGround = false;
    
    private int m_jumpNumber = 0;
    private bool m_isJumping = false;
    private float m_jumpTimer = 0.0f;
    private float m_originGravityScale = 0.0f;
    
    private bool m_hit = false;
    private float m_invulnerableTimer = -1.0f;

    public Arm arm;
    public GameObject body;
    [Header("Navigation")]
    public float m_speedGround = 5.0f;
    public AnimationCurve m_jumpVerticalPosition;
    public AnimationCurve m_airControl;
    public int m_maxJump = 2;
    [Header("Hit")]
    public float m_lossControlTime = 0.2f;
    public float m_invulnerableTime = 0.6f;
    
    public bool canJumping{get{return m_jumpNumber < m_maxJump + 1;}}
    
    
    void OnEnable()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_controller = GetComponent<Controller>();
        
        m_controller.OnJump += ReceiveJumpInput;
        m_controller.OnAttack += ReceiveAttackInput;
    }
    void OnDisable()
    {
        m_controller.OnJump -= ReceiveJumpInput;
        m_controller.OnAttack -= ReceiveAttackInput;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_originGravityScale = m_rigidBody.gravityScale;
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
        
        if (m_invulnerableTimer >= m_invulnerableTime - m_lossControlTime)
        {
            
        }
        else if (m_isOnGround && (!m_isJumping || m_jumpTimer > 0.1f))
        {
            m_rigidBody.velocity = move * m_speedGround + m_rigidBody.velocity.y * Vector2.up;
            m_jumpNumber = 0;
        }
        else
        {
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
    }


    void OnCollisionEnter2D(Collision2D _other)
    {
        m_isOnGround = false;
        foreach(var contact in _other.contacts)
        {
            if (Vector2.Dot(Vector2.up, contact.normal) > 0.9f)
            {
                
                m_isOnGround = true;
                m_isJumping = false;
                m_hit = false;
                m_rigidBody.gravityScale = m_originGravityScale;
            }
        }
    }

    void OnCollisionExit2D()
    {
        m_isOnGround = false;
        m_jumpNumber = 1;
    }

    public void Hit(Head _other)
    {
        if (m_invulnerableTimer < 0.0f)
        {
            m_invulnerableTimer = m_invulnerableTime;
            m_rigidBody.gravityScale = m_originGravityScale;
            m_isJumping = false;
            m_hit = true;
            
            OnHit?.Invoke();

            Vector2 forceDir = (transform.position - _other.gameObject.transform.position).normalized;
            forceDir.y = 1.0f;
            m_rigidBody.velocity = forceDir * 15.0f;
        }
    }

    public void AttackTouched()
    {
        if(!m_isOnGround)
            Jump(true);
        
        OnAttackTouched?.Invoke();
    }

    private void Jump(bool reset = false)
    {
        if(reset) m_jumpNumber = 0;
        m_jumpNumber++;
        m_jumpTimer = 0f;
        m_isJumping = true;
        m_hit = false;
    }
    
    void ReceiveAttackInput()
    {
        arm.Attack(m_controller.armDirection);
    }
    
    void ReceiveJumpInput()
    {
        //Debug.Log("Jump");
        if (m_isOnGround || (m_jumpNumber < m_maxJump && m_jumpTimer > 0.1f))
        {
            Jump();
        }
    }
}
