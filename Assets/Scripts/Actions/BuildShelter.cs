using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildShelter : Actions
{
    public GameObject shelter;

    public override bool PrePreform()
    {
        return true;
    }

    public override bool PostPreform()
    {
        shelter.SetActive(true);
        WorldScript.Instance.AddWorldStates("Shelter", 1);
        return true;
    }

}
