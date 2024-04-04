using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class CardCounter : MonoBehaviour
{   
    public Text number; 
    private int num = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetNum(int value)
    {   
        num += value;
        if(num == 0)
        {
            Destroy(gameObject);
            return false;
        }
        else
        {
            number.text = num.ToString();
            return true;   
        }
    }
}
