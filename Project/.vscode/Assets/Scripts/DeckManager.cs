using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour
{   
    public Transform DeckPanel;
    public Transform LibraryPanel;

    public GameObject Deckprefab;
    public GameObject Libraryprefab;

    public GameObject DataManager;
    private CardStore cardStore;
    private PlayerData playerData;
    public Dictionary<int,GameObject> LibraryDic = new Dictionary<int, GameObject>();
    public Dictionary<int,GameObject> DeckDic = new Dictionary<int, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        cardStore = DataManager.GetComponent<CardStore>();
        playerData = DataManager.GetComponent<PlayerData>();
        UpdateLibrary();
        UpdateDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDeck()
    {
        for (int i = 0; i < playerData.playerDeck.Length; i++)
        {   
            if(playerData.playerDeck[i] > 0)
            {
               CreateCard( DeckPanel, i,playerData.playerDeck[i]);
            }
        }
    }

    public void UpdateLibrary()
    {
        for (int i = 0; i < playerData.playerCards.Length; i++)
        {   
            if(playerData.playerCards[i] > 0)
            {
                CreateCard( LibraryPanel, i,playerData.playerCards[i]);
            }
        }
    }

    public void UpdateCard(CardSate _state,int _id)
    {
        if(_state == CardSate.Library)
        {
            playerData.playerCards[_id]--;

            if(playerData.playerDeck[_id] == 0) DeckDic.Remove(_id);
            
            if(!DeckDic.ContainsKey(_id))
            {
                CreateCard( DeckPanel, _id,1);
            }
            else
            {
                DeckDic[_id].GetComponent<CardCounter>().SetNum(1);
            }

            LibraryDic[_id].GetComponent<CardCounter>().SetNum(-1);

            playerData.playerDeck[_id]++;


        }
        else if(_state == CardSate.Deck)
        {
            
            playerData.playerDeck[_id]--;
            if(playerData.playerCards[_id] == 0) LibraryDic.Remove(_id);
            
            if(!LibraryDic.ContainsKey(_id))
            {
                CreateCard( LibraryPanel, _id,1);
            }
            else
            {
                LibraryDic[_id].GetComponent<CardCounter>().SetNum(1);
            }

            DeckDic[_id].GetComponent<CardCounter>().SetNum(-1);

            playerData.playerCards[_id]++;
        }

        
    }

    private void CreateCard(Transform area,int id,int much)
    {   
        if(area == DeckPanel)
        {
            GameObject newCard = Instantiate(Deckprefab,DeckPanel);
            newCard.GetComponent<CardCounter>().SetNum(much);
            newCard.GetComponent<CardDisplay>().card = cardStore.cardList[id];
            DeckDic.Add(id,newCard);
        }
        else if(area == LibraryPanel)
        {
            GameObject newCard = Instantiate(Libraryprefab,LibraryPanel);
            newCard.GetComponent<CardCounter>().SetNum(much);
            newCard.GetComponent<CardDisplay>().card = cardStore.cardList[id];
            LibraryDic.Add(id,newCard);
        }
        
    }

    public void Onclick()
    {   
        MusicManager.Instance.Playsfx(MusicManager.Instance.click);
        playerData.SavePlayerData();
        SceneManager.LoadScene(1);
    }
}
