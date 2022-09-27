using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

namespace Behaviour.InputSystems
{
    public class PlayerInputs1 : MonoBehaviour, IInputProvider
    {
        private const string upButton = "Up1";
        private const string downButton = "Down1";
        private const string JumpButton = "Jump1";
        private const string StabButton = "Stab1";
        private const string ThrowButton = "Throw1";
        
        private HashSet<InputAction> _requestedActions = new HashSet<InputAction>();

        public float GetAxis(Axis axis)
        {
            return Input.GetAxisRaw(axis.ToUnityAxis1());
        }

        public bool GetActionPressed(InputAction action)
        {
            return _requestedActions.Contains(action);
        }

        private void Update()
        {
            CaptureInput();
        }

        private void CaptureInput()
        {
            if (Input.GetButtonDown(upButton))
            {
                _requestedActions.Add(InputAction.Up1);
            }
            if (Input.GetButtonUp(upButton))
            {
                _requestedActions.Remove(InputAction.Up1);
            }
            
            if (Input.GetButtonDown(downButton))
            {
                _requestedActions.Add(InputAction.Down1);
            }
            if (Input.GetButtonUp(downButton))
            {
                _requestedActions.Remove(InputAction.Down1);
            }
            
            if (Input.GetButtonDown(JumpButton))
            {
                _requestedActions.Add(InputAction.Jump1);
            }
            if (Input.GetButtonUp(JumpButton))
            {
                _requestedActions.Remove(InputAction.Jump1);
            }
            
            if (Input.GetButtonDown(StabButton))
            {
                _requestedActions.Add(InputAction.Stab1);
            }
            if (Input.GetButtonUp(StabButton))
            {
                _requestedActions.Remove(InputAction.Stab1);
            }
            
            if (Input.GetButtonDown(ThrowButton))
            {
                _requestedActions.Add(InputAction.Throw1);
            }
            if (Input.GetButtonUp(ThrowButton))
            {
                _requestedActions.Remove(InputAction.Throw1);
            }
        }
    }
}