using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq.Expressions;
using System.Reflection.Emit;
using JetBrains.Annotations;
public enum GamePhase
{
    gameStart,playerDrew,playerAction,enemyDrew,enemyAction
}
public class BattleManager : MonoSingleton<BattleManager>
{
    //数据
    public PlayerData playerData;
    public PlayerData enemyData;

    //卡组
    public List<Card> playerDeck = new List<Card>();
    public List<Card> enemyDeck = new List<Card>();

    //手牌
    public Transform playerHand;
    public Transform enemyHand;

    //格子
    public GameObject[] playerBlocks;
    public GameObject[] enemyBlocks;

    //头像
    public GameObject playerIcon;
    public GameObject enemyIcon;

    //卡牌
    public GameObject cardprefab;

    //游戏开始
    public GamePhase gamePhase = GamePhase.gameStart;

    //阶段改变事件
    public UnityEvent phaseChange = new UnityEvent();

    //卡牌召唤暂存
    private GameObject waitingMonster;
    private int waitingPlayer;

    //玩家召唤点数
    private int[] maxSummonPoint = {1,1};
    private int[] summonPoint = new int[2];

    //箭头样本
    public GameObject Arrow;
    public GameObject AttackArrow;

    //可能存在的箭头
    private GameObject arrow;
    private GameObject attackArrow;

    //画布
    public GameObject Canvas;

    //攻击者暂存
    private GameObject waitingAttacker;

    //法术暂存
    private GameObject waitingSpell;

    //双方费用
    public GameObject playerSpend;
    public GameObject enemySpend;

    //提示
    public GameObject tips;
    
    //攻击次数
    public int attacktimes = 1;

    //回合结束按钮
    public GameObject TurnButton;
    


