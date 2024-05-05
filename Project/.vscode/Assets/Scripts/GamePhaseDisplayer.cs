using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhaseDisplayer : MonoBehaviour
{   
    public Text gamePhase;
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.phaseChange.AddListener(PhaseChange);  //对阶段变化事件增加监听函数
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PhaseChange()
    {
        gamePhase.text = BattleManager.Instance.gamePhase.ToString();
    }
}
