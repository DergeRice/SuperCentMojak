using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody PlayerRigid;
    public GameObject MainPlayer;
    [SerializeField] GameObject DeathEffect,DoorEffect;
    [SerializeField] GameObject FightScene;
    
    public float Movespeed;

    private float RandAni;

    [SerializeField] float MaxClose =0.5f;
    float CloseTime=0;
    float GoCastleTime=0;

    public bool DontMove;

    [SerializeField] GameObject JoyStick;
    [SerializeField] GameObject CrowdCenter;
    [SerializeField] GameObject FightArea;

    GameObject Door;

    [SerializeField]GameObject DoorCenter;

    GameObject Castle;
    Vector2 inputVector;
    Vector3 MoveVector;

    Vector3 SpreadVector;

    Animator animator;

    public bool ComeClose=false;
    public bool GotoCastle =false;

    bool  DoorFollow = false;

    bool LookForward =true;

    int layerMask;
    bool ReachCastle =false;
    Vector3  DoorMove;

    [SerializeField]float MoveToDoorSped=2;

    Collider[] EnemeyCol;
    // Start is called before the first frame update
    void Awake(){
        DontMove = false;
        MainPlayer = GameObject.Find("Player");
        JoyStick = GameObject.Find("JoyPad");
        PlayerRigid = gameObject.GetComponent<Rigidbody>();
        animator  = gameObject.GetComponent<Animator>();
        FightArea = GameObject.Find("FightArea");
        
       
    }
    private void OnEnable() {
        
        Movespeed = MainPlayer.GetComponent<Player>().Movespeed;
        
        
        CrowdCenter = GameObject.Find("CrowdCenter");
        FightArea = GameObject.Find("FightArea");
        FightScene =GameObject.Find("Fight").transform.GetChild(0).gameObject;
        Castle = GameObject.Find("Castle");
        DoorCenter = GameObject.Find("DoorCenter");

        Door = GameObject.Find("Door");
        InvokeRepeating(nameof(ChangeAttack),0f,2f);
        animator.SetFloat("Rand",Random.Range(0,1f));
    }


    void ChangeAttack(){
        
            animator.SetFloat("FightRand",Random.Range(0,2));
    }
    void Start()
    {
        Movespeed = 0;
        layerMask = 1  << LayerMask.NameToLayer("Enemy"); 
    }

    // Update is called once per frame
    void Update()
    {
        if(!DontMove)
            Move();
        SetAnimator();
        FinalScene();
        FindEnemy();
        PlayerLootAtForward();
        GettoDoor();
    }

    void GettoDoor(){
        if(DoorFollow){
                transform.position = Vector3.MoveTowards(transform.position,DoorMove,MoveToDoorSped*Time.deltaTime);
        }
                
                
         
    }
    void PlayerLootAtForward(){
        if(LookForward)
            transform.LookAt(Vector3.forward);
        if(animator.GetBool("Dance"))
            transform.LookAt(CrowdCenter.transform.position);
    }

    void FindEnemy(){
        EnemeyCol= Physics.OverlapSphere(transform.position,10,layerMask);
        if(EnemeyCol!=null&&EnemeyCol.Length>0){
             LookForward =false;
             transform.LookAt(EnemeyCol[0].transform);
             GotoCastle =false;
             Vector3 ToEnemyVector = new Vector3(EnemeyCol[0].transform.position.x,0,EnemeyCol[0].transform.position.z);
             transform.position = Vector3.MoveTowards(transform.position,ToEnemyVector,2*Time.deltaTime);    
        }
    }

    public void StartRealFight(){
             animator.SetBool("RealFight",true);
             animator.SetBool("Fight",false);
    }

    void FinalScene(){
        if(ComeClose){
           CloseTime  += Time.deltaTime;
           if(CloseTime <MaxClose){
            transform.position= Vector3.MoveTowards(transform.position,CrowdCenter.transform.position,2*Time.deltaTime);
           }
        }
        if(GotoCastle){
            GoCastleTime  += Time.deltaTime;
           if(!ReachCastle){
            SpreadVector= new Vector3((transform.position.x-CrowdCenter.transform.position.x)*4,transform.position.y,Castle.transform.position.z);
            transform.position= Vector3.MoveTowards(transform.position,SpreadVector,2*Time.deltaTime);
           }
        }

    }

    void Move(){
        inputVector = JoyStick.GetComponent<JoyStick>().inputVector;

        MoveVector = new Vector3(-inputVector.normalized.x,0,-1);

         if(Input.GetMouseButton(0)){
            Movespeed =8.5f;
            animator.SetBool("Active",true);
            }
         else {
            Movespeed = 0;
            animator.SetBool("Active",false);
          }

        PlayerRigid.transform.position += MoveVector*Movespeed*Time.deltaTime;
    }
    void SetAnimator(){
        // if(animator.GetBool("Fight")){
        //     gameObject.transform.LookAt(FightArea.transform.position);
        // }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.CompareTag("Gimmick"))
        {
            Instantiate(DeathEffect,transform.position,Quaternion.identity);
            CrowdCenter.GetComponent<CrowdCenter>().SubCrowd(gameObject);
            Destroy(gameObject);
        }

        

    }
    private void OnCollisionStay(Collision collision) {
        if(collision.collider.CompareTag("Enemy"))
        {

            collision.collider.gameObject.GetComponent<EnemyCS>().DeathTime-=1*Time.deltaTime;
        }
        
    }
    private void OnTriggerEnter(Collider collision) {
        if(collision.CompareTag("Item"))
        {
           Destroy(collision.gameObject);
        }

        if(collision.CompareTag("EndLine"))
        {
            CrowdCenter.GetComponent<CrowdCenter>().CrowdReachFinal();
          animator.SetBool("Fight",true);
          FightScene.SetActive(true);
        }


        if(collision.CompareTag("Castle"))
        {
          ReachCastle =true;
        }

        
        if(collision.CompareTag("Door"))
        {
            animator.SetBool("Punch",true);
            
           
            
            collision.gameObject.GetComponent<BoxCollider>().enabled= false;
            
          
        }

        if(collision.CompareTag("DoorCenter"))
        {
            CrowdCenter.GetComponent<CrowdCenter>().DoorBroken();
             Instantiate(DoorEffect,collision.transform.position,Quaternion.identity);// 히트 파티클 넣어봐
            CrowdCenter.GetComponent<CrowdCenter>().WhoBreakDoor(gameObject);
            Destroy(Door);
           
        }
    }

    public void LetsDance(){
        DoorMove =transform.position;
        MoveToDoorSped=0;
        //transform.position = Vector3.MoveTowards(transform.position,transform.position,2f*Time.deltaTime);
        animator.SetBool("Dance",true);
        // DoorFollow =false;
      
    }

    public void GotoDoor(){
        
        DoorMove = new Vector3(DoorCenter.transform.position.x,transform.position.y,DoorCenter.transform.position.z);
        DoorFollow =true;
        animator.SetBool("RealFight",false);
        animator.SetBool("RunAgain",true);
        if(Door!=null){
            transform.LookAt(Door.transform);
        }
        CrowdCenter.GetComponent<CrowdCenter>().LookDoor();
    }

}
