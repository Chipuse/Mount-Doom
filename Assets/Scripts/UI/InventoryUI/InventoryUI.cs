using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class InventoryUI : MonoBehaviour
{

    

    #region vars
    [SerializeField] GameObject PopUpHeroCard;
    
    [SerializeField] TradeInventoryUI tradeInventory;
    [SerializeField] ExploreInventoryUI exploreInventory;
    [SerializeField] GameObject slotParent;
    [HideInInspector] public HeroSlot[] heroSlots;

    [Space]
    [SerializeField] DragHero draggableHero;
    [Space]
    [SerializeField] Sprite fallbackImage;

    private Dictionary<string, Sprite> defaultImageDict = new Dictionary<string, Sprite>(); //this does not exist yet

    private HeroSlot draggedSlot;
    private Vector3 originalPosition;

    private bool isDragging = false;

    private ScrollRect scroll;

    private int amountInTrade = 0;
    private int amountInDungeon = 0;

    [HideInInspector] public PlayerHero releaseHero;
    [HideInInspector] public PlayerHero addHero;
    [HideInInspector] public int addHeroSlotId;
    [HideInInspector] public bool DoRelease;
    [HideInInspector] public bool DoAdd;
    #endregion


    //Init
    //-----------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        heroSlots = slotParent.GetComponentsInChildren<HeroSlot>();
        //init tred inventory
        tradeInventory.inventory = this;
        tradeInventory.heroSlots = heroSlots;

        tradeInventory.InitTradeHub();
        tradeInventory.ResetTrade();

        //init explore inventory
        exploreInventory.inventory = this;
        exploreInventory.heroSlots = heroSlots;

        exploreInventory.InitExploreHub();
        exploreInventory.ResetExplore();

        //get scroll
        scroll = GetComponent<ScrollRect>();

        //set up events:
        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlots[i].OnBeginDragEvent += BeginDrag;
            heroSlots[i].OnCancelDragEvent += CancelDrag;
            heroSlots[i].OnDragEvent += Drag;
            heroSlots[i].OnEndDragEvent += EndDrag;
            heroSlots[i].OnDropEvent += Drop;

            heroSlots[i].slotID = i;

            heroSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().heroReference = i;
            heroSlots[i].heroCard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
            heroSlots[i].OnClickEvent += Click; ;

        }

        defaultImageDict = SpriteStruct.SpriteDictionary;
    }

    private void Start()
    {
        InitInventoryUI();
    }

    public void InitInventoryUI()
    {
        if (DatabaseManager._instance != null && DatabaseManager._instance.activePlayerData != null && DatabaseManager._instance.activePlayerData.inventory != null && heroSlots != null)
        {     
            //reset
            for (int i = 0; i < heroSlots.Length; i++ )
            {
                heroSlots[i].removeHero();
                heroSlots[i].hideHero();
            }
            //assign
            foreach(PlayerHero hero in DatabaseManager._instance.activePlayerData.inventory)
            {
                InitAssignHeroToSlot(hero);
            }
            DatabaseManager._instance.SaveGameDataLocally();
        }

    }//do this somewhere at the start

    private void InitAssignHeroToSlot(PlayerHero hero)
    {
        //catch exeptions
        if(hero.invIndex >= heroSlots.Length)
        {
            Debug.Log("trying to show hero which is in slot that is bigger than the inventory size");
            return;
        }

        if (heroSlots[hero.invIndex].playerHero != null)
        {            
            Debug.Log("trying to show hero which is already shown or assign more heroes to one slot");
            //return;
        }

        if (!DatabaseManager._instance.defaultHeroData.defaultHeroDictionary.ContainsKey(hero.heroId))
        {
            heroSlots[hero.invIndex].removeHero();
            heroSlots[hero.invIndex].hideHero();
            Debug.Log("trying to show hero that does not exist");
            return;
        }


        //
        //default assign
        AssignHeroToSlot(hero, hero.invIndex, -1,-1);//default is -1
        

        //update tradeing and exploring heroes
        InitUpdateTradeHeroes(hero);
        InitUpdateExploreHeroes(hero);
        //should be actually only called when the UI is being enabled and not at the start 
    }


    //update sub inventories
    private void InitUpdateTradeHeroes(PlayerHero hero)
    {
        if (hero.status == HeroStatus.Trading)
        {
            amountInTrade += 1;

            if (amountInTrade > 4)
            {
                Debug.Log("trying to trade too many heroes");
                return;
            }

            tradeInventory.AssignHeroToSlot(hero, amountInTrade - 1, hero.invIndex);
            heroSlots[hero.invIndex].tradeReferenceID = amountInTrade - 1;

            exploreInventory.UpdateReference(hero.invIndex, -1);
            tradeInventory.UpdateReference(hero.invIndex, amountInTrade - 1);
        }
    }

    private void InitUpdateExploreHeroes(PlayerHero hero)
    {
        if (hero.status == HeroStatus.Exploring)
        {
            amountInDungeon += 1;

            if (amountInDungeon > 4)
            {
                Debug.Log("trying to explore too many heroes");
                return;
            }

            exploreInventory.AssignHeroToSlot(hero, amountInDungeon - 1, hero.invIndex);
            heroSlots[hero.invIndex].exploreReferenceID = amountInDungeon - 1;

            tradeInventory.UpdateReference(hero.invIndex, -1);
            exploreInventory.UpdateReference(hero.invIndex, amountInDungeon - 1);
        }
    }


    //update inventory display
    //------------------------------------------------------------------------------------------------------------------------------------------------------

    public void ResetExploring()
    {
        exploreInventory.RemoveAllHeroesFromExplore();
    }

    public void ResetTrade()
    {
        tradeInventory.RemoveAllHeroesFromTrade();
    }

    public void UpdateInventory()
    {
        if (DatabaseManager._instance != null && DatabaseManager._instance.activePlayerData != null && DatabaseManager._instance.activePlayerData.inventory != null && heroSlots != null)
        {
            //reset
            for (int i = 0; i < heroSlots.Length; i++)
            {
                heroSlots[i].removeHero();
                heroSlots[i].hideHero();
            }
            //assign
            foreach (PlayerHero hero in DatabaseManager._instance.activePlayerData.inventory)
            {
                UpdateAssignHeroToSlot(hero);
            }
            DatabaseManager._instance.SaveGameDataLocally();
        }
    }

    private void UpdateAssignHeroToSlot(PlayerHero hero)
    {
        //catch exeptions
        if (hero.invIndex > heroSlots.Length)
        {
            Debug.Log("trying to show hero which is in slot that is bigger than the inventory size");
            return;
        }

        if (heroSlots[hero.invIndex].playerHero != null)
        {
            Debug.Log("trying to show hero which is already shown or assign more heroes to one slot");
            //return;
        }

        if (!DatabaseManager._instance.defaultHeroData.defaultHeroDictionary.ContainsKey(hero.heroId))
        {
            heroSlots[hero.invIndex].removeHero();
            heroSlots[hero.invIndex].hideHero();
            Debug.Log("trying to show hero that does not exist");
            return;
        }


        //
        //default assign
        AssignHeroToSlot(hero, hero.invIndex, -1, -1);//default is -1
    }


    //event funcs
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    private void AssignHeroToSlot(PlayerHero hero, int ID, int tradeID, int exploreID)
    {
        heroSlots[ID].showHero();
        heroSlots[ID].updateHero(hero, CheckForSprite(hero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId].rarity,tradeID,exploreID);

        if(heroSlots[ID].playerHero.status == HeroStatus.Trading)
        {
            tradeInventory.UpdateReference(ID, heroSlots[ID].tradeReferenceID);
        }

        if(heroSlots[ID].playerHero.status == HeroStatus.Exploring)
        {
            exploreInventory.UpdateReference( ID, heroSlots[ID].exploreReferenceID);
        }
    }

    //DragDrop
    private void BeginDrag (HeroSlot heroSlot)
    {
        if(heroSlot != null && heroSlot.playerHero != null && DatabaseManager._instance !=null && DatabaseManager._instance.defaultHeroData != null && DatabaseManager._instance.defaultHeroData.defaultHeroDictionary != null)
        {
            //deactivate scrolling
            scroll.vertical = false;

            //where we started
            isDragging = true;

            draggedSlot = heroSlot;

            draggableHero.gameObject.SetActive(true);


            //set drag represetnation to the item in the slot
            draggableHero.updateDragHero(heroSlot.playerHero, CheckForSprite(heroSlot.playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroSlot.playerHero.heroId].rarity);


            //show dragable hero
            draggableHero.transform.position = Input.mousePosition;
            heroSlot.hideHero();
        }
    }

    private void EndDrag(HeroSlot heroSlot)
    {
        //activate scrolling
        scroll.vertical = true;


        draggableHero.gameObject.SetActive(false);
        heroSlot.showHero();
        isDragging = false;
    }

    private void CancelDrag(HeroSlot heroSlot)
    {
        //activate scrolling
        scroll.vertical = true;

        Debug.Log("cancel not assigned yet");
        heroSlot.showHero();
        isDragging = false;
    }

    private void Drag(HeroSlot heroSlot)
    {
        draggableHero.transform.position = Input.mousePosition;
    }

    private void Drop(HeroSlot heroSlot)
    {
        if (draggedSlot != null && draggedSlot.playerHero != null)
            StartCoroutine(DoDrop(heroSlot));
    }


    //double click

    private void DoubleClick(int index)
    {
        //ToDo
        PopUpHeroCard.GetComponent<UpdateHeroCard>().UpdateHero(heroSlots[index].playerHero);
        UIEnablerManager.Instance.EnableElement("HeroCard",true);
    }

    private void Click(int index)
    {
        //only do if forced release
        if (DoRelease && heroSlots[index].playerHero.status == HeroStatus.Idle)
        {
            foreach(HeroSlot heroSlot in heroSlots)
            {
                if (heroSlot.playerHero != null)
                    heroSlot.DisableHighlight();
            }

            heroSlots[index].EnableHighlight();
            releaseHero = heroSlots[index].playerHero;

            UIEnablerManager.Instance.DisableElement("ReleaseBlocked", false);
            UIEnablerManager.Instance.SwitchElements("ReleaseCancel", "ReleaseSubmit", false);

        }

        else if (DoRelease)
        {
            foreach (HeroSlot heroSlot in heroSlots)
            {
                if (heroSlot.playerHero != null)
                    heroSlot.DisableHighlight();
            }

            heroSlots[index].EnableHighlight();

            UIEnablerManager.Instance.DisableElement("ReleaseSubmit", false);
            UIEnablerManager.Instance.SwitchElements("ReleaseCancel", "ReleaseBlocked", false);

        }

        else if (DoAdd && heroSlots[index].playerHero.status == HeroStatus.Idle)
        {
            foreach (HeroSlot heroSlot in heroSlots)
            {
                if (heroSlot.playerHero != null)
                    heroSlot.DisableHighlight();
            }

            heroSlots[index].EnableHighlight();
            addHero = heroSlots[index].playerHero;
            addHeroSlotId = heroSlots[index].slotID;


            UIEnablerManager.Instance.DisableElement("AddHeroBlocked", false);
            UIEnablerManager.Instance.SwitchElements("AddHeroDone", "AddHeroSubmit", false);
        }

        else if(DoAdd)
        {
            foreach (HeroSlot heroSlot in heroSlots)
            {
                if (heroSlot.playerHero != null)
                    heroSlot.DisableHighlight();
            }

            heroSlots[index].EnableHighlight();


            UIEnablerManager.Instance.DisableElement("AddHeroSubmit", false);
            UIEnablerManager.Instance.SwitchElements("AddHeroDone", "AddHeroBlocked", false);
        }
    }
    

    //helper checks
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    public Sprite CheckForSprite(PlayerHero hero)
    {
        if (defaultImageDict.ContainsKey(hero.heroId))
            return defaultImageDict[hero.heroId];

        return fallbackImage;
    }


    //shitty work around coroutine
    IEnumerator DoDrop(HeroSlot heroSlot)
    {
        while(isDragging)
        {
            yield return null;
        }

        //switch two full fields
        if (heroSlot.playerHero != null && heroSlot != draggedSlot)
        {
            PlayerHero temphero = draggedSlot.playerHero;
            int tempTradeID = draggedSlot.tradeReferenceID;
            int tempExploreID = draggedSlot.exploreReferenceID;


            //switch
            AssignHeroToSlot(heroSlot.playerHero, draggedSlot.slotID, heroSlot.tradeReferenceID, heroSlot.exploreReferenceID);
            //ToDO change slotId in database
            heroSlot.playerHero.invIndex = draggedSlot.slotID;

            AssignHeroToSlot(temphero, heroSlot.slotID, tempTradeID,tempExploreID);
            //ToDO change slotId in database
            temphero.invIndex = heroSlot.slotID;

            //update references
            tradeInventory.UpdateReference(draggedSlot.slotID, tempTradeID);
            tradeInventory.UpdateReference(heroSlot.slotID, heroSlot.tradeReferenceID);

            exploreInventory.UpdateReference(draggedSlot.slotID, tempExploreID);
            exploreInventory.UpdateReference(heroSlot.slotID, heroSlot.exploreReferenceID);
        }

        //assign to empty
        else if (heroSlot.playerHero == null && heroSlot != draggedSlot)
        {
            PlayerHero temphero = draggedSlot.playerHero;
            int tempTradeID = draggedSlot.tradeReferenceID;
            int tempExploreID = draggedSlot.exploreReferenceID;


            //make fromer full slot to empty
            draggedSlot.removeHero();
            draggedSlot.hideHero();

            //assign
            AssignHeroToSlot(temphero, heroSlot.slotID,tempTradeID,tempExploreID);
            //ToDO change slotId in database
            heroSlot.playerHero.invIndex = heroSlot.slotID;


            //update references
            tradeInventory.UpdateReference(draggedSlot.slotID, -1);
            tradeInventory.UpdateReference(heroSlot.slotID, heroSlot.tradeReferenceID);

            exploreInventory.UpdateReference(draggedSlot.slotID, -1);
            exploreInventory.UpdateReference(heroSlot.slotID, heroSlot.exploreReferenceID);
        }
    }
}
