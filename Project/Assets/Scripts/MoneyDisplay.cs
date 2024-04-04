using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{   
    public Text money;
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        DisplayMoney();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayMoney()
    {  //读入金币
        money.text = "Your Money:" + " " + playerData.playerCoins.ToString();
    }
}
