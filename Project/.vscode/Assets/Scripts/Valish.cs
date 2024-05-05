using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Valish : MonoSingleton<Valish>
{       
    public Image image;
    public TMP_Text text;
    public GameObject prefab;
    private Vector3 a = new Vector3(960, 540, 0); 
    private Quaternion b = new Quaternion(0, 0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CraeatTips(string _string,GameObject canvas)
    {   
        if(!prefab.activeSelf) prefab.SetActive(true);

        text.DOColor(Color.black,0);
        image.DOColor(Color.white,0);
        prefab.transform.DOMoveY(540,0);
        prefab.transform.SetParent(canvas.transform, false);

        text.text = _string;
        text.DOColor(Color.clear,10.0f);
        image.DOColor(Color.clear,10.0f);
        prefab.transform.DOMoveY(580,5.0f);
    }

    public void Destroy()
    {
        Destroy(prefab);
    }
}
