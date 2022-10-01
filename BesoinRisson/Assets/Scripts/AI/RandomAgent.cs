using PlayerRefacto;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody2D))]
public class RandomAgent : MonoBehaviour
{
    [SerializeField] private GameManager GameManagerScript;

    private void FixedUpdate()
    {
        if (GameManagerScript.state.IsFinished())   
        {
            return;
        }
        
        //C'est caca mais l'agent random utilise le game manager comme il ne fait pas de predictions de jeu
        var possibleInput =
            GameManagerScript.state.CheckInputsPossible(GameManagerScript.state.p2, GameManagerScript.state.p1);
        GameManagerScript.ReadInput(possibleInput[Random.Range(0, possibleInput.Count)]);
    }
}