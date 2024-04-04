using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public enum CardSate
    {
        Library,Deck
    }

public class ClickCard : MonoBehaviour,IPointerDownHandler
{
    public DeckManager deckManager;
    public CardSate state;
    public GameObject cardStyle1;

    public GameObject cardStyle2;
    
    // Start is called before the first frame update
    void Start()
    {   
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int id = this.GetComponent<CardDisplay>().card.id;
        // Debug.Log(state.ToString());
        deckManager.UpdateCard(state,id);
    }
}
