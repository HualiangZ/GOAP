using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actions : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1;
    public State[] preConditions;
    public State[] effects;

    public Transform target;
    private NavMeshAgent agent;

    public Dictionary<string, int> dict_preConditions;
    public Dictionary<string, int> dict_effects;

    public Actions()
    {
        dict_effects = new Dictionary<string, int>();
        dict_preConditions = new Dictionary<string, int>();
    }


    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if(preConditions != null)
        {
            foreach (State s in preConditions)
            {
                dict_preConditions.Add(s.key, s.value);
            }
        }

        if(effects != null)
        {
            foreach(State s in effects)
            {
                dict_effects.Add(s.key, s.value);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //edit as needed when inhert
    public bool CanBeAchieved()
    {
        return true;
    }

}
