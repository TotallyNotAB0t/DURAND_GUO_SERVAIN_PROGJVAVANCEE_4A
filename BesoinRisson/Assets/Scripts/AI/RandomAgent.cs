using System.Collections;
using System.Collections.Generic;
using Enums;
using Extensions;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomAgent : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private ICheck _groundCheck;
    private bool _isHolding;
    private Rigidbody2D _swordBody;
    private bool _facingRight = false;
    

    [Header("Movement Config")] [SerializeField]
    private float walkspeed;

    [SerializeField] private float jumpForce;
    [SerializeField] private Animator swordAnimator;
    [SerializeField] [NotNull] private GameObject groundCheckObject;
    [SerializeField] private GameObject sword;

    //0 = left
    //1 = right
    //2 = up
    //3 = down
    //4 = jump
    //5 = stab
    //6 = throw
    private bool[] ActionList = new []{false, false, false, false, false, false, false};

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundCheck = groundCheckObject.GetComponent<ICheck>();
        _swordBody = sword.GetComponent<Rigidbody2D>();
        StartCoroutine(selectRandomAction());
    }

    private IEnumerator selectRandomAction()
    {
        while (true)
        {
            int random = Random.Range(0, ActionList.Length);
            switch (random)
            {
                case 0:
                    ActionList[1] = false;
                    break;
                case 1:
                    ActionList[0] = false;
                    break;
                case 2:
                    ActionList[3] = false;
                    break;
                case 3:
                    ActionList[2] = false;
                    break;
                default:
                    break;
            }

            ActionList[random] = !ActionList[random];
            
            yield return new WaitForSeconds(0.2f);
        }
        
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
        float inputX = 0f;
        if (ActionList[0])
        {
            inputX = -1;
        }

        if (ActionList[1])
        {
            inputX = 1;
        }
        _rigidbody.SetVelocity(Axis.X, inputX * walkspeed);

        if (inputX < 0 && _facingRight)
        {
            _rigidbody.transform.Rotate(0, -180, 0);
            _facingRight = false;
        }
        else if(inputX > 0 && !_facingRight)
        {
            _rigidbody.transform.Rotate(0, -180, 0);
            _facingRight = true;
        }
    }

    private void ApplyJump()
    {
        if( IsGrounded() && ActionList[4])
        {
            _rigidbody.SetVelocity(Axis.Y, jumpForce);
        }
    }

    private void ApplyThrowSword()
    {
        if (ActionList[6])
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