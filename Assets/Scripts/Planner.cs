using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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


        Debug.Log(leaves.Count);

        foreach (Node leaf in leaves)
        {
            Node node = leaf;
            while (node != null)
            {
                if (node.action != null)
                    Debug.Log(node.action.actionName + ", " + node.cost);
                node = node.parent;
            }
            Debug.Log("=========");
        }

        Node cheapest = null;

        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<Actions> result = new List<Actions>();
        Node n = cheapest;
        while(n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<Actions> queue = new Queue<Actions>();
        foreach(Actions a in result)
        {
            queue.Enqueue(a);
        }

        Debug.Log("plan created");
        foreach(Actions a in queue)
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

}
