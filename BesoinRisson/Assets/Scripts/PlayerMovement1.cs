using Enums;
using Extensions;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IInputProvider))]
public class PlayerMovement1 : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private IInputProvider _inputProvider;
    private ICheck _groundCheck;
    private bool _isHolding;
    private Rigidbody2D _swordBody;
    private bool _facingRight = true;
    

    [Header("Movement Config")] [SerializeField]
    private float walkspeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private Animator swordAnimator;
    [SerializeField] [NotNull] private GameObject groundCheckObject;
    [SerializeField] private GameObject sword;


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

    private void ApplyThrowSword()
    {
        if (_inputProvider.GetActionPressed(InputAction.Throw))
        {
            _swordBody.transform.SetParent(null);
            _swordBody.bodyType = RigidbodyType2D.Dynamic;
            _swordBody.AddForce(transform.forward * 10, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return _groundCheck.Check();
    }
}