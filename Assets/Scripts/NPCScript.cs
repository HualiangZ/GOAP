using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public States NPCStates;
    // Start is called before the first frame update
    void Start()
    {
        NPCStates = new States();
        NPCStates.AddState("Hunger", 100);
        NPCStates.AddState("Thirst", 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
