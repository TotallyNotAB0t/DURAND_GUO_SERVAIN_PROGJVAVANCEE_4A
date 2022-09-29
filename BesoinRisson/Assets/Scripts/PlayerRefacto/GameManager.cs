using System;
using Behaviour.InputSystems;
using Behaviour.Utils;
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
        private GameState GMBuffer;
        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;

        private void Start()
        {
            state = new GameState();
            state.p1.pos = new Vector2(-3.5f, 10.75f);
            state.p1.velocity = Vector2.zero;
            state.p2.pos = new Vector2(3.5f, 0.75f);
            _inputProvider = GetComponent<PlayerInputs>();
            _inputProvider1 = GetComponent<PlayerInputs1>();
        }
        
        private void Update()
        {
            //Create Copy GameState for checking
            //Left and right move
            ReadInput();

            //Gravity

            MyUpdate(state);
            
            player1.transform.position = state.p1.pos;
            player2.transform.position = state.p2.pos;
        }

        public void MyUpdate(GameState state)
        {
            state.p1 = SimulatePhysics(state.p1);
            state.p2 = SimulatePhysics(state.p2);
        }

        private GameState.Player SimulatePhysics(GameState.Player player)
        {
            if (CheckGround(player.pos, 1f, new Vector2(0, -0.5f), 1) && player.velocity.y < 0)
            {
                player.velocity.y = 0;
                player.pos.y = 0.5f;
                player.isGrounded = true;
                
            }
            else
            {
                player.velocity.y += -9.81f * 0.016f;
            }

            player.pos += player.velocity * 0.016f;
            player.velocity.x = 0;
            return player;
        }

        //Input function
        public void ReadInput()
        {
            if (_inputProvider.GetActionPressed(InputAction.Left))
            {
                state.p1 = applyLeft(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Left1))
            {
                state.p2 = applyLeft(state.p2);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Right))
            {
                state.p1 = applyRight(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Right1))
            {
                state.p2 = applyRight(state.p2);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Jump))
            {
                state.p1 = applyJump(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Jump1))
            {
                state.p2 = applyJump(state.p2);
            }
        }
        
        //Moving function
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

        private GameState.Player applyJump(GameState.Player player)
        {
            if (player.isGrounded)
            {
                player.velocity.y = 5;
                player.isGrounded = false;
            }

            return player;
        }
        private GameState.Player applyLeft(GameState.Player player)
        {
            player.velocity.x = -7;
            player.isRight = false;
            return player;
        }
        private GameState.Player applyRight(GameState.Player player)
        {
            player.velocity.x = 7;
            player.isRight = true;
            return player;
        }
        
        
        public bool CheckOverlap(Vector2 collider1, float radius1, Vector2 collider2, float radius2)
        {
            return Vector2.Distance(collider1, collider2) < (radius1/2)+(radius2/2);
        }
        
        public bool Check(Vector2 collider1, float radius1, Vector2 collider2, float radius2)
        {
            return Vector2.Distance(collider1, collider2) < (radius1/2)+(radius2/2);
        }
        
        public bool CheckGround(Vector2 collider1, float radius1, Vector2 ground, float size)
        {
            return Vector2.Distance(collider1, new Vector2(collider1.x, ground.y)) < (radius1/2)+(size/2);
        }

        private Vector2 ApplyGravity(GameState.Player player)
        {
            return player.pos += new Vector2(player.pos.x, player.velocity.y);
        }
    }
}