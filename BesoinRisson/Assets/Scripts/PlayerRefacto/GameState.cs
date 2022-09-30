using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameState
    {
        public float simTickRate = 0.016f; //(0.16f = 60 fps), remplaces Time.deltaTime
        public enum SwordPos
        {
            Top,
            Mid,
            Bot
        }

        public struct Player
        {
            public Vector2 pos;
            public Vector2 velocity;
            public SwordPos swordState;
            public Vector2 swordCoord;
            public Vector2 swordVelocity;
            public float cooldown;
            public bool isGrounded;
            public bool isRight;
            public bool isIdle;
            public bool isAttacking;
            public bool isAlive;
            public bool hasWon;
        }

        //public bool isFinished;
        public float timer;
        private float swordRadius;
        private float playerRadius;

        private float jumpForce = 20;
        private float jumpTime;
        private float gravity = -9.81f;
        private float velocity = 0;

        public Player p1;
        public Player p2;

        public GameState()
        {
            
        }
        
        //Copy constructor
        public GameState(GameState previousGamestate)
        {
            swordRadius = previousGamestate.swordRadius;
            playerRadius = previousGamestate.playerRadius;
            jumpForce = previousGamestate.jumpForce;
            gravity = previousGamestate.gravity;
            velocity = previousGamestate.velocity;
            
            p1 = previousGamestate.p1;
            p2 = previousGamestate.p2;
        }
        
        public List<InputAction> CheckInputsPossible(Player self, Player opponent)
        {
            List<InputAction> possible = new List<InputAction>();

            if (self.hasWon || !self.isAlive || opponent.hasWon || self.isAttacking)
            {   
                possible.Add(InputAction.Idle);
                return possible;
            }
            
            possible.Add(InputAction.Left1);
            possible.Add(InputAction.Right1);
            possible.Add(InputAction.Up1);
            possible.Add(InputAction.Down1);
            possible.Add(InputAction.Jump1);
            possible.Add(InputAction.Stab1);
            //possible.Add(InputAction.Throw1);

            if (!self.isGrounded)
            {
                possible.Remove(InputAction.Jump1);
                possible.Remove(InputAction.Stab1);
            }

            return possible;
        }

        public bool IsFinished()
        {
            return p1.hasWon || p2.hasWon || timer < 0 || !p1.isAlive || !p2.isAlive;
        }

        public bool HasWon()
        {
            return p2.hasWon;
        }
        
        //decoy
        public GameState PlayAction(InputAction input)
        {
            return null;
        }
    }
}
