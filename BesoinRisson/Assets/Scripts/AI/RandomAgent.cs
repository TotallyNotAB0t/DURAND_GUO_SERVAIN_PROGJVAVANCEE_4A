using System;
using System.Collections;
using Enums;
using Extensions;
using Interfaces;
using JetBrains.Annotations;
using PlayerRefacto;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody2D))]
public class RandomAgent : MonoBehaviour
{
    [SerializeField] private GameManager GameManagerScript;

    private void FixedUpdate()
    {
        var possibleInput =
            GameManagerScript.state.CheckInputsPossible(GameManagerScript.state.p2, GameManagerScript.state.p1);
        GameManagerScript.ReadInput(possibleInput[Random.Range(0, possibleInput.Count)]);
        
    }

}