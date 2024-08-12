using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public Actions action;

 
    public Node (Node parent, float cost, Dictionary<string, int> state, Actions action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int> (state);
        this.action = action;
    }
}
public class Planner
{
    public Queue<Actions> plan(List<Actions> actions, Dictionary<string, int> goal, State state)
    {
        List<Actions> useableAction = new List<Actions>();

        foreach (Actions a in actions)
        {
            if (a.CanBeAchieved())
            {
                useableAction.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, WorldScript.Instance.GetWorldState().GetStates(), null);

        bool success = BuildGraph(start, leaves, useableAction, goal);

        if (!success)
        {
            Debug.Log("No plan");
            return null;
        }



        List<List<Actions>> tree = new List<List<Actions>>();

        foreach(Node leaf in leaves)
        {
            List<Actions> result = new List<Actions>();
            Node n = leaf;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }
                n = n.parent;
            }
            tree.Add(result);
        }

        List<Actions> cheapest = Dijkstra(tree);



        /* ==== testing code ==== */
/*        Debug.Log(tree.Count);

        foreach (List<Actions> l in tree)
        {
            foreach(Actions a in l)
            {
                Debug.Log(a.actionName);
            }
            Debug.Log("============");
        }*/
        /* ================== */

        Queue<Actions> queue = new Queue<Actions>();

        foreach (Actions a in cheapest)
        {
            Debug.Log(a.actionName + ", " + a.cost);  
            queue.Enqueue(a);
        }

        //Debug.Log("plan created");
        foreach (Actions a in queue)
        {
            //Debug.Log(a.actionName);
        }
        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<Actions> useableActions, Dictionary<string, int> goal)
    {
        bool foundGraph = false;

        foreach(Actions a in  useableActions)
        {
            if (a.AchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(KeyValuePair<string, int> eff in a.effect)
                {
                    if (!currentState.ContainsKey(eff.Key))
                    {
                        currentState.Add(eff.Key, eff.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + a.cost, currentState, a);
                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundGraph = true;
                }
                else
                {
                    List<Actions> subset = ActionsSubset(useableActions, a);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundGraph = true;
                    }
                }

            }
        }
        return foundGraph;
    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<Actions> ActionsSubset(List<Actions> actions, Actions removeA)
    {
        List<Actions> subset = new List<Actions>();
        foreach(Actions a in actions)
        {
            if (!a.Equals(removeA))
            {
                subset.Add(a);
            }
        }
        return subset;
    }

    private List<Actions> Dijkstra(List<List<Actions>> tree)
    {
        //Debug.Log(tree.Count);

        List<Actions> cheapest = null;
        float cheapestCost = Mathf.Infinity;
        foreach (List<Actions> l in tree)
        {
            //Debug.Log("================");
            float cost = 0;
            foreach (Actions a in l)
            {
                //Debug.Log(a.actionName + ", " + a.cost);
                cost += a.cost;
            }
            //Debug.Log(cost);
            //Debug.Log("================");
            if (cost < cheapestCost)
            {
                //Debug.Log("cheaper");
                cheapestCost = cost;
                cheapest = l;
            }
        }

        return cheapest;
    }

    private List<Actions> AStar()
    {
        List<Actions> cheapest = null;
        Dictionary<Actions, int> OPEN = new Dictionary<Actions, int>();
        Dictionary<Actions, int> CLOSED = new Dictionary<Actions, int>();



        return cheapest;
    }
}
