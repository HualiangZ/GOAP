using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goals
{

    public Dictionary<string, int> dict_startCondition;
    public Goals(string s, int i) 
    {
        dict_startCondition = new Dictionary<string, int>();
        dict_startCondition.Add(s, i);
    }

}
