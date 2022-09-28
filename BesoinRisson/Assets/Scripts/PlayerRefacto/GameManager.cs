using System;
using Enums;
using UnityEngine;

namespace PlayerRefacto
{
    public class GameManager : MonoBehaviour
    {
        // ici c'est la logique de jeu, tu lui passes un gamestate et un input

        [SerializeField] private GameState state;
        [SerializeField] private GameObject player1;

        private void Update()
        {
            Debug.Log($"p1 pos :{player1.transform.position}; p1 new vector : {state.p1.pos}");
            state.applyLeftOrRight(state.p1, InputAction.Left);
            player1.transform.position = state.p1.pos;
        }
    }
}