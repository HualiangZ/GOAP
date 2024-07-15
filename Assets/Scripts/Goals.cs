using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals : MonoBehaviour
{

    public float priority = 0;
    public State[] startCondition;

    public Dictionary<string, int> dict_startCondition;


    public void Awake()
    {
        if (startCondition != null)
        {
            foreach (State s in startCondition)
            {
                dict_startCondition.Add(s.key, s.value);
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
}
