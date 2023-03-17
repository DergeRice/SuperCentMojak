using UnityEngine;
using UnityEngine.EventSystems; // 키보드, 마우스, 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class JoyStick : MonoBehaviour
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    [SerializeField, Range(10f, 150f)]
    private float leverRange;
    
    public Vector2 inputVector;  
    public Vector2 OriginVector;  // 추가
    public bool isInput;    // 추가
    PointerEventData dd;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        OriginVector = transform.position;
    }
    
    private void Update() {
        
        if(Input.GetMouseButtonDown(0)){  
            transform.position = Input.mousePosition;
            lever.anchoredPosition = Input.mousePosition;
        }

        if(Input.GetMouseButton(0)){
            var inputDir = new Vector2(Input.mousePosition.x,Input.mousePosition.y) - rectTransform.anchoredPosition;
            Vector2 clampedDir;

            if(inputDir.magnitude < leverRange){
                lever.transform.position =  Input.mousePosition;;
            }else{
                lever.transform.position = inputDir.normalized * leverRange;
            }
            clampedDir = inputDir.magnitude < leverRange ? inputDir 
            : inputDir.normalized * leverRange;

            lever.anchoredPosition = clampedDir;
        inputVector = clampedDir / leverRange;   // 추가
        }else{
            transform.position = OriginVector;
        lever.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        }
        
    }
    
}