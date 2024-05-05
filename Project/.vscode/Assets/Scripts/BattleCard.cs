using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BattleCardState
{
    inHand,inBlock
}

public class BattleCard : MonoBehaviour,IPointerDownHandler
{   
    private bool attackable = false;
    private bool choosable = false;
    public int playerID;
    public GameObject spend;
    public BattleCardState battleCardState = BattleCardState.inHand;
    public int attacktimes = 1;
    public void OnPointerDown(PointerEventData eventData)
    {   
        if (playerID == 0)
        {
            //实现召唤请求
            if(battleCardState == BattleCardState.inHand && GetComponent<CardDisplay>().card is MonsterCard)
            {   
                MusicManager.Instance.Playsfx(MusicManager.Instance.click);
                BattleManager.Instance.SummonRequest(playerID,gameObject);
            }
            //实现攻击请求
            else if(battleCardState == BattleCardState.inBlock && !attackable && !choosable)
            {
                BattleManager.Instance.AttackRequest(playerID,gameObject);
            }
            //实现法术请求
            if(battleCardState == BattleCardState.inHand && GetComponent<CardDisplay>().card is SpellCard)
            {   
                MusicManager.Instance.Playsfx(MusicManager.Instance.click);
                BattleManager.Instance.SpellRequest(playerID,gameObject);
            }
        }
            
        //被攻击
        if(attackable && !choosable)
        {
            BattleManager.Instance.AttackConfirm(gameObject);
        }

        
        //被选中
        if(choosable && !attackable)
        {
            BattleManager.Instance.SpellConfirm(gameObject);
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

    public void SetAttackable(bool x)
    {
        this.attackable = x;
    }

    public void SetChoosable(bool x)
    {
        this.choosable = x;
    }

    public void Beattacker(int damage)
    {
        MonsterCard mc = this.GetComponent<CardDisplay>().card as MonsterCard;
        mc.healthPoint -= damage;
        // Debug.Log($"失去{damage}点体力");
        if(0 >= mc.healthPoint)
        {
            Destroy(gameObject);
        }
    }

}
