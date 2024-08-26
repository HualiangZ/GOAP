using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text hunger;
    public TMP_Text thirst;
    public TMP_Text rainBool;
    public TMP_Text dry;
    public NPCScript NPC;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NPC != null)
        {
            hunger.text = NPC.NPCStates.GetStateValue("Hunger").ToString();
            thirst.text = NPC.NPCStates.GetStateValue("Thirst").ToString();
            dry.text = NPC.NPCStates.GetStateValue("Dry").ToString();
            if (NPC.rain)
            {
                rainBool.text = "True";
            }
            else
            {
                rainBool.text = "False";
            }
        }
        
    }
}
