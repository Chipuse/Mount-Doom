using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class TradeSlot : MonoBehaviour, IDropHandler
{
    [HideInInspector] public PlayerHero playerHero;

    [HideInInspector] public int slotID;
    [HideInInspector] public int originalSlotReferenceID = -1;


    [SerializeField] public  GameObject heroCard;
    [SerializeField] private GameObject disabledCard;

    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject[] rarityGraphics;
    [SerializeField] public TMP_Text heroName;
    [SerializeField] public Image portrait;
    [HideInInspector]public int slotrarity { get; private set; }

//events
public event Action <TradeSlot> OnDropEvent;
    public event Action <TradeSlot> OnRemoveEvent;
    
    public void updateHero(PlayerHero hero, Sprite sprite, int rarity, int referenceID)
    {
        slotrarity = rarity;
        playerHero = hero;
        originalSlotReferenceID = referenceID;

        portrait.sprite = sprite;
        heroName.text = hero.heroId;

        int spacing = -130;

        foreach (GameObject gameObject in rarityGraphics)
        {
            gameObject.SetActive(false);
        }

        for (int i = 0; i < rarity; i++)
        {
            spacing += 20;
            rarityGraphics[i].SetActive(true);
        }

        rarityGroup.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
    }

    public void showHero()
    {
        disabledCard.SetActive(false);
        heroCard.SetActive(true);
    }

    //public funcs
    public void hideHero()
    {
        heroCard.SetActive(false);
        disabledCard.SetActive(true);
    }

    public void removeHero()
    {
        OnRemoveEvent(this);
        playerHero = null;
    }


    public void OnDrop(PointerEventData pointerEventData)
    {
        if (pointerEventData != null)
        {
            OnDropEvent(this);
        }
    }
}
