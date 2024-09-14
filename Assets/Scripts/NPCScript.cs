using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public States NPCStates;
    public List<Actions> actions = new List<Actions>();
    public Dictionary<Goals, int> goals = new Dictionary<Goals, int>();
    public GameObject waitingArea;
    public GameObject shelter;
    public bool rain;

    private Queue<Actions> actionQ;
    private Goals currentGoal;
    public Actions currentAction;
    public NavMeshAgent agent;
    private Planner planner;
    bool ran = false;

    string fileName = Application.dataPath + "/dataA.csv";
    TextWriter tw;

    Goals g1 = new Goals("Fish", 1, true);
    Goals g2 = new Goals("Apple", 1, true);
    Goals g = new Goals("Dry", 1, true);
    // Start is called before the first frame update
    void Start()
    {
       tw = new StreamWriter(fileName);
        rain = false;
        NPCStates = new States();
        agent = GetComponent<NavMeshAgent>();
        Actions[] act = GetComponents<Actions>();
        foreach (Actions a in act) 
        {
            actions.Add(a);
        }
        NPCStates.AddState("Hunger", 55);
        InvokeRepeating("DecreaseHunger", 1.0f, 1.0f);
        NPCStates.AddState("Thirst", 100);
        InvokeRepeating("DecreaseThirst", 1.0f, 1.0f);
        NPCStates.AddState("Dry", 55);
        InvokeRepeating("DecreaseDry", 1.0f, 1.0f);
        Invoke("StartRain", 10f);
        Invoke("StopRain", 300f);
    }
    private void Update()
    {
        if (goals.Count == 0)
        {
            agent.SetDestination(waitingArea.transform.position);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(currentAction != null && currentAction.isRunning)
        {
            float distance = Vector3.Distance(currentAction.target.transform.position, this.transform.position);
            if(distance < 1f)
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

            if (planner.timer != null)
            {
                Debug.Log(planner.timer);
                tw.WriteLine(planner.timer);
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

    private void OnApplicationQuit()
    {
        tw.Flush();
        tw.Close();
    }

    private void DecreaseHunger()
    {
        if (NPCStates.GetStateValue("Hunger") > 0)
        {
            NPCStates.ModifyStates("Hunger", -1);
            //Debug.Log(NPCStates.GetStateValue("Hunger"));
        }
        if (NPCStates.GetStateValue("Hunger") < 50)
        {
            if (goals.ContainsKey(g2))
            {
                goals[g2] += 1;
            }
            else
            {
                goals.Add(g2, 1);
            }
             
        }

    }

    private void DecreaseThirst()
    {
        if (NPCStates.GetStateValue("Thirst") > 0)
        {
            NPCStates.ModifyStates("Thirst", -1);
            //Debug.Log(NPCStates.GetStateValue("Thirst"));
        }
        if (NPCStates.GetStateValue("Thirst") < 50)
        {
            if (goals.ContainsKey(g1))
            {
                 goals[g1] += 1;
            }
            else
            {
                goals.Add(g1, 1);  
            }
           
        }

    }

    void StartRain()
    {
        rain = true;
    }

    private void StopRain()
    {
        rain = false;
    }

    private void DecreaseDry()
    {
        if(rain)
        {
            if (NPCStates.GetStateValue("Dry") > 0)
            {
                NPCStates.ModifyStates("Dry", -2);
                
            }
            float distance = Vector3.Distance(shelter.transform.position, this.transform.position);
            if (distance <= 1f && shelter.activeSelf)
            {
                NPCStates.ModifyStates("Dry", 6);
                try
                {
                    goals[g] -= 5;
                }
                catch
                {

                }
                
            }
            if (NPCStates.GetStateValue("Dry") < 50)
            {
                if (goals.ContainsKey(g))
                {
                    goals[g] += 1;
                }
                else
                {
                    goals.Add(g, 1);
                }

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
