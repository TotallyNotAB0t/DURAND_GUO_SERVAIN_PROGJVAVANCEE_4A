using Enums;
using Extensions;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IInputProvider))]
public class PlayerMovement2 : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private IInputProvider _inputProvider;
    private ICheck _groundCheck;

    [Header("Movement Config")] [SerializeField]
    private float walkspeed;

    [SerializeField] private Respawn _respawn;
    [SerializeField] private PlayerMovement1 _player1State;
    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject sword;

    [SerializeField] [NotNull] private GameObject groundCheckObject;
    [SerializeField] private GameObject Adversary;


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

        if (_inputProvider.GetAxis(Axis.X) < 0)
        {
            _rigidbody.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(_inputProvider.GetAxis(Axis.X) > 0)
        {
            _rigidbody.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    private void ApplyJump()
    {
        if( IsGrounded() && _inputProvider.GetActionPressed(InputAction.Jump1))
        {
            _rigidbody.SetVelocity(Axis.Y, jumpForce);
        }
    }
    
    private bool IsGrounded()
    {
        return _groundCheck.Check();
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Sword") && !_player1State.IsBlocking && !_player1State.IsSwordDown)
        {
            gameObject.SetActive(false);
        }
    }
}