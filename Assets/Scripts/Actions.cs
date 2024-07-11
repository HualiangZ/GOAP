using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public int cost = 0;
    public State[] preConditions;
    public State[] effects;

    public Transform target;

    public Dictionary<string, int> dict_preConditions;
    public Dictionary<string, int> dict_effects;



    // Start is called before the first frame update
    void Start()
    {
        dict_effects = new Dictionary<string, int>();
        dict_preConditions = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
