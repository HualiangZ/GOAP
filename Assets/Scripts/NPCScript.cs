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

    private bool rain = false;

    private Queue<Actions> actionQ;
    private Goals currentGoal;
    public Actions currentAction;
    private Planner planner;
    bool ran = false;

    Goals g1 = new Goals("Fish", 1, true);
    Goals g2 = new Goals("Dry", 1, true);
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
        InvokeRepeating("DecreaseHunger", 1.0f, 1.0f);
        NPCStates.AddState("Thirst", 100);
        InvokeRepeating("DecreaseThirst", 1.0f, 1.0f);
        NPCStates.AddState("Dry", 100);

        Goals g = new Goals("Shelter", 1, true);

        goals.Add(g, 3);
        goals.Add(g1, 2);
        goals.Add(g2, 1);

    }
    private void Update()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(currentAction != null && currentAction.isRunning)
        {
            float distance = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            if(currentAction.agent.hasPath && distance < 2f)
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
            if (currentGoal.canBeRemoved)
            {
                goals.Remove(currentGoal);
            }
            else
            {
                if (goals.ContainsKey(currentGoal))
                {
                    if (goals[currentGoal] != 0)
                    {
                        goals[currentGoal] -= 1;
                    }
                }
            }
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

    private void DecreaseHunger()
    {
        if (NPCStates.GetStateValue("Hunger") > 0)
        {
            NPCStates.ModifyStates("Hunger", -1);
            Debug.Log(NPCStates.GetStateValue("Hunger"));
        }

    }

    private void DecreaseThirst()
    {
        if (NPCStates.GetStateValue("Thirst") > 0)
        {
            NPCStates.ModifyStates("Thirst", -1);
            Debug.Log(NPCStates.GetStateValue("Thirst"));
        }

    }

    void IsActionComplete()
    {
        currentAction.isRunning = false;
        currentAction.PostPreform();
        ran = false;
    }
}
