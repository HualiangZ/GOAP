using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public sealed class WorldScript
{
    private static States worldStates = new States();
    private static readonly WorldScript worldScript;

    static WorldScript()
    {
        worldScript = new WorldScript();
    }
    private WorldScript()
    {
        //GetAllTrees();
        //GetAllPond();
    }

    public static WorldScript Instance
    {
        get { return worldScript; }
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
