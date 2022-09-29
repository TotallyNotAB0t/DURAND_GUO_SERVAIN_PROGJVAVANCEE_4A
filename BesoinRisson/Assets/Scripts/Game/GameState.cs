using System;
using System.Collections.Generic;
using Behaviour.Utils;
using Enums;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameState : MonoBehaviour
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

        [SerializeField] private float swordRadius;
        [SerializeField] private float playerRadius;
        
        [SerializeField] private float jumpForce = 20;
        [SerializeField] private float jumpTime;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float velocity;

        private Player p1;
        private Player p2;
        

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
        
        //ici c'est du game manager
        public void readInputs(InputAction input)
        {
            if (input.Equals(InputAction.Up))
            {
                 applyUp(p1);
            }
            else if (input.Equals(InputAction.Up1))
            {
                applyUp(p2);
            }
            else if (input.Equals(InputAction.Down))
            {
                applyDown(p1);
            }
            else if (input.Equals(InputAction.Down1))
            {
                applyDown(p2);
            }
            else if (input.Equals(InputAction.Stab))
            {
                applyStab(p1, p2);
            }
            else if (input.Equals(InputAction.Stab1))
            {
                applyStab(p2, p1);
            }
            else if (input.Equals(InputAction.Jump))
            {
                applyJump(p1);
            }
            else if (input.Equals(InputAction.Jump1))
            {
                applyJump(p2);
            }
        }

        private void applyUp(Player player)
        {
            switch (player.swordState)
            {
                case SwordPos.Mid:
                    player.swordState = SwordPos.Top;
                    break;
                case SwordPos.Bot:
                    player.swordState = SwordPos.Mid;
                    break;
            }
        }

        private void applyDown(Player player)
        {
            switch (player.swordState)
            {
                case SwordPos.Mid:
                    player.swordState = SwordPos.Bot;
                    break;
                case SwordPos.Top:
                    player.swordState = SwordPos.Mid;
                    break;
            }
        }

        private void applyStab(Player player, Player rival)
        {
            player.isAttacking = true;
            //avancer epee
            var memo = player.swordCoord;
            if (player.isRight)
            {
                player.swordCoord += Vector2.right;
            }
            else player.swordCoord += Vector2.left;
            //checker collision epee/rival
            if (CheckOverlap(player.swordCoord, swordRadius, rival.pos, playerRadius))
            {
                
            }
            //reculer epee
            player.swordCoord = memo;
        }

        private void applyJump(Player player)
        {
            player.isGrounded = false;
            player.currentJumpSpeed = player.initialJumpSpeed;
            player.starty = player.pos.y;
            player.pos = Vector2.MoveTowards(player.pos, player.pos + new Vector2(player.pos.x, jumpForce), Time.deltaTime);
        }
        
        
        public bool CheckOverlap(Vector2 collider1, float radius1, Vector2 collider2, float radius2)
        {
            return Vector2.Distance(collider1, collider2) < (radius1/2)+(radius2/2);
        }
    }
    
    
}