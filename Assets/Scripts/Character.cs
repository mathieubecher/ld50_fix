using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Controller m_controller;
    private Rigidbody2D m_rigidBody;
    
    private bool m_isOnGround = false;
    
    private int m_jumpNumber = 0;
    private float m_jumpTimer = 0.0f;
    private float m_originGravityScale = 0.0f;

    public Arm arm;
    public GameObject body;
    public float m_speedGround = 5.0f;
    public AnimationCurve m_airControl;
    public int m_maxJump = 2;
    
    public bool isJumping{get{return m_jumpNumber < m_maxJump + 1;}}
    
    public AnimationCurve m_jumpVerticalPosition;
    
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
        
        Vector2 move = m_controller.moveInput;
        float direction = move.x;
        if (math.abs(direction) > 0.0f && !arm.isAttacking)
            body.transform.localScale = new Vector3(math.sign(direction), 1.0f, 1.0f);
    }

    void FixedUpdate()
    {
        Vector2 move = m_controller.moveInput;
        move.y = 0f;
        
        if (m_isOnGround && (!isJumping || m_jumpTimer > 0.1f))
        {
            m_rigidBody.velocity = move * m_speedGround + m_rigidBody.velocity.y * Vector2.up;
            m_jumpNumber = 0;
        }
        else
        {
            float desiredHorizontalSpeed = move.x * m_speedGround;
            desiredHorizontalSpeed = math.lerp(m_rigidBody.velocity.x, desiredHorizontalSpeed, m_airControl.Evaluate(m_jumpTimer));

            float desiredVerticalSpeed = m_rigidBody.velocity.y;
            if (isJumping)
            {
                if (m_jumpTimer + Time.deltaTime < m_jumpVerticalPosition.keys[m_jumpVerticalPosition.length - 1].time)
                {
                    m_rigidBody.gravityScale = 0.0f;
                    desiredVerticalSpeed = (m_jumpVerticalPosition.Evaluate(m_jumpTimer + Time.deltaTime) - m_jumpVerticalPosition.Evaluate(m_jumpTimer)) / Time.deltaTime;
                }
                else
                {
                    m_rigidBody.gravityScale = m_originGravityScale;
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
                m_rigidBody.gravityScale = m_originGravityScale;
            }
        }
    }

    void OnCollisionExit2D()
    {
        m_isOnGround = false;
    }

    void ReceiveJumpInput()
    {
        //Debug.Log("Jump");
        if (m_isOnGround || (m_jumpNumber < m_maxJump && m_jumpTimer > 0.1f))
        {
            m_jumpNumber++;
            m_jumpTimer = 0f;
        }
    }

    void ReceiveAttackInput()
    {
        arm.Attack(m_controller.armDirection);
        
    }
}
