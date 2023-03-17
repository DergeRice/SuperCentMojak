using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrowdCenter : MonoBehaviour
{
    public GameObject Mom;
    Collider[] CrowdCol;
    [SerializeField] List<GameObject> CrowdObject;
    [SerializeField]int LastCrowdCount;

    [SerializeField] float MoveSpeed;
    [SerializeField] Vector3 CenterPos;
    public float CenterPosX;
    public float CenterPosY;
    public float CenterPosZ;
    int layerMask;
    bool CrowdChange;
    bool ReachFinal;
    bool DoorBreak;

    bool LookDoorBool;

    bool YouAreFirst = true;

    GameObject EnemeyMom;

    [SerializeField] GameObject FollowCam;

    [SerializeField] GameObject Logo;
     [SerializeField] GameObject LogoCanvas;
    // Start is called before the first frame update
     private void Awake() {
        CrowdObject = new List<GameObject>(); 
        EnemeyMom =GameObject.Find("Castle").transform.GetChild(0).gameObject;
        //FollowCam =GameObject.Find("")
        
    }
    void Start()
    {
       CrowdObject.Add(Mom);
        CenterPosX = 0;
        CenterPosY = 0;
        CenterPosZ = 0;

        layerMask = 1  << LayerMask.NameToLayer("Crowd"); 

    }

    // Update is called once per frame
    void Update()
    {
        SetCenter();
        WinEvent();

        CrowdCol= Physics.OverlapSphere(transform.position,5,layerMask);
        if(CrowdCol!=null){
             for(int i =0; i < CrowdCol.Length;i++){
              if(CrowdCol[i].gameObject.tag == "Player"&&!(CrowdObject.Contains(CrowdCol[i].gameObject)))
                 {
                    CrowdObject.Add(CrowdCol[i].gameObject);
                    CrowdChange =true;
                 }
             }
        }
         if(CrowdObject.Count>1)
            {
                if(!ReachFinal)
                    CenterPos = new Vector3 (CenterPosX/CrowdObject.Count,CrowdObject[0].transform.position.y,CrowdObject[0].transform.position.z);
                else
                    if(DoorBreak)CenterPos = new Vector3 (CenterPosX/CrowdObject.Count,CrowdObject[0].transform.position.y,(CenterPosZ/CrowdObject.Count)+5f);
                    else CenterPos = new Vector3 (CenterPosX/CrowdObject.Count,CrowdObject[0].transform.position.y,(CenterPosZ/CrowdObject.Count)-2f);
            }
        else 
            {CenterPos= CrowdObject[0].transform.position;}

        transform.position = new Vector3(Mathf.Lerp(transform.position.x,CenterPos.x,MoveSpeed*Time.deltaTime),CenterPos.y,CenterPos.z); 


        
    }

    public void LookDoor(){
        if(!LookDoorBool)
           { FollowCam.GetComponent<FollowCam>().GoingToDoor();
           LookDoorBool = true;
           }
    }
    private void WinEvent(){
        if(EnemeyMom.transform.childCount == 1){
            for(int i=0;i<CrowdObject.Count;i++){
            CrowdObject[i].GetComponent<Player>().GotoDoor();
            // =true;
            }
        }
    }
    private void LateUpdate() {
        MobChanged();  
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,5);
    }

    void ListChangeCheck(){
        
    }
    void SetCenter(){
        CenterPosX = 0;
        CenterPosY = 0;
        CenterPosZ = 0;
        for(int i =0; i < CrowdObject.Count; i++){
                 CenterPosX +=CrowdObject[i].transform.position.x;
                 CenterPosY +=CrowdObject[i].transform.position.y;
                 CenterPosZ +=CrowdObject[i].transform.position.z;
                }
    }

    public void SubCrowd(GameObject OB){
        if((CrowdObject.Contains(OB)))
            CrowdObject.Remove(OB);
    }

    void MobChanged(){
        if(CrowdChange){//변화가 생김
            
            CrowdChange= false;
            
                GameObject Logos= Instantiate(Logo,LogoCanvas.transform);
                Logos.SetActive(true);
            
            
        }
    }

    public void CrowdReachFinal(){
        ReachFinal =true;
        for(int i=0;i<CrowdObject.Count;i++){
            CrowdObject[i].GetComponent<Player>().DontMove =true;
            CrowdObject[i].GetComponent<Player>().ComeClose =true;
        }
        StartCoroutine(StartSpread());
        
    }

    IEnumerator StartSpread(){
        yield return new WaitForSeconds(0.5f);
        for(int i=0;i<CrowdObject.Count;i++){
            CrowdObject[i].GetComponent<Player>().GotoCastle =true;
            CrowdObject[i].GetComponent<Player>().ComeClose =false;
        }
    }

    public void DoorBroken(){
        DoorBreak =true;
        for(int i=0;i<CrowdObject.Count;i++){
            CrowdObject[i].GetComponent<Player>().LetsDance() ;
        }
        FollowCam.GetComponent<FollowCam>().EndingScreen();
        GameObject.Find("Castle").transform.GetChild(3).gameObject.SetActive(true);
        GameObject.Find("Castle").transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find("Castle").transform.GetChild(1).gameObject.GetComponent<Animator>().SetBool("Dance",true);
    }

    public void WhoBreakDoor(GameObject Who){
        if(YouAreFirst){
             Destroy(Who);
            SubCrowd(Who);
            YouAreFirst =false;
        }

    }
}
