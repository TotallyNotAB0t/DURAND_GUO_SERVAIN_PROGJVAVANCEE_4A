using System;
using Enums;
using Interfaces;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameManager : MonoBehaviour
    {
        // ici c'est la logique de jeu, tu lui passes un gamestate et un input
        private IInputProvider _inputProvider;
        [SerializeField] private GameState state;
        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;

        private void Start()
        {
            _inputProvider = GetComponent<IInputProvider>();
        }
        
        private void Update()
        {
            Debug.Log($"p1 pos :{player1.transform.position}; p1 new vector : {state.p1.pos}");
            ReadInputLeftOrRight();
            player1.transform.position = state.p1.pos;
        }
        
        //Moving function
        
        
        
        
        
        
        //Input function
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //ici c'est du game manager
        /*public void readInputs(InputAction input, GameState.Player p1)
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
        }*/

        private void applyUp(GameState.Player player)
        {
            switch (player.swordState)
            {
                case GameState.SwordPos.Mid:
                    player.swordState = GameState.SwordPos.Top;
                    break;
                case GameState.SwordPos.Bot:
                    player.swordState = GameState.SwordPos.Mid;
                    break;
            }
        }

        private void applyDown(GameState.Player player)
        {
            switch (player.swordState)
            {
                case GameState.SwordPos.Mid:
                    player.swordState = GameState.SwordPos.Bot;
                    break;
                case GameState.SwordPos.Top:
                    player.swordState = GameState.SwordPos.Mid;
                    break;
            }
        }

        public void PlayerGoLeft(GameState.Player player)
        {
            player.pos += Vector2.left;
        }
        
        public void PlayerGoRight(GameState.Player player)
        {
            player.pos += Vector2.right;
        }

        public void ReadInputLeftOrRight()
        {
            if (_inputProvider.GetActionPressed(InputAction.Left))
            {
                PlayerGoLeft(state.p1);
            }

            if (_inputProvider.GetActionPressed(InputAction.Left1))
            {
                PlayerGoLeft(state.p2);
            }

            if (_inputProvider.GetActionPressed(InputAction.Right))
            {
                PlayerGoRight(state.p1);
            }

            if (_inputProvider.GetActionPressed(InputAction.Right1))
            {
                PlayerGoRight(state.p2);
            }
        }

        /*private void applyStab(GameState.Player player, GameState.Player rival)
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
        }*/

        /*private void applyJump(GameState.Player player)
        {
            player.isGrounded = false;
            player.currentJumpSpeed = player.initialJumpSpeed;
            player.starty = player.pos.y;
            player.pos = Vector2.MoveTowards(player.pos, player.pos + new Vector2(player.pos.x, jumpForce), Time.deltaTime);
        }*/
        
        
        public bool CheckOverlap(Vector2 collider1, float radius1, Vector2 collider2, float radius2)
        {
            return Vector2.Distance(collider1, collider2) < (radius1/2)+(radius2/2);
        }
    }
}