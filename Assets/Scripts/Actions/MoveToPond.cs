using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPond : Actions
{
    NPCScript NPC;
    public void Start()
    {
        NPC = GetComponent<NPCScript>();
    }
    public override bool PrePreform()
    {
        return true;
    }

    public override bool PostPreform()
    {
        NPC.NPCStates.ModifyStates("Thirst", 30);
        return true;
    }

}
