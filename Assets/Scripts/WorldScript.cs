using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    public States worldStates;
    // Start is called before the first frame update
    void Start()
    {
        worldStates = new States();
        GetAllTrees();
        GetAllPond();
        //Debug.Log(worldStates.HasState("Pond"));
        //Debug.Log(worldStates.GetStateValue("Pond"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetAllTrees()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        worldStates.AddState("Tree", trees.Length);
    }
    private void GetAllPond()
    {
        GameObject[] pond = GameObject.FindGameObjectsWithTag("Pond");
        worldStates.AddState("Pond", pond.Length);
    }

    public States GetWorldState()
    {
        return worldStates;
    }
}
