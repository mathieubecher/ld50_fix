using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Head : MonoBehaviour
{
    public enum HeadState
    {
        Idle,
        Origin,
        Attack,
        Recover
    }

    public OriginState m_originState;
    public IdleState m_idleState;
    public AttackState m_attackState;
    public RecoveryState m_recoveryState;
    
    [SerializeField]
    private float m_randomMoveSpeed = 2.0f;
    [SerializeField]
    public float m_randomMoveSize = 0.5f;
    
    private Character m_target;
    private Hydra m_parent;

    private float m_lifeTimer;
    
    private float m_randomMoveOffset;
    public HeadState m_state = HeadState.Origin;
    
    // Start is called before the first frame update
    void Start()
    {
        m_lifeTimer = -Random.value * 5.0f;
        
        m_target = FindObjectOfType<Character>();
        m_parent = FindObjectOfType<Hydra>();
        transform.position = m_parent.neckPosition.position;
        m_originState.originPos = transform.position + Quaternion.AngleAxis(math.lerp(90.0f, 220.0f, Random.value), Vector3.forward) * Vector2.right * m_parent.neckSize;

        m_randomMoveOffset = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        m_lifeTimer += Time.deltaTime;
        Vector3 position = m_originState.originPos;
        switch (m_state)
        {
            case HeadState.Origin:
                position = OriginStateBehaviour(position);
                break;
            case HeadState.Idle:
                position = IddleStateBehaviour(position);
                break;
            case HeadState.Attack:
                position = AttackStateBehaviour(position);
                break;
            case HeadState.Recover:
                position = RecoveryStateBehaviour(position);
                break;
        }
        

        if (m_state == HeadState.Idle || m_state == HeadState.Origin)
        {
            position += Vector3.up * math.sin((m_lifeTimer + m_randomMoveOffset) * m_randomMoveSpeed) * m_randomMoveSize;
            position += Vector3.right * math.cos((m_lifeTimer + m_randomMoveOffset) * m_randomMoveSpeed) * m_randomMoveSize;
        }
        transform.position = position;
    }

    [Serializable]
    public struct OriginState
    {
        public float originDuration;
        [HideInInspector]
        public Vector2 originPos;
    }
    private Vector3 OriginStateBehaviour(Vector3 _position)
    {
        Vector3 position = _position;
        
        if (m_lifeTimer < m_originState.originDuration)
        {
            position = Vector3.Lerp(m_parent.neckPosition.position, m_originState.originPos, math.min(m_lifeTimer / m_originState.originDuration, 1.0f));
        }
        else RequestIdle();

        return position;
    }
    
    [Serializable]
    public struct IdleState
    {
        public float idleDuration;
        [HideInInspector]
        public float idleTimer;
    }
    private void RequestIdle()
    {
        m_state = HeadState.Idle;
        m_idleState.idleTimer = m_idleState.idleDuration;
    }

    private Vector3 IddleStateBehaviour(Vector3 _position)
    {
        if (m_idleState.idleTimer > 0.0f)
        {
            m_idleState.idleTimer -= Time.deltaTime;
        }
        else
        {
            RequestAttack();
        }
        
        return _position;
        
    }
    
    
    [Serializable]
    public struct AttackState
    {
        public float waitingDuration;
        public float attackDuration;
        public float maxAttackDistance;
        [HideInInspector]
        public float attackTimer;
        [HideInInspector]
        public Vector2 attackPos;
    }
    private void RequestAttack()
    {
        m_state = HeadState.Attack;
        m_attackState.attackTimer = 0.0f;
        m_attackState.attackPos = m_target.transform.position;
        float distanceRatio = m_attackState.maxAttackDistance / (m_attackState.attackPos - (Vector2)m_parent.neckPosition.position).magnitude;
        m_attackState.attackPos = Vector3.Lerp(m_parent.neckPosition.position, m_attackState.attackPos, math.min(distanceRatio, 1.0f));
    }
    private Vector3 AttackStateBehaviour(Vector3 _position)
    {
        Vector3 position = _position;
        m_attackState.attackTimer += Time.deltaTime;
        if (m_attackState.attackTimer > m_attackState.waitingDuration)
        {
            float lerpValue = (m_attackState.attackTimer - m_attackState.waitingDuration) / m_attackState.attackDuration;
            position = Vector3.Lerp(m_originState.originPos, m_attackState.attackPos, math.min(lerpValue, 1.0f));
            if (lerpValue > 1.0f) RequestRecovery();
        }
        return position;
    }

    [Serializable]
    public struct RecoveryState
    {
        public float waitingDuration;
        public float recoveryDuration;
        [HideInInspector]
        public float recoveryTimer;
    }
    private void RequestRecovery()
    {
        m_state = HeadState.Recover;
        m_recoveryState.recoveryTimer = 0.0f;
    }
    private Vector3 RecoveryStateBehaviour(Vector3 _position)
    {
        Vector3 position = m_attackState.attackPos;
        m_recoveryState.recoveryTimer += Time.deltaTime;
        if (m_recoveryState.recoveryTimer > m_recoveryState.waitingDuration)
        {
            float lerpValue = (m_recoveryState.recoveryTimer - m_recoveryState.waitingDuration) / m_recoveryState.recoveryDuration;
            
            position = Vector3.Lerp(m_attackState.attackPos, m_originState.originPos, math.min(lerpValue, 1.0f));
            if (lerpValue > 1.0f) RequestIdle();
        }
        return position;
    }
    
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (!_other.isTrigger)
        {
            _other.GetComponent<Character>().Hit(this);
        }
        
    }

    public void Hit()
    {
        m_parent.CreateHead();
        Destroy(gameObject);
    }
}
