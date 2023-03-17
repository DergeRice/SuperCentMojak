using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRotate : MonoBehaviour
{
    public float RotSpeed =80;
    public float RotAmount =0;
    [SerializeField] bool X,Y,Z;
    
    [SerializeField] bool _Xop,_Yop,_Zop;

    Quaternion RotateAngle;

    [SerializeField]Vector3 CurRot;

    void Update()
    {
        //CurRot = new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z);
        if(X){transform.rotation= Quaternion.Euler(-RotAmount, CurRot.y, CurRot.z);}
        if(Y){transform.rotation= Quaternion.Euler(CurRot.x, -RotAmount  , CurRot.z);}
        if(Z){transform.rotation= Quaternion.Euler(CurRot.x, CurRot.y, -RotAmount  );}
        if(_Xop){transform.rotation= Quaternion.Euler(RotAmount  , CurRot.y, CurRot.z);}
        if(_Yop){transform.rotation= Quaternion.Euler(CurRot.x, RotAmount  , CurRot.z);}
        if(_Zop){transform.rotation= Quaternion.Euler(CurRot.x, CurRot.y, RotAmount );}
        RotAmount +=Time.deltaTime*RotSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
       //CurRot = new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z);
    }

    // Update is called once per frame

}
