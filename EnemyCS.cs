using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCS : MonoBehaviour
{
    Animator animator;

    GameObject FightArea;

    [SerializeField] GameObject DeathEffect;
    [SerializeField] GameObject CrowdCenter;

    public float DeathTime;

    Collider[] PlayerCol;

    int layerMask;

    bool GoingToOut=true;
    // Start is called before the first frame update
    void Start()
    {
        CrowdCenter = GameObject.Find("CrowdCenter");
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("Fight",true);
        animator.SetBool("Active",true);
        FightArea = GameObject.Find("FightArea");
        // StartCoroutine(StartFight());
         layerMask = 1  << LayerMask.NameToLayer("Crowd"); 
         InvokeRepeating(nameof(ChangeAttack),0f,2f);
         DeathTime =5f;

        animator.SetFloat("Rand",Random.Range(0,1f));
    }
        void ChangeAttack(){
        animator.SetFloat("FightRand",Random.Range(0,2));
    }

    // Update is called once per frame
    void Update()
    {
        if(GoingToOut)
            {transform.position=Vector3.MoveTowards(transform.position,FightArea.transform.position,3*Time.deltaTime);
            transform.LookAt(FightArea.transform.position);
            }
        FindPlayer();
        if(DeathTime<0){Death();}
    }

    void FindPlayer(){
        PlayerCol= Physics.OverlapSphere(transform.position,4,layerMask);
        if(PlayerCol!=null&&PlayerCol.Length>0){
            transform.LookAt(PlayerCol[0].transform);
            animator.SetBool("RealFight",true);
            animator.SetBool("Fight",false);
            GoingToOut = false;
             Vector3 ToEnemyVector = new Vector3(CrowdCenter.transform.position.x,0,CrowdCenter.transform.position.z);
              transform.position = Vector3.MoveTowards(transform.position,ToEnemyVector,0.8f*Time.deltaTime);  

            for(int i = 0; i<PlayerCol.Length;i++){
                PlayerCol[i].gameObject.GetComponent<Player>().StartRealFight();     
            }
        }
    }

    // IEnumerator StartFight(){
    //     yield return new WaitForSeconds(5f);
    //     animator.SetBool("RealFight",true);
    //      animator.SetBool("Fight",false);
    //     GoingToOut = false;
    // }

    private void OnCollisionStay(Collision collision) {

        if(collision.collider.CompareTag("Player"))
        {
            
           //Destroy(gameObject);
        }
    }

    void Death(){
        Instantiate(DeathEffect,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

}
