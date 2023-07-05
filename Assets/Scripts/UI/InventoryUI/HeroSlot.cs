using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class HeroSlot : MonoBehaviour ,IDragHandler,IBeginDragHandler,IEndDragHandler, IDropHandler
{
    #region vars
    //basics
    [HideInInspector] public PlayerHero playerHero;
    

    [HideInInspector] public int slotID;
    [HideInInspector] public int tradeReferenceID = -1;
    [HideInInspector] public int exploreReferenceID = -1;

    [SerializeField] public GameObject heroCard;
    [SerializeField] private GameObject disabledCard;
    [SerializeField] private GameObject highlight;

    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject [] rarityGraphics;
    [SerializeField] TMP_Text heroName;
    [SerializeField] Image portrait;

    [SerializeField] GameObject TradeStatusUI;
    [SerializeField] GameObject DungeonStatusUI;
 
    //drag and drop
    bool isDragging = false;
    #endregion

    //events
    public event Action<HeroSlot> OnDragEvent;
    public event Action<HeroSlot> OnBeginDragEvent;
    public event Action<HeroSlot> OnEndDragEvent;
    public event Action<HeroSlot> OnDropEvent;
    public event Action<HeroSlot> OnCancelDragEvent;

    public event Action<int> OnClickEvent;
    //base funcs
    //------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        heroCard.GetComponent<Button>().onClick.AddListener(() => OnClick());
    }

    public void showHero()
    {
        disabledCard.SetActive(false);
        highlight.SetActive(false);
        heroCard.SetActive(true);
    }

    //public funcs
    public void hideHero()
    {
        heroCard.SetActive(false);
        highlight.SetActive(false);
        disabledCard.SetActive(true);
    }

    public void removeHero()
    {
        playerHero = null;
        tradeReferenceID = -1;
        exploreReferenceID = -1;
    }

    public void EnableHighlight()
    {
        highlight.SetActive(true);
    }
    public void DisableHighlight()
    {
        highlight.SetActive(false);
    }

    public void updateHero(PlayerHero hero, Sprite sprite,int rarity, int tradeID, int exploreID)
    {
        playerHero = hero;
        tradeReferenceID = tradeID;
        exploreReferenceID = exploreID;

        portrait.sprite = sprite;
        heroName.text = hero.heroId;

        int spacing = -130;

        foreach (GameObject gameObject in rarityGraphics)
        {
            gameObject.SetActive(false);
        }

        for(int i = 0; i < rarity; i ++)
        {
            spacing += 20;
            rarityGraphics[i].SetActive(true);
        }

        rarityGroup.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

        displayStatus(hero.status);
    }

    public void changeStatus(HeroStatus status)
    {
        if (playerHero != null)
        {
            playerHero.status = status;
            displayStatus(status);
        }

        else
        {
            tradeReferenceID = -1;
            exploreReferenceID = -1;
        }
    }

    private void displayStatus(HeroStatus status)
    {
        switch (status)
        {
            case HeroStatus.Trading:
                TradeStatusUI.SetActive(true);
                DungeonStatusUI.SetActive(false);
                break;
            case HeroStatus.Exploring:
                TradeStatusUI.SetActive(false);
                DungeonStatusUI.SetActive(true);
                break;
            default:
                TradeStatusUI.SetActive(false);
                DungeonStatusUI.SetActive(false);
                break;
        }
    }

    //drag funcs
    //-------------------------------------------------------------------------------------------------------------------------------------------

    public void OnDrag(PointerEventData pointerEventData)
    {
        if (isDragging)// && Input.touchCount == 1)
        {
            OnDragEvent(this);
            return;
        }

        /*if (isDragging)
        { 
            OnCancelDragEvent(this);
            isDragging = false;
        }*/
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if(playerHero != null)// && Input.touchCount == 1)
        {
            OnBeginDragEvent(this);
            isDragging = true;
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (isDragging)
        {
            OnEndDragEvent(this);
            isDragging = false;
        }
    }
    
    public void OnDrop(PointerEventData pointerEventData)
    {
        if (pointerEventData != null)
        {
            OnDropEvent(this);
        }
    }

    public void OnClick()
    {
        OnClickEvent(slotID);
    }
}
