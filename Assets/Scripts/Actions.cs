using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Actions : MonoBehaviour
{
    public string actionName = "Action";
    public int cost = 1;
    public State[] preConditions;
    public State[] afterEffects;

    public Transform target;
    private NavMeshAgent agent;

    public Dictionary<string, int> dict_preConditions;
    public Dictionary<string, int> effect;

    public Actions()
    {
        effect = new Dictionary<string, int>();
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

        if(afterEffects != null)
        {
            foreach(State s in afterEffects)
            {
                effect.Add(s.key, s.value);
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

    public bool AchievableGiven(Dictionary<string, int > c)
    {
        foreach(KeyValuePair<string, int> p in c)
        {
            if (!c.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }

}
