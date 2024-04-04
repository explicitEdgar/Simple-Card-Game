using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour,IPointerDownHandler
{   
    public GameObject card;
    public GameObject summonBlock;
    public void OnPointerDown(PointerEventData eventData)
    {
        //传递格子坐标
        if(summonBlock.activeInHierarchy)
        {
            BattleManager.Instance.SummonConfirm(transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
