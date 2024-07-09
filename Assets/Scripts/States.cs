using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State
{
    public string key;
    public int value;
}

public class States 
{ 
    public Dictionary<string, int> state;

    public States() 
    { 
        state = new Dictionary<string, int>();
    }

    public void AddState(string s, int i)
    {
        state.Add(s, i);
    }

    public bool HasState(string s)
    {
        return state.ContainsKey(s);
    }

    public void RemoveState(string s) 
    {
        state.Remove(s);
    }

    public void SetStateValue(string s, int i)
    {
        if (HasState(s))
        {
            state[s] = i;
        }
        else
        {
            AddState(s, i);
        }
    }
    
    public void ModifyStates(string s, int i)
    {
        if (HasState(s))
        {
            state[s] += i;
        }
        else
        {
            AddState(s, i);
        }

    }

    public int GetStateValue(string s) 
    {
        return state[s];
    }
}

