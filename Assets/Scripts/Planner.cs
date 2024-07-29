using System.Collections;
using System.Collections.Generic;
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
public class Planner : MonoBehaviour
{
    public WorldScript world;
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
        Node start = new Node(null, 0, world.GetWorldState().GetStates(), null);

        bool success = BuildGraph(start, leaves, useableAction, goal);

        if (!success)
        {
            Debug.Log("No plan");
            return null;
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
        return queue;
    }
}
