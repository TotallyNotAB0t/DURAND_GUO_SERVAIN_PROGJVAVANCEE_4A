using Behaviour.InputSystems;
using Enums;
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
            
            //Dire a l'epee de suivre les joueurs
            state.p1.swordCoord = Vector2.one;
            state.p2.swordCoord = new Vector2(2, 5);
            
            _inputProvider = GetComponent<PlayerInputs>();
            _inputProvider1 = GetComponent<PlayerInputs1>();
        }
        
        private void Update()
        {
            MyUpdate(state);
            
            //Apply gamestate to the graphic engine
            player1.transform.position = state.p1.pos;
            player2.transform.position = state.p2.pos;
        }

        public void MyUpdate(GameState objState)
        {
            //Read player and bot moves
            ReadInput();
            objState.p1 = SimulatePhysics(objState.p1);
            objState.p2 = SimulatePhysics(objState.p2);
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
            
            CheckWin();
            ApplyKill();

            player.pos += player.velocity * 0.016f;
            player.velocity.x = 0;
            return player;
        }

        //Input function
        public void ReadInput(InputAction action = InputAction.Up)
        {
            if (_inputProvider.GetActionPressed(InputAction.Left))
            {
                state.p1 = ApplyLeft(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Left1) || action == InputAction.Left1)
            {
                state.p2 = ApplyLeft(state.p2);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Right))
            {
                state.p1 = ApplyRight(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Right1) || action == InputAction.Right1)
            {
                state.p2 = ApplyRight(state.p2);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Jump))
            {
                state.p1 = ApplyJump(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Jump1) || action == InputAction.Jump1)
            {
                state.p2 = ApplyJump(state.p2);
            }
        }
        
        //Moving function
        private void ApplyUp(GameState.Player player)
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

        private void ApplyDown(GameState.Player player)
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

        private GameState.Player ApplyJump(GameState.Player player)
        {
            if (player.isGrounded)
            {
                player.velocity.y = 5;
                player.isGrounded = false;
            }

            return player;
        }
        private GameState.Player ApplyLeft(GameState.Player player)
        {
            //if (CheckOverlap(state.p1.swordCoord, 0.25f, state.p2.swordCoord, 0.25f)) return player;
            player.velocity.x = -7;
            player.isRight = false;

            return player;
        }
        
        private GameState.Player ApplyRight(GameState.Player player)
        {
            //if (CheckOverlap(state.p1.swordCoord, 0.25f, state.p2.swordCoord, 0.25f)) return player;
            player.velocity.x = 7;
            player.isRight = true;
            return player;
        }

        private void CheckWin()
        {
            if (state.p1.pos.x > 18)
            {
                Debug.LogError("P1 WON");
                state.p1.hasWon = true;
            } else if (state.p2.pos.x < -18)
            {
                Debug.LogError("P2 WON");
                state.p2.hasWon = true;
            }
        }

        private void ApplyKill()
        {
            if (CheckOverlap(state.p1.swordCoord, 0.25f, state.p2.pos, 0.5f))
            {
                state.p2.pos = new Vector2(state.p1.pos.x + 2f, 0.75f);
            }
            else if (CheckOverlap(state.p2.swordCoord, 0.25f, state.p1.pos, 0.5f))
            {
                state.p1.pos = new Vector2(state.p2.pos.x - 2f, 0.75f);
            }
            
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

        /*private Vector2 ApplyGravity(GameState.Player player)
        {
            return player.pos += new Vector2(player.pos.x, player.velocity.y);
        }*/
    }
}