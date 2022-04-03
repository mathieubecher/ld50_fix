using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Camera mainCam;
    
    public delegate void Jump();
    public event Jump OnJump;
    
    public delegate void Attack();
    public event Attack OnAttack;
    
    [HideInInspector]
    public Vector2 moveInput;

    public Vector2 armDirection;
    
    void Start()
    {
        mainCam = Camera.main;
    }
    
    void Update()
    {
    }

    public void ReadMoveInput(InputAction.CallbackContext _context)
    {
        moveInput = _context.ReadValue<float>() * Vector2.right;
    }
    
    public void ReadDirectionInput(InputAction.CallbackContext _context)
    {
        armDirection = _context.ReadValue<Vector2>();
    }

    public void ReadJumpInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
            OnJump?.Invoke();
    }
    
    
    public void ReadAttackInput(InputAction.CallbackContext _context)
    {
        if (_context.performed)
            OnAttack?.Invoke();
    }
}
