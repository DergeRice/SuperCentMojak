using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadingParents : MonoBehaviour
{

    public bool Touched=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Touched){
            foreach(Transform child in transform)
            { 
                child.gameObject.tag = "Player";
                child.GetComponent<Player>().enabled=true;
                child.GetComponent<StandingManCS>().Touched();
            }
        }
    }
}
