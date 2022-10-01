using Behaviour.InputSystems;
using Enums;
using TMPro;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameManager : MonoBehaviour
    {
        /*
         * Cette classe aurait du etre un singleton ou l'on aurais pu recuperer
         * son instance pour faire des appels dessus voire meme faire une classe
         * statique, mais ce ne sonnes pas comme une super idee
         */
        
        private PlayerInputs _inputProvider;
        private PlayerInputs1 _inputProvider1;
        public GameState state;
        [SerializeField] private GameObject player1;
        [SerializeField] private GameObject player2;
        [SerializeField] private GameObject sword1;
        [SerializeField] private GameObject sword2;
        
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _rival;
        [SerializeField] private GameObject _uiWinScreen;
        [SerializeField] private TextMeshProUGUI _winnerText;

        private void Start()
        {
            Time.timeScale = 1f;
            state = new GameState();
            state.timer = 25f;
            state.p1.pos = new Vector2(-3.5f, 0.75f);
            state.p1.velocity = Vector2.zero;
            state.p1.isRight = true;
            state.p1.swordState = GameState.SwordPos.Mid;
            state.p1.isAlive = true;
            state.p2.pos = new Vector2(3.5f, 0.75f);
            state.p2.velocity = Vector2.zero;
            state.p2.isRight = false;
            state.p2.swordState = GameState.SwordPos.Mid;
            state.p2.isAlive = true;
            
            _inputProvider = GetComponent<PlayerInputs>();
            _inputProvider1 = GetComponent<PlayerInputs1>();
        }
        
        private void FixedUpdate()
        {
            // Pour le JcJ
            ReadInput();
            //Ici on geres les entrées due notre agent
            MyUpdate(state);
            
            // On utilise le gamestate pour appliquer les changements a l'ecran
            player1.transform.position = state.p1.pos;
            player2.transform.position = state.p2.pos;
            player1.transform.Find("Sprite").transform.rotation = Quaternion.Euler(0, state.p1.isRight ? 0 : 180, 0);
            player2.transform.Find("Sprite").transform.rotation = Quaternion.Euler(0, state.p2.isRight ? 0 : 180, 0);

            sword1.transform.position = state.p1.swordCoord;
            sword2.transform.position = state.p2.swordCoord;
            
            //Win applying to the scene
            if (state.p1.hasWon)
            {
                _winnerText.text = $"{_player.name} won!";
                _uiWinScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else if (state.p2.hasWon)
            {
                _winnerText.text = $"{_rival.name} won!";
                _uiWinScreen.SetActive(true);
                Time.timeScale = 0;
            }
            else if (state.IsFinished())
            {
                _winnerText.text = "DRAW";
                _uiWinScreen.SetActive(true);
                Time.timeScale = 0;
            }
        }

        public GameState MyUpdate(GameState objState, InputAction inputAction = InputAction.Idle, float tickRate = 0.016f)
        {
            //Read player and bot moves
            if(inputAction != InputAction.Idle)
                ReadInput(inputAction);
            objState.p1 = SimulatePhysics(objState.p1, tickRate);
            objState.p2 = SimulatePhysics(objState.p2, tickRate);
            objState.p1 = SimulatePhysics(objState.p1, tickRate);
            objState.p2 = SimulatePhysics(objState.p2, tickRate);
            objState.p1 = SwordPlacement(objState.p1, tickRate);
            objState.p2 = SwordPlacement(objState.p2, tickRate);
            objState.p1 = SwordStab(objState.p1, tickRate);
            objState.p2 = SwordStab(objState.p2, tickRate);
            objState.timer -= tickRate;

            return objState;
        }

        private GameState.Player SimulatePhysics(GameState.Player player, float tickRate = 0.016f)
        {
            if (CheckGround(player.pos, 1f, new Vector2(0, -0.5f), 1f) && player.velocity.y < 0)
            {
                //ici on s'assure de ne pas passer sous le sol
                player.velocity.y = 0;
                player.pos.y = 0.5f;
                player.isGrounded = true;
            }
            else
            {
                player.velocity.y += -9.81f * tickRate;
            }
            
            CheckWin();
            ApplyKill();

            player.pos += player.velocity * tickRate;
            player.velocity.x = 0;
            return player;
        }

        private GameState.Player SwordPlacement(GameState.Player player, float tickRate = 0.016f)
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

            player.swordCoord.x = player.isRight ? player.pos.x + 1.9f : player.pos.x + -1.9f;
            return player;
        }
        private GameState.Player SwordStab(GameState.Player player, float tickRate = 0.016f)
        {
            if (!player.isAttacking)
            {
                return player;
            }
            if (player.isRight)
            {
                if (player.swordCoord.x < player.pos.x + 1.9f && player.swordVelocity.x < 0)
                {
                    player.swordVelocity.x = 0;
                }
                else
                {
                    player.swordVelocity.x += -300f * tickRate;
                }
            }
            else
            {
                if (player.swordCoord.x > player.pos.x - 1.9f && player.swordVelocity.x > 0)
                {
                    player.swordVelocity.x = 0;
                }
                else
                {
                    player.swordVelocity.x += 300f * tickRate;
                }
            }

            player.swordCoord.x += player.swordVelocity.x * tickRate;
            player.cooldown -= tickRate;
            if (player.cooldown < 0)
            {
                player.isAttacking = false;
            }
            return player;
        }

        //Input function, surement le doAction
        public void ReadInput(InputAction action = InputAction.Idle)
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
            if (_inputProvider.GetActionPressed(InputAction.Up))
            {
                state.p1 = ApplyUp(state.p1);
                _inputProvider.RemoveKey(InputAction.Up);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Up1) || action == InputAction.Up1)
            {
                state.p2 = ApplyUp(state.p2);
                _inputProvider1.RemoveKey(InputAction.Up1);
            }
            if (_inputProvider.GetActionPressed(InputAction.Down))
            {
                state.p1 = ApplyDown(state.p1);
                _inputProvider.RemoveKey(InputAction.Down);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Down1) || action == InputAction.Down1)
            {
                state.p2 = ApplyDown(state.p2);
                _inputProvider1.RemoveKey(InputAction.Down1);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Jump))
            {
                state.p1 = ApplyJump(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Jump1) || action == InputAction.Jump1)
            {
                state.p2 = ApplyJump(state.p2);
            }
            
            if (_inputProvider.GetActionPressed(InputAction.Stab))
            {
                state.p1 = ApplyStab(state.p1);
            }
            if (_inputProvider1.GetActionPressed(InputAction.Stab1) || action == InputAction.Stab1)
            {
                state.p2 = ApplyStab(state.p2);
            }
        }
        
        //Moving function
        private GameState.Player ApplyUp(GameState.Player player)
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

        private GameState.Player ApplyDown(GameState.Player player)
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

        private GameState.Player ApplyStab(GameState.Player player)
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

        private GameState.Player ApplyJump(GameState.Player player)
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
        private GameState.Player ApplyLeft(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
            //if (CheckOverlap(state.p1.swordCoord, 0.25f, state.p2.swordCoord, 0.25f)) return player;
            player.velocity.x = -7;
            player.isRight = false;

            return player;
        }
        
        private GameState.Player ApplyRight(GameState.Player player)
        {
            if (player.isAttacking)
            {
                return player;
            }
            //if (CheckOverlap(state.p1.swordCoord, 0.25f, state.p2.swordCoord, 0.25f)) return player;
            player.velocity.x = 7;
            player.isRight = true;
            return player;
        }

        private void CheckWin()
        {
            if (state.p1.pos.x > 18 || !state.p2.isAlive)
            {
                state.p1.hasWon = true;
            } else if (state.p2.pos.x < -18 || !state.p1.isAlive)
            {
                state.p2.hasWon = true;
            }
        }

        private void ApplyKill()
        {
            if (CheckOverlap(state.p1.swordCoord, 0.25f, state.p2.pos, 1f) && state.p1.isAttacking)
            {
                if (CheckHeight(state.p1, state.p2))
                {
                    return;
                }
                state.p2.isAlive = false;
            }
            else if (CheckOverlap(state.p2.swordCoord, 0.25f, state.p1.pos, 1f) && state.p2.isAttacking)
            {
                if (CheckHeight(state.p1, state.p2))
                {
                    return;
                }
                state.p1.isAlive = false;
            }
        }

        private bool CheckHeight(GameState.Player p1, GameState.Player p2)
        {
            return p1.swordState == p2.swordState;
        }

        public bool CheckOverlap(Vector2 collider1, float radius1, Vector2 collider2, float radius2)
        {
            return Vector2.Distance(collider1, collider2) < (radius1/2)+(radius2/2);
        }

        public bool CheckGround(Vector2 collider1, float radius1, Vector2 ground, float size)
        {
            return Vector2.Distance(collider1, new Vector2(collider1.x, ground.y)) < (radius1/2)+(size/2);
        }
    }
}