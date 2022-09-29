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
        [SerializeField] private GameObject sword1;
        [SerializeField] private GameObject sword2;

        private void Start()
        {
            state = new GameState();
            state.p1.pos = new Vector2(-3.5f, 0.75f);
            state.p1.velocity = Vector2.zero;
            state.p1.isRight = true;
            state.p1.swordState = GameState.SwordPos.Mid;
            state.p2.pos = new Vector2(3.5f, 0.75f);
            state.p2.velocity = Vector2.zero;
            state.p2.isRight = false;
            state.p2.swordState = GameState.SwordPos.Mid;
            
            _inputProvider = GetComponent<PlayerInputs>();
            _inputProvider1 = GetComponent<PlayerInputs1>();
        }
        
        private void Update()
        {
            ReadInput();

            MyUpdate(state);
            
            player1.transform.position = state.p1.pos;
            player2.transform.position = state.p2.pos;
            player1.transform.rotation = Quaternion.Euler(0, state.p1.isRight ? 0 : 180, 0);
            player2.transform.rotation = Quaternion.Euler(0, state.p2.isRight ? 0 : 180, 0);

            sword1.transform.position = state.p1.swordCoord;
            sword2.transform.position = state.p2.swordCoord;
        }

        public void MyUpdate(GameState state)
        {
            state.p1 = SimulatePhysics(state.p1);
            state.p2 = SimulatePhysics(state.p2);
            state.p1 = SwordPlacement(state.p1);
            state.p2 = SwordPlacement(state.p2);
            state.p1 = SwordStab(state.p1);
            state.p2 = SwordStab(state.p2);
        }

        private GameState.Player SimulatePhysics(GameState.Player player)
        {
            if (CheckGround(player.pos, 1f, new Vector2(0, -0.5f), 1f) && player.velocity.y < 0)
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

        private GameState.Player SwordPlacement(GameState.Player player)
        {
            if (player.isAttacking)
                return player;
            player.swordCoord.y = player.swordState switch
            {
                GameState.SwordPos.Top => player.pos.y + 0.3f,
                GameState.SwordPos.Mid => player.pos.y,
                GameState.SwordPos.Bot => player.pos.y - 0.3f,
                _ => player.pos.y
            };

            player.swordCoord.x = player.isRight ? player.pos.x + 1.8f : player.pos.x + -1.8f;
            return player;
        }
        private GameState.Player SwordStab(GameState.Player player)
        {
            if (!player.isAttacking)
            {
                return player;
            }
            if (player.isRight)
            {
                if (player.swordCoord.x < player.pos.x + 1.8f && player.swordVelocity.x < 0)
                {
                    player.swordVelocity.x = 0;
                }
                else
                {
                    player.swordVelocity.x += -300f * 0.016f;
                }
            }
            else
            {
                if (player.swordCoord.x > player.pos.x - 1.8f && player.swordVelocity.x > 0)
                {
                    player.swordVelocity.x = 0;
                }
                else
                {
                    player.swordVelocity.x += 300f * 0.016f;
                }
            }

            player.swordCoord.x += player.swordVelocity.x * 0.016f;
            player.cooldown -= 0.016f;
            if (player.cooldown < 0)
            {
                player.isAttacking = false;
            }
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
            if (_inputProvider.GetActionPressed(InputAction.Up))
            {
                state.p1 = applyUp(state.p1);
                _inputProvider.RemoveKey(InputAction.Up);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Up1))
            {
                state.p2 = applyUp(state.p2);
                _inputProvider1.RemoveKey(InputAction.Up1);
            }
            if (_inputProvider.GetActionPressed(InputAction.Down))
            {
                state.p1 = applyDown(state.p1);
                _inputProvider.RemoveKey(InputAction.Down);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Down1))
            {
                state.p2 = applyDown(state.p2);
                _inputProvider1.RemoveKey(InputAction.Down1);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Jump))
            {
                state.p1 = applyJump(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Jump1))
            {
                state.p2 = applyJump(state.p2);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Stab))
            {
                state.p1 = applyStab(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Stab1))
            {
                state.p2 = applyStab(state.p2);
            }
        }
        
        //Moving function
        private GameState.Player applyUp(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
            switch (player.swordState)
            {
                case GameState.SwordPos.Mid:
                    player.swordState = GameState.SwordPos.Top;
                    break;
                case GameState.SwordPos.Bot:
                    player.swordState = GameState.SwordPos.Mid;
                    break;
            }

            return player;
        }

        private GameState.Player applyDown(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
            switch (player.swordState)
            {
                case GameState.SwordPos.Mid:
                    player.swordState = GameState.SwordPos.Bot;
                    break;
                case GameState.SwordPos.Top:
                    player.swordState = GameState.SwordPos.Mid;
                    break;
            }

            return player;
        }

        private GameState.Player applyStab(GameState.Player player)
        {
            if (!player.isGrounded)
            {
                return player;
            }
            if (player.cooldown > 0)
            {
                return player;
            }
            player.isAttacking = true;
            player.cooldown = 0.25f;
            if (player.isRight)
            {
                player.swordVelocity.x = 30;
            }
            else player.swordVelocity.x = -30;

            return player;
        }

        private GameState.Player applyJump(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
            if (player.isGrounded)
            {
                player.velocity.y = 5;
                player.isGrounded = false;
            }

            return player;
        }
        private GameState.Player applyLeft(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
            player.velocity.x = -7;
            player.isRight = false;
            return player;
        }
        private GameState.Player applyRight(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
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
        
    }
}