using System;
using System.Collections;
using Enums;
using Extensions;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IInputProvider))]
public class PlayerMovement1 : MonoBehaviour
{
    private enum SwordPositions
    {
        Up,
        Mid,
        Down
    }
    
    private Rigidbody2D _rigidbody;
    private IInputProvider _inputProvider;
    private ICheck _groundCheck;
    private bool _isHolding;
    private Rigidbody2D _swordBody;
    private bool _facingRight = true;
    private bool _hasSword = true;
    private SwordPositions _swordState = SwordPositions.Mid;
    

    [Header("Movement Config")] [SerializeField]
    private float walkspeed;

    [SerializeField] private Respawn _respawn;

    [SerializeField] private float jumpForce;
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private Animation swordStabAnimation;
    [SerializeField] [NotNull] private GameObject groundCheckObject;
    [SerializeField] private GameObject sword;
    
    [SerializeField] private GameObject Adversary;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inputProvider = GetComponent<IInputProvider>();
        _groundCheck = groundCheckObject.GetComponent<ICheck>();
        _swordBody = sword.GetComponent<Rigidbody2D>();
    }

    //for engine and movement
    private void FixedUpdate()
    {
        ApplyHorizontalMovement();
        ApplyJump();
        ApplyThrowSword();
        ApplyStab();
        //ApplyBonk();
        SwitchWeaponHeight();
    }

    private void ApplyHorizontalMovement()
    {
        var inputX = _inputProvider.GetAxis(Axis.X);
        _rigidbody.SetVelocity(Axis.X, inputX * walkspeed);

        if (_inputProvider.GetAxis(Axis.X) < 0 && _facingRight)
        {
            _rigidbody.transform.Rotate(0, -180, 0);
            _facingRight = false;
        }
        else if(_inputProvider.GetAxis(Axis.X) > 0 && !_facingRight)
        {
            _rigidbody.transform.Rotate(0, -180, 0);
            _facingRight = true;
        }
    }

    private void ApplyJump()
    {
        if( IsGrounded() && _inputProvider.GetActionPressed(InputAction.Jump))
        {
            _rigidbody.SetVelocity(Axis.Y, jumpForce);
        }
    }

    private void SwitchWeaponHeight()
    {
        switch (_swordState)
        {
            case SwordPositions.Up:
                if (_inputProvider.GetActionPressed(InputAction.Down))
                {
                    _swordState = SwordPositions.Mid;
                    SwitchWeaponPosition(_swordState);
                }
                break;
            case SwordPositions.Mid:
                if (_inputProvider.GetActionPressed(InputAction.Up))
                {
                    _swordState = SwordPositions.Up;
                    SwitchWeaponPosition(_swordState);
                } else if (_inputProvider.GetActionPressed(InputAction.Down))
                {
                    _swordState = SwordPositions.Down;
                    SwitchWeaponPosition(_swordState);
                }
                break;
            case SwordPositions.Down:
                if (_inputProvider.GetActionPressed(InputAction.Up))
                {
                    _swordState = SwordPositions.Mid;
                    SwitchWeaponPosition(_swordState);
                }
                break;
        }
    }

    private void SwitchWeaponPosition(SwordPositions enu)
    {
        switch (enu)
        {
            case SwordPositions.Up:
                sword.transform.localPosition = new Vector3(1.25f, 0.5f, 0);
                //sword.transform.localRotation = new Quaternion(0, 0, -45, sword.transform.localRotation.w);
                //sword.transform.Rotate(0, 0, -45f);
                break;
            case SwordPositions.Mid:
                sword.transform.localPosition = new Vector3(1.32f, 0, 0);
                //sword.transform.localRotation = new Quaternion(0, 0, -90, sword.transform.localRotation.w);
                //sword.transform.Rotate(0, 0, -90);
                break;
            case SwordPositions.Down:
                sword.transform.localPosition = new Vector3(1.32f, -0.25f, 0);
                break;
        }
    }

    private void ApplyStab()
    {
        if (_inputProvider.GetActionPressed(InputAction.Stab))
        {
            _swordState = SwordPositions.Mid;
            swordAnimator.Play("Sword1Estoque");
        }
    }

    private void ApplyBonk()
    {
        if (_inputProvider.GetActionPressed(InputAction.Up))
        {
            _swordState = SwordPositions.Up;
            swordAnimator.Play("Sword1Attack");
        }
    }

    private void ApplyThrowSword()
    {
        if (_inputProvider.GetActionPressed(InputAction.Throw) && _hasSword)
        {
            swordAnimator.enabled = false;
            _swordBody.transform.SetParent(null);
            _swordBody.bodyType = RigidbodyType2D.Dynamic;
            _swordBody.AddForce(transform.right * 50 , ForceMode2D.Impulse);
            _hasSword = false;
            //swordAnimator.enabled = true;
        }
    }

    private bool IsGrounded()
    {
        return _groundCheck.Check();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Sword") && col.gameObject != sword)
        {
            gameObject.SetActive(false);
        }
    }
}