using UnityEngine;

public class Card
{
    public int id;
    public string cardName;
    public Sprite sprite;

    public int spend;

    public Card(int _id,string _cardName,Sprite _sprite,int _spend)
    {
        this.id = _id;
        this.cardName = _cardName;
        this.sprite = _sprite;
        this.spend = _spend;
    }

}

public class MonsterCard: Card
{
    public int attack;
    public int healthPoint;
    public int healthPointMax;

    public MonsterCard(int _id,string _cardName,Sprite _sprite,int _spend,int _attack,int _healthPointMax): base(_id,_cardName,_sprite,_spend)
    {
        this.attack = _attack;
        this.healthPoint = _healthPointMax;
        this.healthPointMax = _healthPointMax;
    }
}

public class SpellCard: Card
{
    public string effect;

    public SpellCard(int _id,string _cardName,Sprite _sprite,int _spend,string _effect): base(_id,_cardName,_sprite,_spend)
    {
        this.effect = _effect;
    }
}