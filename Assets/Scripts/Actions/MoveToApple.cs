using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToApple : Actions
{
    // Start is called before the first frame update
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
        NPC.NPCStates.ModifyStates("Hunger", 30);
        return true;
    }
}
