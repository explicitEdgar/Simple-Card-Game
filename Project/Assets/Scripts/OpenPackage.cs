using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPackage : MonoBehaviour
{   
    public GameObject cardPrefab;
    public GameObject cardpool;

    public List<GameObject> cards = new List<GameObject>();
    public CardStore cardStore;

    public PlayerData playerData;

    public MoneyDisplay moneyDisplay;

    // Start is called before the first frame update
    void Start()
    {   
        cardStore = GetComponent<CardStore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {   
        if(playerData.playerCoins < 2)
        {
            Debug.Log("you have no money!");
            return;
        }
        else
        {
            playerData.playerCoins -= 2;
        }
        moneyDisplay.DisplayMoney();    
        ClearCardpool();
        for (int i = 0; i < 5; i++)
        {
            GameObject newcard = GameObject.Instantiate(cardPrefab,cardpool.transform);
            newcard.GetComponent<CardDisplay>().card = cardStore.RandomCard();

            cards.Add(newcard);
        }
        SavaCards();
        playerData.SavePlayerData();
    }

    public void ClearCardpool()
    {
        foreach (var item in cards)
        {
            Destroy(item);
        }
        cards.Clear();
    }

    public void SavaCards()
    {
        foreach (var card in cards)
        {
            int id = card.GetComponent<CardDisplay>().card.id;
            playerData.playerCards[id]++;
        }
    }
}
