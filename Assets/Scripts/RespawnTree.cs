using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTree : MonoBehaviour
{
    public bool chopped = false;
    public float timer = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (chopped)
        {
            GetComponent<MeshCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }

    }

    void Respawn()
    {
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }


}
