using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStore : MonoBehaviour
{
    public TextAsset cardData;
    public List<Card> cardList = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        // LoadData();
        Test();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData()
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if(rowArray[0] == "monster")
            {
                //新建怪兽卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int atk = int.Parse(rowArray[3]);
                int health = int.Parse(rowArray[4]);
                Sprite sprite = Resources.Load<Sprite>(path: rowArray[5]);
                int spend = int.Parse(rowArray[6]);

                MonsterCard monsterCard = new MonsterCard(id,name,sprite,spend,atk,health);
                cardList.Add(monsterCard);
            }
            else if(rowArray[0] == "spell")
            {   
                //新建魔法卡
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect = rowArray[3];
                Sprite sprite = Resources.Load<Sprite>(path: rowArray[4]);
                int spend = int.Parse(rowArray[5]);

                SpellCard spellCard = new SpellCard(id,name,sprite,spend,effect);
                cardList.Add(spellCard);
            }
        }
        
    }

    public void Test()
    {
        foreach (var item in cardList)
        {
            Debug.Log("卡牌:"  + item.id.ToString() + item.cardName); 
        }
    }

    public Card RandomCard()
    {   
        Card card = cardList[Random.Range(0,cardList.Count)];
        return card;
    }

    public Card CopyCard(int id)
    {   
        Card newCard = new Card(id,cardList[id].cardName,cardList[id].sprite,cardList[id].spend);
        
        if(cardList[id] is MonsterCard)
        {
            var monsterCard = cardList[id] as MonsterCard;
            newCard = new MonsterCard(id,monsterCard.cardName,monsterCard.sprite,monsterCard.spend,monsterCard.attack,monsterCard.healthPointMax);
        }
        else if(cardList[id] is SpellCard)
        {
            var spellCard = cardList[id] as SpellCard;
            newCard = new SpellCard(id,spellCard.cardName,spellCard.sprite,spellCard.spend,spellCard.effect);
        }

        return newCard;
    }
}
