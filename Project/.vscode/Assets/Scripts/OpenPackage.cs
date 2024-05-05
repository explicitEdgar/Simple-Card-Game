using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenPackage : MonoBehaviour
{   
    public GameObject cardPrefab;
    public GameObject cardpool;

    public List<GameObject> cards = new List<GameObject>();
    public CardStore cardStore;

    public PlayerData playerData;

    public MoneyDisplay moneyDisplay;

    public GameObject canvas;


    void Awake()
    {   
        cardStore = GetComponent<CardStore>();
        playerData.LoadPlayerData();
        if(playerData.playerOK == false)
        {
            SceneManager.LoadScene(0);
        }

    }
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {   
        if(playerData.playerCoins < 2)
        {   
            MusicManager.Instance.Playsfx(MusicManager.Instance.click);
            Valish.Instance.CraeatTips("you have no money!",canvas);
            return;
        }
        else
        {   
            MusicManager.Instance.Playsfx(MusicManager.Instance.open);
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

        public void OnClick1()
    {   
        MusicManager.Instance.Playsfx(MusicManager.Instance.click);
        SceneManager.LoadScene(2);
    }

        public void OnClick2()
    {   
        MusicManager.Instance.Playsfx(MusicManager.Instance.click);
        SceneManager.LoadScene(3);
    }

        public void OnClick3()
    {   
        MusicManager.Instance.Playsfx(MusicManager.Instance.click);
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
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
