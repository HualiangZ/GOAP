using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals
{

    public Dictionary<string, int> goal;
    public bool canBeRemoved;
    public Goals(string s, int i, bool r) 
    {
        goal = new Dictionary<string, int>();
        goal.Add(s, i);
        canBeRemoved = r;
    }



}
