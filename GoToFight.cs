using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToFight : MonoBehaviour
{

    [SerializeField] GameObject Flag, Door,Camera,Enemy;
    FollowCam CamF;
    float FightCamY,FightCamZ;
    bool CamBool;
   // [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
        Flag.GetComponent<Animator>().SetBool("GoToFight",true);
        Door.GetComponent<Animator>().SetBool("GoToFight",true);
        CamF =Camera.GetComponent<FollowCam>();
        CamF.GameCamY = CamF.FightCamY;
        CamF.GameCamZ = CamF.FightCamZ;
       
       StartCoroutine(StartFight());
        Enemy.SetActive(true);
        
    }

    IEnumerator StartFight(){
        yield return new WaitForSeconds(1f);
         CamF.FightBool =true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
