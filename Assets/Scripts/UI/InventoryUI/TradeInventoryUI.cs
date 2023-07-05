using UnityEngine;

public class TradeInventoryUI : MonoBehaviour
{
    #region vars
    [HideInInspector]public HeroSlot[] heroSlots;
    [HideInInspector]public InventoryUI inventory;

    [SerializeField] GameObject slotParent;
    [SerializeField] GameObject completeParent;

    [HideInInspector] public TradeSlot[] tradeSlots;
    private HeroSlot draggedSlot;
    #endregion

    //action funcs
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    private void BeginDrag(HeroSlot heroSlot)
    {
        draggedSlot = heroSlot;
    }

    private void Drop(TradeSlot tradeSlot)
    {
        if (draggedSlot != null && draggedSlot.playerHero != null && draggedSlot.playerHero.status == HeroStatus.Idle)
        {
            //assign to empty
            if (tradeSlot.playerHero == null)
            {
                //assign
                PlayerHero temphero = draggedSlot.playerHero;
                int tempID = draggedSlot.slotID;

                AssignHeroToSlot(temphero, tradeSlot.slotID, tempID);


                //update original
                draggedSlot.changeStatus(HeroStatus.Trading);
                draggedSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity, tradeSlot.slotID, -1);

                return;
            }

            //switcheroo
            if (draggedSlot.slotID != tradeSlot.originalSlotReferenceID)
            {
                //assign
                PlayerHero temphero = draggedSlot.playerHero;
                int tempID = draggedSlot.slotID;
                int originalID = tradeSlot.originalSlotReferenceID;

                AssignHeroToSlot(temphero, tradeSlot.slotID, tempID);


                //update original
                draggedSlot.changeStatus(HeroStatus.Trading);
                draggedSlot.updateHero(draggedSlot.playerHero, inventory.CheckForSprite(draggedSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[draggedSlot.playerHero.heroId].rarity, tradeSlot.slotID, -1);


                heroSlots[originalID].changeStatus(HeroStatus.Idle);
            }

        }
    }

    private void RemoveHeroFromSlot(TradeSlot tradeSlot)
    {
        if (tradeSlot.originalSlotReferenceID >= 0)
        {
            heroSlots[tradeSlot.originalSlotReferenceID].tradeReferenceID = -1;
            heroSlots[tradeSlot.originalSlotReferenceID].changeStatus(HeroStatus.Idle);
        }
    }




    //public funcs
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    //init
    public void RefreshTradeUI()
    {
        foreach(TradeSlot slot in tradeSlots)
        {
            PlayerHero tempHero= DatabaseManager._instance.activePlayerData.GetHeroByUniqueId(slot.playerHero.uniqueId);
            if (tempHero != null && tempHero.status == HeroStatus.Trading)
            {
                
            }

            else
            {
                RemoveHeroFromSlot(slot);
                slot.removeHero();
                slot.hideHero();
            }
        }
        
    }

    public void ResetTrade()
    {
        foreach (TradeSlot slot in tradeSlots)
        {
            slot.removeHero();
            slot.hideHero();
        }
    }

    public void InitTradeHub()
    {
        tradeSlots = slotParent.GetComponentsInChildren<TradeSlot>();

        //set up events:
        for (int i = 0; i < tradeSlots.Length; i++)
        {
            tradeSlots[i].OnDropEvent += Drop;
            tradeSlots[i].OnRemoveEvent += RemoveHeroFromSlot;

            tradeSlots[i].slotID = i;
            tradeSlots[i].originalSlotReferenceID = -1;
            

            tradeSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().heroReference = i;
            tradeSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
        }

        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].OnBeginDragEvent += BeginDrag;

            heroSlots[i].slotID = i;
        }
    }

    //inventory assignment
    public void UpdateReference(int referenceID, int ID)
    {
        if (ID >= 0 && ID < tradeSlots.Length)
            tradeSlots[ID].originalSlotReferenceID = referenceID;
    }//helper

    public void AssignHeroToSlot(PlayerHero hero, int ID, int referenceID)
    {
        tradeSlots[ID].updateHero(hero, inventory.CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity, referenceID);
        tradeSlots[ID].showHero();
    }



    //button events
    //-------------------------------------------------------------------------------------------------------------------------------------------------------
    public void RemoveAllHeroesFromTrade()
    {
        foreach (HeroSlot slot in heroSlots)
        {
            if (slot != null && slot.playerHero != null && slot.playerHero.status == HeroStatus.Trading)
                slot.changeStatus(HeroStatus.Idle);
        }

        foreach (TradeSlot slot in tradeSlots)
        {
            RemoveHeroFromSlot(slot);
            slot.hideHero();
            slot.removeHero();
        }

        UIEnablerManager.Instance.SwitchElements("TradeSelect", "General", false); //hide UI
    }

    public bool ConfirmAllHeroesForTrade()
    {
        bool anySlotIsFull = false;

        foreach (TradeSlot slot in tradeSlots)
        {
            if (slot.playerHero != null)
            {
                anySlotIsFull = true;
                break;
            }
        }

        if (anySlotIsFull)
        {
            Debug.Log("do transfer logic");

            return true;
        }

        return false;
    }

    private void DoubleClick(int index)
    {
        AudioManager.PlayEffect("delete");

        RemoveHeroFromSlot(tradeSlots[index]);
        tradeSlots[index].removeHero();
        tradeSlots[index].hideHero();
    }
}
