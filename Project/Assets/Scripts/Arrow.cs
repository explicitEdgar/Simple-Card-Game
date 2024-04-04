using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{   
    public Vector2 StartPosition;
    private Vector2 EndingPosition;
    private RectTransform arrow;

    private float arrowLength;
    private float arrowTheta;
    private Vector2 ArrowPosition;
    // Start is called before the first frame update
    void Start()
    {
        arrow = transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {   
        //计算
        EndingPosition = Input.mousePosition - new Vector3(960.0f,540.0f,0.0f);
        arrowLength = Mathf.Sqrt((StartPosition.x - EndingPosition.x)*(StartPosition.x - EndingPosition.x) + (StartPosition.y - EndingPosition.y)*(StartPosition.y - EndingPosition.y)) - 50;
        ArrowPosition = new Vector2((StartPosition.x + EndingPosition.x)/2,(StartPosition.y + EndingPosition.y)/2);
        arrowTheta = Mathf.Atan2(EndingPosition.y - StartPosition.y,EndingPosition.x - StartPosition.x);

        //赋值
        arrow.localPosition = ArrowPosition;
        arrow.sizeDelta = new Vector2(arrowLength,arrow.sizeDelta.y);
        arrow.localEulerAngles = new Vector3(0.0f,0.0f,arrowTheta * 180 / Mathf.PI);
    }

    public void SetStartPoint(Vector2 _StartPosition)
    {
        StartPosition = _StartPosition - new Vector2(960.0f,540.0f);
    }
}
