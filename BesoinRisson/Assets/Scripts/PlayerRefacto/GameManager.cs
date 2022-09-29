using System;
using Behaviour.InputSystems;
using Enums;
using Interfaces;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameManager : MonoBehaviour
    {
        // ici c'est la logique de jeu, tu lui passes un gamestate et un input
        private PlayerInputs _inputProvider;
        private PlayerInputs1 _inputProvider1;
        private GameState state;
        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;

        private void Start()
        {
            state = new GameState();
            state.p1.pos = new Vector2(-3.5f, 0.75f);
            state.p2.pos = new Vector2(3.5f, 0.75f);
            _inputProvider = GetComponent<PlayerInputs>();
            _inputProvider1 = GetComponent<PlayerInputs1>();
        }
        
        private void Update()
        {
            //Debug.Log($"p1 pos :{player1.transform.position}; p1 new vector : {state.p1.pos}");
            ReadInputLeftOrRight();
            player1.transform.position = Vector2.MoveTowards(player1.transform.position, state.p1.pos, Time.deltaTime*5);
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

        public void PlayerGoLeft(bool isP1)
        {
            state.ModifyPos(isP1, Vector2.left * 0.1f);
        }
        
        public void PlayerGoRight(bool isP1)
        {
            state.ModifyPos(isP1, Vector2.right * 0.1f);
        }
        
        public void ReadInputLeftOrRight()
        {
            if (_inputProvider.GetActionPressed(InputAction.Left))
            {
                PlayerGoLeft(true);
            }
            if (_inputProvider.GetActionPressed(InputAction.Left1))
            {
                PlayerGoLeft(false);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Right))
            {
                PlayerGoRight(true);
            }
            if (_inputProvider.GetActionPressed(InputAction.Right1))
            {
                PlayerGoRight(false);
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