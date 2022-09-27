using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Extensions;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IInputProvider))]
public class PlatformMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private IInputProvider _inputProvider;
    private ICheck _groundCheck;

    [Header("Movement Config")] [SerializeField]
    private float walkspeed;

    [SerializeField] private float jumpForce;
   
    [SerializeField] [NotNull] private GameObject groundCheckObject;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputProvider = GetComponent<IInputProvider>();
        _groundCheck = groundCheckObject.GetComponent<ICheck>();
    }

    //for engine and movement
    private void FixedUpdate()
    {
        ApplyHorizontalMovement();
        ApplyJump();
    }

    private void ApplyHorizontalMovement()
    {
        var inputX = _inputProvider.GetAxis(Axis.X);
        _rigidbody.SetVelocity(Axis.X, inputX * walkspeed);
    }

    private void ApplyJump()
    {
        if( IsGrounded() && _inputProvider.GetActionPressed(InputAction.Jump))
        {
            _rigidbody.SetVelocity(Axis.Y, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return _groundCheck.Check();
    }
}