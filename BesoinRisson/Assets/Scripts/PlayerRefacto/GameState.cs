using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameState
    {
        float simTickRate = 0.016f; //(0.16f = 60 fps), remplaces Time.deltaTime
        public enum SwordPos
        {
            Top,
            Mid,
            Bot
        }

        public struct Player
        {
            public Vector2 pos;
            public SwordPos swordState;
            public Vector2 swordCoord;
            public bool isGrounded;
            public bool isRight;
            public bool isIdle;
            public bool isAttacking;
            public bool isAlive;
            public bool hasWon;
            
            //jumping
            public float currentJumpSpeed;
            public float initialJumpSpeed;
            public float jumpStartTime;
            public float starty;
        }

        private float swordRadius;
        private float playerRadius;

        private float jumpForce = 20;
        private float jumpTime;
        private float gravity = -9.81f;
        private float velocity;

        public Player p1;
        public Player p2;
        
        public List<InputAction> CheckInputsPossible(Player self, Player opponent)
        {
            List<InputAction> possible = new List<InputAction>();

            if (self.hasWon || !self.isAlive || opponent.hasWon || self.isAttacking) return possible;
            
            //gauche droite?
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

        public void ModifyPos(bool isP1, Vector2 newPos)
        {
            if (isP1)
            {
                p1.pos += newPos;
            }
            else
            {
                p2.pos += newPos;
            }
        }
    }
}
