using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeSwipeActions : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject sendButton;
    [SerializeField] GameObject matchButton;
    [SerializeField] GameObject unmatchButton;
    [SerializeField] GameObject cancelFetchDataButton;
    [SerializeField] GameObject noMatchButton;



    [SerializeField]SwipeInventory swipeInventory;
    [SerializeField]TradeInventoryUI tradeInventory;

    [SerializeField] ScrollSnapHero snapHero;

    [SerializeField] HubButtonActions hub;

    //hero list of trades

    #endregion


    private void Awake()
    {
        cancelButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
        matchButton.GetComponent<Button>().onClick.AddListener(() => { ClickedMatch(); });
        unmatchButton.GetComponent<Button>().onClick.AddListener(() => { ClickedUnmatch(); });
        sendButton.GetComponent<Button>().onClick.AddListener(() => { ClickedSend(); });
        nextButton.GetComponent<Button>().onClick.AddListener(() => { ClickedNext(); });
        //cancelFetchDataButton.GetComponent<Button>().onClick.AddListener(() => { ClickedWaitCancel(); });
        noMatchButton.GetComponent<Button>().onClick.AddListener(() => { NoMatchFinished(); });
    }

    private void Start()
    {
        foreach (SwipeSlot swipeSlot in swipeInventory.swipeSlots)
        {
            swipeSlot.OnClickEvent += FocuseHero;
            swipeSlot.OnDoubleClickEvent += UnfocusHero;
        }

    }

    private void OnPullTradeOffers()
    {

        if (DatabaseManager._instance.tradeData.GetNumberOFOpenOffers() >= 1)
        {
            UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);
            UIEnablerManager.Instance.DisableBlur();

            if (DatabaseManager._instance.tradeData.GetNumberOFOpenOffers() > 1)
            {
                nextButton.SetActive(true);
                sendButton.SetActive(false);
            }

            else
            {
                sendButton.SetActive(true);
                nextButton.SetActive(false);
            }

            matchButton.SetActive(false);
            unmatchButton.SetActive(false);
            cancelButton.SetActive(true);
        }

        else
        {
            UIEnablerManager.Instance.EnableElement("NoTradeFound", true);
            UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);
            UIEnablerManager.Instance.DisableBlur();

            //go to observe
            UIEnablerManager.Instance.SwitchElements("TradeSwipe", "TradeObserve", false);
            UIEnablerManager.Instance.EnableElement("General", false);
            //UIEnablerManager.Instance.EnableElement("HeroHub", false);
            //UIEnablerManager.Instance.EnableElement("ShardAndBuff", false);

            hub.UpdateHubState(HubState.TradeHub);
            hub.UpdateDungeonButton(ButtonState.Unfocused);
            hub.UpdateTradeButton(ButtonState.Focused);
            hub.UpdateHubButton(ButtonState.Unfocused);

            TradeManager._instance.StartTrade(900);
        }
    }

    private void OnEnable()
    {
        TradeManager._instance.PullTradeOffers(OnPullTradeOffers);
        
    }

    //all heroes unfocused
    /*private void ClickedWaitCancel()
    {
        //do GoogleSheetCommunicationTest cancel;

        UIEnablerManager.Instance.SwitchElements("TradeSwipe", "TradeSelect", true);
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);
        UIEnablerManager.Instance.EnableElement("HeroHub", true);

        UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);
    }
    */

    private void ClickedCancel()
    {
        UIEnablerManager.Instance.EnableElement("TradeCancel", true);
    }

    private void ClickedNext()
    {
        AudioManager.PlayEffect("nextMatch");


        List<PlayerHero> playerHeroesToMatch = new List<PlayerHero>();

        foreach (SwipeSlot slot in swipeInventory.swipeSlots)
        {
            if(slot.IsMatched)
            {
                playerHeroesToMatch.Add(slot.playerHero);
            }
        }

        //update blacklist
        DatabaseManager._instance.activePlayerData.AddBlackListEntry(snapHero._tradeOffers[snapHero.GetNearestPage()].playerId, snapHero._tradeOffers[snapHero.GetNearestPage()].heroId);

        if (playerHeroesToMatch.Count > 0)
        {
            TradeManager._instance.UpdateOffer(snapHero._tradeOffers[snapHero.GetNearestPage()].offerId, playerHeroesToMatch.ToArray());
        }

        swipeInventory.UnmatchAll();
        unmatchButton.SetActive(false);
        matchButton.SetActive(false);
        cancelButton.SetActive(true);

        if(snapHero.GetNearestPage() == snapHero._pageCount - 2)
        {
            nextButton.SetActive(false);
            sendButton.SetActive(true);
        }
    }

    //final
    private void ClickedSend()
    {
        AudioManager.PlayEffect("send");


        List<PlayerHero> playerHeroesToMatch = new List<PlayerHero>();

        foreach (SwipeSlot slot in swipeInventory.swipeSlots)
        {
            if (slot.IsMatched)
            {
                playerHeroesToMatch.Add(slot.playerHero);
            }
        }

        //update blacklist
        DatabaseManager._instance.activePlayerData.AddBlackListEntry(snapHero._tradeOffers[snapHero.GetNearestPage()].playerId, snapHero._tradeOffers[snapHero.GetNearestPage()].heroId); 

        if (playerHeroesToMatch.Count > 0)
        {
            TradeManager._instance.UpdateOffer(snapHero._tradeOffers[snapHero.GetNearestPage()].offerId, playerHeroesToMatch.ToArray());
        }

        TradeManager._instance.StartTrade(900);

        //go to observe
        UIEnablerManager.Instance.SwitchElements("TradeSwipe", "TradeObserve", true);
        UIEnablerManager.Instance.EnableElement("General", true);
        //UIEnablerManager.Instance.EnableElement("HeroHub", true);

        hub.UpdateHubState(HubState.TradeHub);
        hub.UpdateTradeButton(ButtonState.Focused);
        //hub.UpdateDungeonButton(ButtonState.Unfocused);
        hub.UpdateHubButton(ButtonState.Unfocused);
    }


    private void NoMatchFinished()
    {
        UIEnablerManager.Instance.DisableElement("NoTradeFound", true);
        TradeManager._instance.StartTrade(900);
    }

    //hero focused
    private void ClickedMatch()
    {
        AudioManager.PlayEffect("match");

        if(swipeInventory.swipeIndex != -1)
        {
            matchButton.SetActive(false);
            unmatchButton.SetActive(true);

            //visuals
            swipeInventory.swipeSlots[swipeInventory.swipeIndex].matchHero();

            matchButton.SetActive(false);
            unmatchButton.SetActive(true);
        }
    }

    private void ClickedUnmatch()
    {
        AudioManager.PlayEffect("match");

        if (swipeInventory.swipeIndex != -1)
        {
            matchButton.SetActive(true);
            unmatchButton.SetActive(false);

            //visuals
            swipeInventory.swipeSlots[swipeInventory.swipeIndex].unmatchHero();
        }
    }

    //focus hero
    private void FocuseHero(int index)
    {
        //assign hero
        swipeInventory.swipeIndex = index;

        //set focused
        cancelButton.SetActive(false);

        //check hero state
        if(swipeInventory.swipeSlots[index].IsMatched)
        {
            matchButton.SetActive(false);
            unmatchButton.SetActive(true);
        }

        else
        {
            matchButton.SetActive(true);
            unmatchButton.SetActive(false);
        }
        
    }


    private void UnfocusHero(int index)
    {
        //unassign hero
        swipeInventory.swipeIndex = -1;

        //set unfocused
        cancelButton.SetActive(true);

        //does not matter
        matchButton.SetActive(false);
        unmatchButton.SetActive(false);
    }


}
