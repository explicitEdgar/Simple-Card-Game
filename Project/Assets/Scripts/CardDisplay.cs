using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour
{
    public Text nameText;
    public Text attackText;
    public Text healthText;
    public Text effectText;

    public Text spendText;

    public Image backgroundImage;

    public Sprite sprite;

    public Card card; 


    // Start is called before the first frame update
    void Start()
    {
        ShowCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCard()
    {
        nameText.text = card.cardName;
        this.sprite =  card.sprite;
        backgroundImage.sprite= this.sprite;
        spendText.text = card.spend.ToString();
        if (card is MonsterCard)
        {
            var monster = card as MonsterCard;
            attackText.text = monster.attack.ToString();
            healthText.text = monster.healthPoint.ToString();

            effectText.gameObject.SetActive(false);
        }
        else if(card is SpellCard)
        {
            var spell = card as SpellCard;
            effectText.text = spell.effect;

            attackText.gameObject.SetActive(false);
            healthText.gameObject.SetActive(false);
        }
    }
    



}
