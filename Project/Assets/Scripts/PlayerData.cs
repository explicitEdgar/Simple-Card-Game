using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class PlayerData : MonoBehaviour
{
    public TextAsset playerdata;
    public CardStore cardStore;
    public int playerCoins;
    public int[] playerCards;
    public int[] playerDeck;
    // Start is called before the first frame update
    void Start()
    {   
        cardStore.LoadData();
        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayerData()
    {
        string path = Application.dataPath + "/Datas/playerdata.csv";
        List<string> data = new List<string>();
        data.Add("coins," + playerCoins.ToString());
        for (int i = 0; i < playerCards.Length; i++)
        {
            if(playerCards[i] != 0)
            {
                data.Add("card," + i.ToString() + "," + playerCards[i].ToString());
            }
        }
        for (int i = 0; i < playerDeck.Length; i++)
        {
            if(playerDeck[i] != 0)
            {
                data.Add("deck," + i.ToString() + "," + playerDeck[i].ToString());
            }
        }

        File.WriteAllLines(path,data);
        AssetDatabase.Refresh();

    }

    public void LoadPlayerData()
    {   
        playerCards = new int[cardStore.cardList.Count];
        playerDeck = new int[cardStore.cardList.Count];
        string[] dataRow = playerdata.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if(rowArray[0] == "coins")
            {
                //读入金币
                playerCoins = int.Parse(rowArray[1]);
                
            }
            else if(rowArray[0] == "card")
            {   
                //读入卡牌
               int id = int.Parse(rowArray[1]);
               playerCards[id] = int.Parse(rowArray[2]);

            }
            else if(rowArray[0] == "deck")
            {
                //读入卡组
                int id = int.Parse(rowArray[1]);
                playerDeck[id] = int.Parse(rowArray[2]);
            }
        }
    }
}

