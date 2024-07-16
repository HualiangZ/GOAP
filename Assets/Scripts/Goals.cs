using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals
{

    public Dictionary<string, int> goal;
    public Goals(string s, int i) 
    {
        goal = new Dictionary<string, int>();
        goal.Add(s, i);
    }

}
