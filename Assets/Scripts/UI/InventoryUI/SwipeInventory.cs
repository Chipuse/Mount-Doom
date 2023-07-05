using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeInventory : MonoBehaviour
{
    #region vars
    [SerializeField] TradeInventoryUI tradeInventory;
    [SerializeField] GameObject slotParent;
    [SerializeField] UpdateHeroCard updateHeroCard;
    [HideInInspector] public SwipeSlot[] swipeSlots;

    //swipeslot focused
    private PlayerHero matchHero;
    [HideInInspector] public int swipeIndex = -1;
    #endregion


    private void Awake()
    {
        swipeSlots = slotParent.GetComponentsInChildren<SwipeSlot>();

        int i = 0;

        foreach (SwipeSlot swipeSlot in swipeSlots)
        {
            swipeSlot.OnClickEvent += Click;
            swipeSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().heroReference = i;
            swipeSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
            i++;
        }
    }

    private void OnEnable()
    {
        ResetSwipeHeroes();
        UpdateSwipeHeroes();
    }

    private void UpdateSwipeHeroes()
    {

        foreach (TradeSlot slot in tradeInventory.tradeSlots)
        {
            if (slot.playerHero != null)
            {
                swipeSlots[slot.slotID].updateHero(slot.playerHero, slot.portrait.sprite, slot.slotrarity,slot.originalSlotReferenceID);
                swipeSlots[slot.slotID].slotID = slot.slotID;
                swipeSlots[slot.slotID].showHero();
            }
        }
    }

    private void ResetSwipeHeroes()
    {
         foreach (SwipeSlot slot in swipeSlots)
        {
            slot.hideHero();
            slot.unmatchHero();
        }    
    }


    public void UnmatchAll()
    {
        foreach (SwipeSlot slot in swipeSlots)
        {
            slot.unmatchHero();
        }
    }


    //highlight
    private void Click(int index)
    {
        foreach (SwipeSlot heroSlot in swipeSlots)
        {
            if (heroSlot.playerHero != null)
                heroSlot.disableHighlight();
        }

        swipeSlots[index].enableHighlight();
        matchHero = swipeSlots[index].playerHero;

        swipeIndex = index;
    }
    private void DoubleClick(int index)
    {
        updateHeroCard.UpdateHero(swipeSlots[index].playerHero);
        UIEnablerManager.Instance.EnableElement("HeroCard", true);
    }
}
