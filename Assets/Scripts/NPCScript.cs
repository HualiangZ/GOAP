using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public States NPCStates;
    public List<Actions> actions = new List<Actions>();
    public Dictionary<Goals, int> goals = new Dictionary<Goals, int>();

    private Queue<Actions> actionQ;
    private Goals currentGoal;
    public Actions currentAction;
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
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
