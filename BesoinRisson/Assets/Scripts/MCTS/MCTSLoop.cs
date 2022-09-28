using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class TreeNode
{
    public TreeNode[] root;
    private GameState state;
    
    private void Selection()
    {
        Node treenode = this;
        while (!state.Win())
        {
            treenode.Expand();
            TreeNode child = treenode.Select();
        }
    }

    private void Expand()
    {
        root = new TreeNode[state.GetPossibleActions()];
        for (int i = 0; i < state.GetPossibleActions().Count(); i++)
        {
            root[i] = new TreeNode()
        }
    }

    Select();
}

public class MCTSLoop
{
    private readonly int NUMBER_SIMULATION = 10;

    private Node currentNode;

    private void Expansion()
    {
        Node = state.Get
    }
    
    private void MCTSSimulation()
    {
        InputAction bestAction;
        foreach (var possibleActions in state.GetPossibleActions())
        {
            Tree tree = new Tree();
        }
    }

    private int SimulateResult(BotFunction action)
    {
        //DoTheFunction
        return Function.Result();
        
    }
}