    // Start is called before the first frame update
    void Start()
    {   
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            DestroyArrow();
            CloseBlocks();
            ChangeAttackable();
            ChangeChoosable();
            waitingMonster = null;
            waitingAttacker = null;
            waitingSpell = null;
        }
        ChangeSpend();
    }

    //游戏开始阶段
    public void GameStart()
    {
        if(gamePhase == GamePhase.gameStart)
        {
            ReadCard();
            ShuffleDeck(playerDeck);
            ShuffleDeck(enemyDeck);
            DrewCard(3,0);
            DrewCard(3,1);
            gamePhase = GamePhase.playerAction;
            summonPoint[0] = maxSummonPoint[0];
            summonPoint[1] = maxSummonPoint[1];
            phaseChange.Invoke();
        }
    }
    //加载玩家卡组
    public void ReadCard()
    {
        if(gamePhase == GamePhase.gameStart)
        {   
            //加载自己卡组
            for (int i = 0; i < playerData.playerDeck.Length; i++)
            {
                if(playerData.playerDeck[i] != 0)
                {
                    for (int j = 0; j < playerData.playerDeck[i]; j++)
                    {
                        playerDeck.Add(playerData.cardStore.CopyCard(i));
                    }
                }
            }
            //加载敌方卡组
            for (int i = 0; i < enemyData.playerDeck.Length; i++)
            {
                if(enemyData.playerDeck[i] != 0)
                {
                    for (int j = 0; j < enemyData.playerDeck[i]; j++)
                    {
                        enemyDeck.Add(enemyData.cardStore.CopyCard(i));
                    }
                }
            }
        }
    }
    //洗牌
    public void ShuffleDeck(List<Card> targetDeck)
    {
        for (int i = 0; i < targetDeck.Count; i++)
        {
            int rad = UnityEngine.Random.Range(0,targetDeck.Count);

            Card temp = targetDeck[i];
            targetDeck[i] = targetDeck[rad];
            targetDeck[rad] = temp;
        }
    }
    //抽卡
    public void DrewCard(int much,int num)  //玩家为0，敌人为1
    {
        if(num == 0)
        {       
            for (int i = 0; i < much; i++)
            {   
                if(playerHand.childCount < 5 && playerDeck.Count != 0)
                {
                    GameObject card = Instantiate(cardprefab,playerHand);
                    card.GetComponent<CardDisplay>().card = playerDeck[0];
                    card.GetComponent<BattleCard>().playerID = num;
                    playerDeck.RemoveAt(0);
                }
                else if(playerDeck.Count != 0)
                {
                    playerDeck.RemoveAt(0);
                    Valish.Instance.CraeatTips($"手牌已满 1张牌已烧毁 还剩{playerDeck.Count}张卡",Canvas);
                }
                else
                {
                    Valish.Instance.CraeatTips("牌库已空",Canvas);
                } 
            }   
        }
        else if(num == 1)
        {
            if(enemyHand.childCount < 5 && enemyDeck.Count != 0)
            {
                for (int i = 0; i < much; i++)
                {
                    GameObject card = Instantiate(cardprefab,enemyHand);
                    card.GetComponent<CardDisplay>().card = enemyDeck[0];
                    card.GetComponent<BattleCard>().playerID = num;
                    enemyDeck.RemoveAt(0);
                }
            }
            else if(enemyDeck.Count != 0)
            {
                enemyDeck.RemoveAt(0);
                Valish.Instance.CraeatTips($"手牌已满 1张牌已烧毁 还剩{enemyDeck.Count}张卡",Canvas);
            }
            else
            {
                Valish.Instance.CraeatTips("牌库已空",Canvas);
            }    
        }
    }
    //回合结束
    public void TurnEnd()
    {
        if(gamePhase == GamePhase.playerAction)
        {
            gamePhase = GamePhase.enemyDrew;
            DrewCard(1,1);
            if(maxSummonPoint[1] < 10)
            {
                maxSummonPoint[1]++;
            }
            summonPoint[1] = maxSummonPoint[1];
            ChangeAttackTimes(1);
            gamePhase = GamePhase.enemyAction;
            phaseChange.Invoke();
            AIuse();
        }
        else if(gamePhase == GamePhase.enemyAction)
        {
            gamePhase = GamePhase.playerDrew;
            DrewCard(1,0);
            if(maxSummonPoint[0] < 10)
            {
                maxSummonPoint[0]++;
            }
            summonPoint[0] = maxSummonPoint[0];
            ChangeAttackTimes(0);
            gamePhase = GamePhase.playerAction;
            phaseChange.Invoke();
        }
    }
    //回合结束按钮
    public void OnClick()
    {
        TurnEnd();
    }
    //召唤请求
    public void SummonRequest(int _player,GameObject _monsterCard)
    {
        GameObject[] blocks = playerBlocks;
        bool blockIsEmpty = false;
        if(_player == 0 && gamePhase == GamePhase.playerAction)
        {
            blocks = playerBlocks;
        }
        else if(_player == 1 && gamePhase == GamePhase.enemyAction)
        {
            blocks = enemyBlocks;
        }
        else
        {
            return;
        }
        if(summonPoint[_player] >= _monsterCard.GetComponent<CardDisplay>().card.spend)
        {
            foreach (var block in blocks)
            {
                if(block.GetComponent<Block>().card == null)
                {
                    blockIsEmpty = true;
                    block.GetComponent<Block>().summonBlock.SetActive(true);
                    block.GetComponent<ZoomUI>().enabled=true;
                }
            }   
        }else if(_player == 0)
        {
            Valish.Instance.CraeatTips("您的剩余召唤点为" + summonPoint[_player].ToString(),Canvas);
        }else
        {
            Valish.Instance.CraeatTips("敌人的剩余召唤点为" + summonPoint[_player].ToString(),Canvas);
        }
        if(blockIsEmpty)
        {
            waitingMonster = _monsterCard;
            waitingPlayer = _player;
            CreateArrow(_monsterCard.transform,Arrow);
        }
       
        
    }
    //召唤确认
    public void SummonConfirm(Transform _block)
    {
        Summon(waitingPlayer,waitingMonster,_block);
        CloseBlocks();
        DestroyArrow();
    }
    //召唤
    public void Summon(int _player,GameObject _monsterCard,Transform _block)
    {
        _monsterCard.transform.SetParent(_block);
        _monsterCard.transform.localPosition = Vector3.zero;
        _monsterCard.GetComponent<BattleCard>().battleCardState = BattleCardState.inBlock;
        _monsterCard.GetComponent<BattleCard>().spend.gameObject.SetActive(false);
        _block.GetComponent<Block>().card = _monsterCard;
        summonPoint[_player] -= _monsterCard.GetComponent<CardDisplay>().card.spend;
        waitingMonster = null;
    }
    //攻击请求
    public void AttackRequest(int _player,GameObject _attacker)
    {
        GameObject[] blocks = playerBlocks;
        bool haveMonster = false;
        bool enter = false;

        if(_player == 1 && gamePhase == GamePhase.enemyAction)
        {
            enter = true;
            blocks = playerBlocks;
        }
        else if(_player == 0 && gamePhase == GamePhase.playerAction)
        {
            enter = true;
            blocks = enemyBlocks;
        }
        
        if(enter)
        {   
            if(_attacker.GetComponent<BattleCard>().attacktimes != 0)
            {
                foreach (var block in blocks)
                {
                    if(block.GetComponent<Block>().card != null)
                    {
                        haveMonster = true;
                        block.GetComponent<Block>().card.GetComponent<BattleCard>().SetAttackable(true);
                    }
                }
            }
            else
            {
                Valish.Instance.CraeatTips("该随从本回合已进行过攻击",Canvas);
            }  
        }
        
        if(haveMonster)
        {   
            waitingAttacker = _attacker;
            waitingPlayer = _player;
            CreateArrow(_attacker.transform,AttackArrow);
            // Debug.Log(waitingAttacker.GetComponent<CardDisplay>().card.cardName + "准备发起攻击" + waitingPlayer + gamePhase);
        }
        else if(_attacker.GetComponent<BattleCard>().attacktimes != 0)
        {
            waitingAttacker = _attacker;
            waitingPlayer = _player;
            CreateArrow(_attacker.transform,AttackArrow);
            if(waitingPlayer == 1)
            {
                playerIcon.GetComponent<Chara>().SetAttackable(true);
                playerIcon.GetComponent<Chara>().enemyborder.SetActive(true);
            }
            else if(waitingPlayer == 0)
            {
                enemyIcon.GetComponent<Chara>().SetAttackable(true);
                enemyIcon.GetComponent<Chara>().enemyborder.SetActive(true);
            }
        }
    }
    //攻击确认
    public void AttackConfirm(GameObject _target)
    {
        Attack(_target,waitingAttacker);
        ChangeAttackable();
        DestroyArrow();
    }
    //攻击
    public void Attack(GameObject _target,GameObject _attacker)
    {   
        if(_target.GetComponent<BattleCard>() != null)
        {
            MonsterCard mc1 = _target.GetComponent<CardDisplay>().card as MonsterCard;
            int damage1 = mc1.attack;
            MonsterCard mc2 = _attacker.GetComponent<CardDisplay>().card as MonsterCard;
            int damage2 = mc2.attack;
            _target.GetComponent<BattleCard>().Beattacker(damage2);
            _attacker.GetComponent<BattleCard>().Beattacker(damage1);
            _target.GetComponent<CardDisplay>().ShowCard();
            _attacker.GetComponent<CardDisplay>().ShowCard();
        }
        else
        {   
            MonsterCard mc = _attacker.GetComponent<CardDisplay>().card as MonsterCard;
            int damage = mc.attack;
            _target.GetComponent<Chara>().Beattacker_player(damage);
        }

        _attacker.GetComponent<BattleCard>().attacktimes --;
    }
    //法术请求
    public void SpellRequest(int _player,GameObject _spell)
    {
        GameObject[] blocks = playerBlocks;

        bool havetarget = false;
        bool usable = false;

        if(_player == 0 && gamePhase == GamePhase.playerAction)
        {
            usable = true;
        }
        else if(_player == 1 && gamePhase == GamePhase.enemyAction)
        {
            usable = true;
        }

        if(usable)  //回合合理性
        {
             if(summonPoint[_player] >= _spell.GetComponent<CardDisplay>().card.spend)  //费用够再循环
            {   
                blocks = playerBlocks;
                foreach (var block in blocks)
                {
                    if(block.GetComponent<Block>().card != null)
                    {
                        havetarget = true;
                        block.GetComponent<Block>().card.GetComponent<BattleCard>().SetChoosable(true);
                    }
                }
                blocks = enemyBlocks;
                foreach (var block in blocks)
                {
                    if(block.GetComponent<Block>().card != null)
                    {
                        havetarget = true;
                        block.GetComponent<Block>().card.GetComponent<BattleCard>().SetChoosable(true);
                    }
                }

                playerIcon.GetComponent<Chara>().SetChoosable(true);
                enemyIcon.GetComponent<Chara>().SetChoosable(true);
                playerIcon.GetComponent<Chara>().border.SetActive(true);
                enemyIcon.GetComponent<Chara>().border.SetActive(true);
                havetarget = true;

            }
            else if(_player == 0)  //费用不够就提醒
            {
                Valish.Instance.CraeatTips("您的剩余召唤点为" + summonPoint[_player].ToString(),Canvas);
            }else
            {
                Valish.Instance.CraeatTips("敌人的剩余召唤点为" + summonPoint[_player].ToString(),Canvas);
            }

        }
       
        if(havetarget)  //拥有目标
        {   
            waitingSpell = _spell;
            waitingPlayer = _player;
            CreateArrow(_spell.transform,AttackArrow);
        }
    }
    //法术确认
    public void SpellConfirm(GameObject _target)
    {
        TickleSpell(_target,waitingSpell);
        ChangeChoosable();
        DestroyArrow();
    }
    //释放法术
    public void TickleSpell(GameObject _target,GameObject _spell)
    {
        SpellEffect(_spell.GetComponent<CardDisplay>().card.id,_target);
        summonPoint[waitingPlayer] -= _spell.GetComponent<CardDisplay>().card.spend;
        Destroy(_spell);
    }
    //法术效果
    public void SpellEffect(int _id,GameObject _target)
    {
        switch (_id)
        {
            case 5:
                if(_target.GetComponent<BattleCard>() != null)
                {
                    _target.GetComponent<BattleCard>().Beattacker(6);
                    _target.GetComponent<CardDisplay>().ShowCard();
                }
                else
                {
                    _target.GetComponent<Chara>().Beattacker_player(6);
                }
                break;

            case 6:
                if(_target.GetComponent<BattleCard>() != null)
                {
                    _target.GetComponent<BattleCard>().Beattacker(-3);
                    _target.GetComponent<CardDisplay>().ShowCard();
                }
                else
                {
                    _target.GetComponent<Chara>().Beattacker_player(-3);
                }
                break;

            case 7:
                int _player = 0;
                if(_target.GetComponent<BattleCard>() != null)
                {
                    _player = _target.GetComponent<BattleCard>().playerID;
                }else
                {
                    _player = _target.GetComponent<Chara>().character == Character.player ? 0 : 1;
                }
                GameObject[] blocks = playerBlocks;
                if(_player == 0)
                {
                    blocks = playerBlocks;
                }
                else if(_player == 1)
                {
                    blocks = enemyBlocks;
                }
                foreach (var block in blocks)
                {
                    if(block.GetComponent<Block>().card != null)
                    {
                        block.GetComponent<Block>().card.GetComponent<BattleCard>().Beattacker(1);
                        block.GetComponent<Block>().card.GetComponent<CardDisplay>().ShowCard();
                    }
                }
                break;

            default: 
                break;
        }
    }
    //创建箭头
    public void CreateArrow(Transform transform,GameObject gameObject)
    {   
        DestroyArrow();
        arrow = Instantiate(gameObject,transform);
        arrow.transform.SetParent(Canvas.transform, false);
        arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(transform.position.x,transform.position.y));
    }
    //摧毁箭头
    public void DestroyArrow()
    {
        Destroy(arrow);
        Destroy(attackArrow);
    }
    //关闭格子显示
    public void CloseBlocks()
    {
        GameObject[] blocks = playerBlocks;
        if(waitingPlayer == 0)
        {
            blocks = playerBlocks;
        }
        else if(waitingPlayer == 1)
        {
            blocks = enemyBlocks;
        }
        foreach (var block in blocks)
        {
            block.GetComponent<Block>().summonBlock.SetActive(false);
            block.GetComponent<ZoomUI>().enabled = false;
            block.transform.localScale = Vector3.one;
        }
    }
    //关闭可被攻击性
    public void ChangeAttackable()
    {   
        GameObject[] blocks = playerBlocks;
        if(waitingPlayer == 1)
        {
            blocks = playerBlocks;
            playerIcon.GetComponent<Chara>().SetAttackable(false);
            playerIcon.GetComponent<Chara>().enemyborder.SetActive(false);
        }
        else if(waitingPlayer == 0)
        {
            blocks = enemyBlocks;
            enemyIcon.GetComponent<Chara>().SetAttackable(false);
            enemyIcon.GetComponent<Chara>().enemyborder.SetActive(false);
        }
        foreach (var block in blocks)
        {   
            if(block.GetComponent<Block>().card != null)
            {
                 block.GetComponent<Block>().card.GetComponent<BattleCard>().SetAttackable(false);   
            }
        }
           
    }
    //关闭可被选择性
    public void ChangeChoosable()
    {
        GameObject[] blocks = playerBlocks;
        foreach (var block in blocks)
        {   
            if(block.GetComponent<Block>().card != null)
            {
                 block.GetComponent<Block>().card.GetComponent<BattleCard>().SetChoosable(false);  
            }
        }
        blocks = enemyBlocks;
        foreach (var block in blocks)
        {   
            if(block.GetComponent<Block>().card != null)
            {
                 block.GetComponent<Block>().card.GetComponent<BattleCard>().SetChoosable(false);   
            }
        }
        playerIcon.GetComponent<Chara>().SetChoosable(false);
        playerIcon.GetComponent<Chara>().border.SetActive(false); 
        enemyIcon.GetComponent<Chara>().SetChoosable(false);
        enemyIcon.GetComponent<Chara>().border.SetActive(false); 
    }
    //重置费用
    public void ChangeSpend()
    {
        enemySpend.GetComponentInChildren<Text>().text = summonPoint[1].ToString();
        playerSpend.GetComponentInChildren<Text>().text = summonPoint[0].ToString();
    }
    //重置攻击次数
    public void ChangeAttackTimes(int _player)
    {
        GameObject[] blocks = playerBlocks;
        if(_player == 0)
        {
            blocks = playerBlocks;
        }
        else if(_player == 1)
        {
            blocks = enemyBlocks;
        }
        foreach (var block in blocks)
        {
            if(block.GetComponent<Block>().card != null)
            {
                block.GetComponent<Block>().card.GetComponent<BattleCard>().attacktimes = 1;
            }
        }
    }
    //退出游戏
    public void ExitGame()
   {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
   }
    //AI打牌
     public void AIuse()
    {   
        if(gamePhase == GamePhase.enemyAction)
        {   
            waitingPlayer = 1;
            DG.Tweening.Sequence bigseq =DOTween.Sequence();
            bigseq.AppendCallback(()=>{TurnButton.SetActive(false);});
            bigseq.AppendInterval(2);
            bigseq.AppendCallback
            (()=>
                {   
                    DG.Tweening.Sequence seq = DOTween.Sequence();
                    //出牌
                    for (int i = 0;i < enemyHand.childCount;i++)
                    {
                        GameObject card = enemyHand.GetChild(i).gameObject;
                        if(summonPoint[1] >= card.GetComponent<CardDisplay>().card.spend) //是否费用够
                        {
                            if(card.GetComponent<CardDisplay>().card is MonsterCard)  //如果是怪物牌
                            {   
                                void AIsummon()  //AI下怪
                                {
                                    foreach (var block in enemyBlocks)
                                    {
                                        if(block.GetComponent<Block>().card == null)
                                        {   
                                            seq.AppendCallback
                                            (()=>
                                                {   //双重检查 避免多线程同时触发导致超费
                                                    if(summonPoint[1] >= card.GetComponent<CardDisplay>().card.spend && block.GetComponent<Block>().card == null)
                                                    Summon(1,card,block.transform);
                                                    else AIsummon();
                                                }
                                            );
                                            seq.AppendInterval(2);
                                            break;
                                        }
                                    }
                                }

                                AIsummon();     
                            }

                            else if(card.GetComponent<CardDisplay>().card is SpellCard)  //如果是法术牌
                            {   
                                bool blockIsEmpty = true;
                                void AIspell(GameObject[] blocks)  //AI施法
                                {
                                    foreach (var block in blocks)
                                    {
                                        if(block.GetComponent<Block>().card != null)
                                        {
                                            blockIsEmpty = false;
                                            seq.AppendCallback
                                            (
                                                ()=>
                                                {
                                                    if(summonPoint[1] >= card.GetComponent<CardDisplay>().card.spend && block.GetComponent<Block>().card != null)
                                                    TickleSpell(block.GetComponent<Block>().card,card);
                                                    else AIspell(blocks);
                                                }
                                            );
                                            seq.AppendInterval(2);
                                            break;
                                        }
                                    }
                                }

                                if(card.GetComponent<CardDisplay>().card.id != 6)  //如果是伤害法术
                                {   
                                    AIspell(playerBlocks);
                                   
                                    if(blockIsEmpty == true)  //如果对方没怪
                                    {
                                        seq.AppendCallback(()=>TickleSpell(playerIcon,card)); 
                                        seq.AppendInterval(2);
                                    }
                                }

                                else  //如果是治疗法术
                                {  
                                    AIspell(enemyBlocks);

                                    if(blockIsEmpty == true)  //如果自己没怪
                                    {   
                                        seq.AppendCallback(()=>TickleSpell(enemyIcon,card));
                                        seq.AppendInterval(2);
                                    }
                                }
                            }
                        }
                    
                    }
                }
            );

            bigseq.AppendInterval(2);
            bigseq.AppendCallback
            (()=>
                {   
                    DG.Tweening.Sequence seq = DOTween.Sequence();
                    //行动
                    foreach (var block in enemyBlocks)
                    {   
                        //如果有怪且怪能动
                        if(block.GetComponent<Block>().card != null && block.GetComponent<Block>().card.GetComponent<BattleCard>().attacktimes != 0)
                        {
                            bool blockIsEmpty = true;
                            void HaveMonster()
                            {   
                                blockIsEmpty = true;
                                foreach (var _block in playerBlocks)
                                {
                                    if(_block.GetComponent<Block>().card != null)
                                    {
                                        blockIsEmpty = false;
                                        seq.AppendCallback
                                        (
                                            ()=>
                                            {
                                                if(_block.GetComponent<Block>().card != null)
                                                Attack(_block.GetComponent<Block>().card,block.GetComponent<Block>().card);
                                                else
                                                HaveMonster();
                                            }   
                                        );
                                        seq.AppendInterval(2);
                                        break;
                                    }    
                                }
                            }

                            HaveMonster();
                            
                            if(blockIsEmpty == true)  //如果对方没怪
                            {
                                seq.AppendCallback(()=>Attack(playerIcon,block.GetComponent<Block>().card));
                                seq.AppendInterval(2);                       
                            }

                        }
                    }
                }
            );
           
            //结束回合
            bigseq.AppendInterval(2);
            bigseq.AppendCallback(TurnEnd);
            bigseq.AppendCallback(()=>{TurnButton.SetActive(true);});
        }
    }
}