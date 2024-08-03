using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public States NPCStates;
    public List<Actions> actions = new List<Actions>();
    public Dictionary<Goals, int> goals = new Dictionary<Goals, int>();

    private Queue<Actions> actionQ;
    private Goals currentGoal;
    public Actions currentAction;
    private Planner planner;
    bool ran = false;
    // Start is called before the first frame update
    void Start()
    {

        NPCStates = new States();
        Actions[] act = GetComponents<Actions>();
        foreach (Actions a in act) 
        {
            actions.Add(a);
        }

        NPCStates.AddState("Hunger", 100);
        NPCStates.AddState("Thirst", 100);

        Goals g = new Goals("Fish", 1);
        goals.Add(g, 1);

        Goals g1 = new Goals("Apple", 1);
        goals.Add(g1, 1);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(currentAction != null && currentAction.isRunning)
        {
            if(currentAction.agent.hasPath && currentAction.agent.remainingDistance < 1f)
            {
                if (!ran)
                {
                    Invoke("IsActionComplete", currentAction.waitDuration);
                    ran = true;
                }
            }
            return;
        }

        if(planner == null || actionQ == null)
        {
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            planner = new Planner();

            foreach (KeyValuePair<Goals, int> g in sortedGoals)
            {
                actionQ = planner.plan(actions, g.Key.goal, null);
                if (actionQ != null)
                {
                    currentGoal = g.Key;
                    break;
                }
            }
        }

        if(actionQ != null && actionQ.Count == 0)
        {
            planner = null;
        }

        if(actionQ != null && actionQ.Count > 0)
        {
            currentAction = actionQ.Dequeue();
            if (currentAction.PrePreform())
            {
                if(currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindGameObjectWithTag(currentAction.targetTag);
                }
                if(currentAction.target != null)
                {
                    currentAction.isRunning = true;
                    currentAction.agent.SetDestination(currentAction.target.transform.position);
                }
            }
            else
            {
                actionQ = null;
            }
        }

    }

    void IsActionComplete()
    {
        currentAction.isRunning = false;
        currentAction.PostPreform();
        ran = false;
    }
}
