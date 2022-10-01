using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using PlayerRefacto;
using UnityEngine;
using Random = UnityEngine.Random;

public class MCTSAgentNode
{
    // Ici on stockes les informations relatives au noeud de notre arbre prévisionnel
    public MCTSAgentNode[] children;
    public MCTSAgentNode parent;
    public GameState gamestate;
    public InputAction actionToDo;
    public uint numberWin = 0;
    public uint numberPlayed = 0;
    public float ratioWin = 0;
    public bool allChildrenFinished = false;

    public MCTSAgentNode SelectBestChild()
    {
        return children.OrderByDescending(x => x.ratioWin).First();
    }
}

public class MCTSAgent
{
    /* =========================================================
     * Melheureusement le MCTS n'a pas pu être ajouté au jeu de
     * par un probleme de temps et de refactoring d'architecture
     * qui aurais demandé encore plus de travail. Nous avons
     * laissé des commentaires a travers le code pour expliquer
     * notre chemin de pensee et expliquer les choses qui manquent
     ========================================================== */
    
    
    //Ici sont nos valeurs permettant de gérer le mcts
    private readonly uint NUMBER_SIMULATION = 30;
    private readonly static uint NUMBER_TESTS = 1000;
    private readonly float RATIO_EXPLOIT_EXPLORE = .7f;

    private MCTSAgentNode parentNode;
    private MCTSAgentNode currentNode;
    //On stockes les noeuds non finis pour des performances de recherche dans notre arbre
    //le parcours d'une liste étant plus simple et moins couteux car l'arbre n'est pas trié
    private List<MCTSAgentNode> allunfinichednodes = new List<MCTSAgentNode>();

    public MCTSAgent(MCTSAgentNode parentNode)
    {
        this.parentNode = parentNode;
        allunfinichednodes.Add(this.parentNode);
    }
    
    private InputAction ProcessLoop( GameState initGamestate)
    {
        // On copies le gameState
        currentNode.gamestate = new GameState(initGamestate);
        //Ensuite on boucle sur l nombre de tests a faire a partir du noeud "racine"
        for (int i = 0; i < NUMBER_TESTS; i ++)
        {
            //Selection par Exploration ou Exploitation
            MCTSAgentNode selectedAgentNode = AgentSelection();
            //On joue notre coup en créant un noeud enfant
            MCTSAgentNode newAgentNode = AgentExpand(selectedAgentNode);
            //On simules X parties pour avoir un nombre de victoires
            uint numberVictory = AgentSimulate(newAgentNode, NUMBER_SIMULATION);
            //et on enregistre le score
            AgentBackpropagation(newAgentNode, numberVictory, NUMBER_SIMULATION);
        }
        //Enfin on retourne le meilleur coup a faire pour la frame
        return parentNode.SelectBestChild().actionToDo;
    }

    /*
     * Selects the agent to be processed
     */
    private MCTSAgentNode AgentSelection()
    {
        // premiere iteration, le if serait a enlever pour améliorer les performances
        if (allunfinichednodes.Count == 1)
        {
            return parentNode;
        }
        //on explore ou on exploite
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
        //on créés notre noeud enfant
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
                // On a ici un failsafe au cas ou la simulation de fait une boucle infinie
                // meme si ce ne devrait pas arriver
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
            
            // il faut également verifier que notre noeud n'est pas
            // dans un état ou ses enfants sont tous joués et finis
            // pour ne pas boucler dessus alors que rien n'est faisable
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
