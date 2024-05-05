using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PlayerDataClass
{
    public bool playerOK;
    public int playerCoins;
    public int[] playerCards;
    public int[] playerDeck;

}
public enum PlayerType
{
    player,enemy
}

public class PlayerData : MonoBehaviour
{
    // public TextAsset playerdata;
    public CardStore cardStore;
    public bool playerOK;
    public int playerCoins;
    public int[] playerCards;
    public int[] playerDeck;
    public PlayerDataClass playerDataclass;

    public PlayerType playerType;
    // Start is called before the first frame update
    void Awake()
    {   
        LoadPlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayerData()
    {   
        // string path = Application.streamingAssetsPath + "/playerdata.csv";
        // FileInfo fi = new FileInfo(path);
        // if (!fi.Directory.Exists)
        // {
        //     fi.Directory.Create();
        // }
        // List<string> data = new List<string>();
        // data.Add("OK," + playerOK.ToString());
        // data.Add("coins," + playerCoins.ToString());
        // for (int i = 0; i < playerCards.Length; i++)
        // {
        //     if(playerCards[i] != 0)
        //     {
        //         data.Add("card," + i.ToString() + "," + playerCards[i].ToString());
        //     }
        // }
        // for (int i = 0; i < playerDeck.Length; i++)
        // {
        //     if(playerDeck[i] != 0)
        //     {
        //         data.Add("deck," + i.ToString() + "," + playerDeck[i].ToString());
        //     }
        // }

        // File.WriteAllLines(path,data);
        // #if UNITY_EDITOR
        //     AssetDatabase.Refresh();
        // #endif

        playerDataclass.playerOK = playerOK;
        playerDataclass.playerCoins = playerCoins;
        for (int i = 0;i < cardStore.cardList.Count;i++)
        {   
            // Debug.Log(i);
            playerDataclass.playerCards[i] = playerCards[i];
            playerDataclass.playerDeck[i] = playerDeck[i];
        }

        string json = JsonUtility.ToJson(playerDataclass,true);
        string filepath = Application.streamingAssetsPath + "/playerdata.json";

        using(StreamWriter sw = new StreamWriter(filepath))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
    }

    public void LoadPlayerData()
    {   
        // playerCards = new int[cardStore.cardList.Count];
        // playerDeck = new int[cardStore.cardList.Count];
        // string[] dataRow = playerdata.text.Split('\n');
        // foreach (var row in dataRow)
        // {
        //     string[] rowArray = row.Split(',');
        //     if (rowArray[0] == "#")
        //     {
        //         continue;
        //     }
        //     else if(rowArray[0] == "OK")
        //     {
        //         //读入剧情
        //         playerOK = bool.Parse(rowArray[1]);
                
        //     }
        //     else if(rowArray[0] == "coins")
        //     {
        //         //读入金币
        //         playerCoins = int.Parse(rowArray[1]);
                
        //     }
        //     else if(rowArray[0] == "card")
        //     {   
        //         //读入卡牌
        //        int id = int.Parse(rowArray[1]);
        //        playerCards[id] = int.Parse(rowArray[2]);

        //     }
        //     else if(rowArray[0] == "deck")
        //     {
        //         //读入卡组
        //         int id = int.Parse(rowArray[1]);
        //         playerDeck[id] = int.Parse(rowArray[2]);
        //     }
        // }

        playerCards = new int[cardStore.cardList.Count];
        playerDeck = new int[cardStore.cardList.Count];
        
        string json;
        string filepath = Application.streamingAssetsPath + "/playerdata.json";
        if(playerType == PlayerType.enemy)
        {
            filepath = Application.streamingAssetsPath + "/enemydata.json";
        }

        if(File.Exists(filepath))
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                json = sr.ReadToEnd();
                sr.Close();
            }

            playerDataclass = JsonUtility.FromJson<PlayerDataClass>(json);
        }
        else
        {
            GenerateData();
        }

        playerOK = playerDataclass.playerOK;
        playerCoins = playerDataclass.playerCoins;
        // Debug.Log(cardStore.cardList.Count);
        for (int i = 0;i < cardStore.cardList.Count;i++)
        {
            playerCards[i] = playerDataclass.playerCards[i];
            playerDeck[i] = playerDataclass.playerDeck[i];
        }
    }

    public void GenerateData()
    {   
        playerDataclass.playerCards = new int[cardStore.cardList.Count];
        playerDataclass.playerDeck = new int[cardStore.cardList.Count];
        playerDataclass.playerOK = false;
        playerDataclass.playerCoins = 0;
        for (int i = 0;i < cardStore.cardList.Count;i++)
        {
            playerDataclass.playerCards[i] = 1;
            playerDataclass.playerDeck[i] = 1;
        }
        
    }
}

