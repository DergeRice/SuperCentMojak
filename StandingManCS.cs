using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingManCS : MonoBehaviour
{
    [SerializeField] Material Blue;

    GameObject CrowdCenter;
    FixedJoint Joint;
    // Start is called before the first frame update

    void Awake(){
        //CrowdCenter =  GameObject.Find("CrowdCenter");
        //Joint.connectedBody =CrowdCenter.GetComponent<Rigidbody>();
    }
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
      //  if(Collision)
    }
    private void OnCollisionEnter(Collision collision) {

        if(collision.collider.CompareTag("Player"))
        {
            if(gameObject.transform.parent.TryGetComponent<StadingParents>(out StadingParents parent))
                parent.Touched=true;
        }
    }
    private void OnTriggerEnter(Collider collider) {
        if(collider.CompareTag("Player"))
        {
            if(gameObject.transform.parent.TryGetComponent<StadingParents>(out StadingParents parent))
                parent.Touched=true;
        }
    }

    public void Touched(){
        //Debug.Log("Dd");
        
        foreach(Transform child in transform)
            {
                if(child.GetComponent<SkinnedMeshRenderer>()!=null)
                    child.GetComponent<SkinnedMeshRenderer>().material= Blue;
            }
    }
}
