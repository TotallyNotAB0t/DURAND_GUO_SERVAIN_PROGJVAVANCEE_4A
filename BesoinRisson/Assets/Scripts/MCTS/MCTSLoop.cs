using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Enums;
using PlayerRefacto;
using TreeEditor;
using UnityEngine;

public class MCTSAgentNode
{
    public MCTSAgentNode[] children;
    public MCTSAgentNode parent;
    public GameState gamestate;
    public InputAction actionToDo;
    public uint numberWin = 0;
    public uint numberPlayed = 0;
    public float ratioWin = 0;
    public bool allChildrenFinished = false;

    private GameState copyCurrentGame()
    {
        //recup copie du gamestate
        return null;
    }

    private MCTSAgentNode SelectBestChild()
    {
        return children.OrderByDescending(x => x.ratioWin).First();
    }
}

public class MCTSLoop
{
    private readonly uint NUMBER_SIMULATION = 30;
    private readonly static uint NUMBER_TESTS = 150;
    private readonly float RATIO_EXPLOIT_EXPLORE = .5f;

    private MCTSAgentNode parentNode;
    private MCTSAgentNode currentNode;
    private GameState startGameState;
    private List<MCTSAgentNode> allunfinichednodes = new List<MCTSAgentNode>();

    public MCTSLoop(MCTSAgentNode parentNode)
    {
        this.parentNode = parentNode;
        allunfinichednodes.Add(this.parentNode);
    }
    
    private InputAction ProcessLoop( GameState initGamestate)
    {
        // On copies le gameState
        currentNode.gamestate = new GameState(initGamestate);
        //ensuite on boucle sur l nombre de tests a faire a partir du noeud "racine"
        for (int i = 0; i < NUMBER_TESTS; i ++)
        {
            MCTSAgentNode selectedAgentNode = AgentSelection();
            MCTSAgentNode newAgentNode = AgentExpand(selectedAgentNode);
            uint numberVictory = AgentSimulate(newAgentNode, NUMBER_SIMULATION);
            AgentBackpropagation(newAgentNode, numberVictory, NUMBER_SIMULATION);
        }

        MCTSAgentNode bestChild = null;
        foreach (var child in parentNode.children)
        {
            if (bestChild == null || bestChild.ratioWin < child.ratioWin) bestChild = child;
        }

        return bestChild.actionToDo;
    }

    /*
     * Selects the agent to be processed
     */
    private MCTSAgentNode AgentSelection()
    {
        if (allunfinichednodes.Count == 1)
        {
            return parentNode;
        }

        if (Random.Range(0f, 1f) > RATIO_EXPLOIT_EXPLORE)
        {
            //Exploit
            MCTSAgentNode bestChild = null;
            foreach (var node in allunfinichednodes)
            {
                if (bestChild == null || node.ratioWin > bestChild.ratioWin)
                {
                    bestChild = node;
                }
            }
            return bestChild;
        }

        //Explore
        return allunfinichednodes[Random.Range(0, allunfinichednodes.Count)];
    }

    private MCTSAgentNode AgentExpand(MCTSAgentNode agent)
    {
        List<InputAction> actionsPossibles = agent.gamestate.CheckInputsPossible(agent.gamestate.p2, agent.gamestate.p1);
        foreach (var child in agent.children)
        {
            actionsPossibles.Remove(child.actionToDo);
        }

        MCTSAgentNode newChild = new MCTSAgentNode();
        newChild.parent = agent;
        newChild.actionToDo = actionsPossibles[Random.Range(0, actionsPossibles.Count-1)];
        newChild.gamestate = agent.gamestate.PlayAction(newChild.actionToDo);
        newChild.children = new MCTSAgentNode[newChild.gamestate.CheckInputsPossible(agent.gamestate.p2, agent.gamestate.p1).Count];
        
        agent.children[agent.children.Length] = newChild;
        return newChild;
    }

    /*
     * Simulates the playthrough X times
     */
    private uint AgentSimulate(MCTSAgentNode agent, uint numberSimulation)
    {
        uint numberWin = 0;
        for (int i = 0; i < numberSimulation; ++i)
        {
            //creer une copie du gamestate d'agent
            GameState simState = new GameState(agent.gamestate);
            uint cpt = 1000000000;
            while (!agent.gamestate.IsFinished())
            {
                List<InputAction> actions = agent.gamestate.CheckInputsPossible(agent.gamestate.p2, agent.gamestate.p1);
                InputAction inputAction = actions[Random.Range(0, actions.Count)];
                simState = simState.PlayAction(inputAction);
                cpt--;
                if (cpt == 0) break;
            }
            if (agent.gamestate.HasWon()) numberWin ++;
        }
        return numberWin;
    }

    private void AgentBackpropagation(MCTSAgentNode agent, uint numberVictory, uint NUMBER_SIMULATION)
    {
        while (agent != null)
        {
            agent.numberPlayed += NUMBER_SIMULATION;
            agent.numberWin += numberVictory;
            agent.ratioWin = agent.numberWin / agent.numberPlayed;
            if (agent.children.Length == agent.gamestate.CheckInputsPossible(agent.gamestate.p2, agent.gamestate.p1).Count)
            {
                agent.allChildrenFinished = true;
                foreach (var child in agent.children)
                {
                    if (!child.allChildrenFinished) agent.allChildrenFinished = false;
                }
            }
            else agent.allChildrenFinished = false;
            
            if (agent.allChildrenFinished) allunfinichednodes.Remove(agent);

            agent = agent.parent;
        }
    }
}
