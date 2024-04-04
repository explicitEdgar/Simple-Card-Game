using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public enum Character
{
    player,enemy
}

public class Chara : MonoBehaviour,IPointerDownHandler
{   
    private bool attackable = false;
    private bool choosable = false;

    //血量
    public Text Health;
    private int health = 20;

    public Character character;

    public GameObject border;
    public GameObject enemyborder;

    public GameObject victory;
    public GameObject gameOver;

    public GameObject Canvas;

    IEnumerator Exit()
    {   
        yield return new WaitForSeconds(4);
        BattleManager.Instance.ExitGame();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //被攻击
        if(attackable && !choosable && enemyborder.activeInHierarchy)
        {
            BattleManager.Instance.AttackConfirm(gameObject);
        }
        //没有激活就被攻击
        else if(!attackable && !choosable)
        {
            Valish.Instance.CraeatTips("请先攻击随从！",Canvas);
        }
        
        //被选中
        if(choosable && !attackable && border.activeInHierarchy)
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

    public void Beattacker_player(int damage)
    {
        this.health -= damage;
        if(0 >= health)
        {   
            //判断游戏胜败
            if(character == Character.player)
            {
                gameOver.SetActive(true);
                gameOver.transform.DOScale(new Vector3(10,10,10),3);
            }
            else if(character == Character.enemy)
            {
                victory.SetActive(true);
                victory.transform.DOScale(new Vector3(10,10,10),3);
            }

            //死亡特效
            foreach (var image in gameObject.GetComponentsInChildren<Image>())
            {
                image.DOColor(Color.clear,3.0f);
            }
            gameObject.transform.DOMoveY(50,3.0f).SetRelative(true);
            StartCoroutine(Exit());
        }
        ChangeHealth();
    }

     //重置血量
    public void ChangeHealth()
    {
        this.Health.text = this.health.ToString();
    }
}
