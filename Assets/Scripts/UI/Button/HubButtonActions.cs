using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum HubState
{
    HeroHub,
    TradeHub,
    DungeonHub
}

public enum ProgressState
{
    Empty,
    Pending,
    Done
}

public enum ButtonState
    {
        Focused,
        Unfocused,
        Finished,
    }

public class HubButtonActions : MonoBehaviour
{

    #region vars
    //actual buttons
    [SerializeField] GameObject tradeButton;//base hub buttons
    [SerializeField] GameObject tradeFocusedButton;
    [SerializeField] GameObject tradeReadyButton;
    [SerializeField] GameObject dungeonButton;
    [SerializeField] GameObject dungeonFocusedButton;
    [SerializeField] GameObject dungeonReadyButton;
    [SerializeField] GameObject hubButton;
    [SerializeField] GameObject hubFocusedButton;
    [Space]
    [SerializeField] GameObject ContinueEndTextButton;//reward UI buttons
    [SerializeField] GameObject ContinueHeroPullButton;
    [SerializeField] GameObject DiscardHeroPullButton;
    [SerializeField] GameObject ReleaseHeroPullButton;
    [SerializeField] GameObject ContinueShardButton;
    [SerializeField] GameObject ContinueHeroGrowthButton;
    [SerializeField] GameObject CloseWarningButton;
    [SerializeField] GameObject CancelReleaseButton;
    [SerializeField] GameObject ConfirmReleaseButton;
    [SerializeField] GameObject CannotConfirmReleaseButton;
    [Space]
    [SerializeField] GameObject CloseAddWarning;
    [SerializeField] GameObject SubmitAddHero;
    [SerializeField] GameObject BlockedAddHero;
    [SerializeField] GameObject CancelAddHero;
    [SerializeField] GameObject DontWantAddHero;
    [SerializeField] GameObject YesWantAddHero;
    [SerializeField] GameObject NextHeroExchange;
    [SerializeField] GameObject FinishHeroExchange;
    [SerializeField] GameObject FinishTrade;
    [SerializeField] GameObject ShowBuff;
    [SerializeField] GameObject ContinueTrade;
    [SerializeField] GameObject CancelAdd1;
    [SerializeField] GameObject CancelAdd2;
    [Space]
    [SerializeField] GameObject Cancel1;
    [SerializeField] GameObject Cancel2;
    [Space]
    [SerializeField] GameObject tradeTextGroup;
    [SerializeField] GameObject tradeSungleTextGroup;
    [SerializeField] GameObject dungeonTextGroup;
    [SerializeField] GameObject dungeonSingleTextGroup;
    [Space]
    [SerializeField] Image tradeProgressBar;
    [SerializeField] Image dungeonProgressBar;
    [SerializeField] TextMeshProUGUI tradeProgressTime;
    [SerializeField] TextMeshProUGUI dungeonProgressTime;
    [Space]
    [SerializeField] Image tradeFocusProgressBar;
    [SerializeField] Image dungeonFocusProgressBar;
    [SerializeField] TextMeshProUGUI tradeFocusProgressTime;
    [SerializeField] TextMeshProUGUI dungeonFocusProgressTime;

    [HideInInspector] public HubState currentHubFocus;
    [Space]
    [Space]
    [SerializeField] InventoryUI InventoryUI;
    [SerializeField] TradeInventoryUI TradeInventoryUI;
    [SerializeField] ExchangeHeroes exchangeUI;
    [SerializeField] TradeObserveUpdate tradeObserve;

    public bool isRewarding = false;

    //helper
    private ProgressState tradeState;
    private ProgressState dungeonState;

    private bool IsPulling = false;

    [HideInInspector]public int totalMatchAmount = 0;
    [HideInInspector]public int currentMatchNumber = 0;

    #endregion


    private void Start()
    {

        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickedTrade(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickedDungeon(); });
        hubButton.GetComponent<Button>().onClick.AddListener(() => { ClickedHub(); });
        tradeFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedTrade(); });
        dungeonFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickedFocusedDungeon(); });
        dungeonReadyButton.GetComponent<Button>().onClick.AddListener(() => { ClickedReadyDungeon(); });
        tradeReadyButton.GetComponent<Button>().onClick.AddListener(() => { ClickedReadyTrade(); });

        ContinueHeroGrowthButton.GetComponent<Button>().onClick.AddListener(() => { ContinueHeroGrowth(); });
        ContinueEndTextButton.GetComponent<Button>().onClick.AddListener(() => { ContinueEndText(); });
        ContinueShardButton.GetComponent<Button>().onClick.AddListener(() => { ContinueShards(); });
        ReleaseHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { ReleaseHeroReward(); });
        DiscardHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { DiscardHeroReward(); });
        ContinueHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { ContinueHeroReward(); });

        CancelReleaseButton.GetComponent<Button>().onClick.AddListener(() => { CancelRelease(); });
        Cancel1.GetComponent<Button>().onClick.AddListener(() => { CancelRelease(); });
        Cancel2.GetComponent<Button>().onClick.AddListener(() => { CancelRelease(); });


        ConfirmReleaseButton.GetComponent<Button>().onClick.AddListener(() => { ConfirmRelease(); });
        CloseWarningButton.GetComponent<Button>().onClick.AddListener(() => { CloseWarningPopUp(); });
        CannotConfirmReleaseButton.GetComponent<Button>().onClick.AddListener(() => { CannotConfirmRelease(); });

        CloseAddWarning.GetComponent<Button>().onClick.AddListener(() => { CloseAddWarningConfirm(); });
        CancelAdd1.GetComponent<Button>().onClick.AddListener(() => { AddHeroFinish(); });
        CancelAdd2.GetComponent<Button>().onClick.AddListener(() => { AddHeroFinish(); });
        SubmitAddHero.GetComponent<Button>().onClick.AddListener(() => { AddHeroSubmit(); });
        BlockedAddHero.GetComponent<Button>().onClick.AddListener(() => { CannotAddHeroSubmit(); });
        CancelAddHero.GetComponent<Button>().onClick.AddListener(() => { AddHeroFinish(); });
        DontWantAddHero.GetComponent<Button>().onClick.AddListener(() => { DoNotAddHeroToRoster(); });
        YesWantAddHero.GetComponent<Button>().onClick.AddListener(() => { ShowAddHeroToRoster(); });
        NextHeroExchange.GetComponent<Button>().onClick.AddListener(() => { ContinueExchange(); });
        FinishHeroExchange.GetComponent<Button>().onClick.AddListener(() => { FinishExchange(); });
        FinishTrade.GetComponent<Button>().onClick.AddListener(() => { FinishedTrade(); });
        ContinueTrade.GetComponent<Button>().onClick.AddListener(() => { ContinueToSwipe(); });
        ShowBuff.GetComponent<Button>().onClick.AddListener(() => { ShowBuffUI(); });

        //sound
        FinishHeroExchange.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });

        tradeButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        dungeonButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        hubButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //tradeFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //dungeonFocusedButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //dungeonReadyButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //tradeReadyButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });

        ContinueHeroGrowthButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        ContinueEndTextButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        ContinueShardButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        ReleaseHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        DiscardHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        ContinueHeroPullButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });

        CancelReleaseButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        Cancel1.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        Cancel2.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });


        ConfirmReleaseButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //CloseWarningButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //CannotConfirmReleaseButton.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });

        //CloseAddWarning.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        CancelAdd1.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        CancelAdd2.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        SubmitAddHero.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        //BlockedAddHero.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        CancelAddHero.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        DontWantAddHero.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        YesWantAddHero.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        NextHeroExchange.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        FinishHeroExchange.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        FinishTrade.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });
        ContinueTrade.GetComponent<Button>().onClick.AddListener(() => { ClickSound(); });

        //delevents
        DeleventSystem.DungeonStep += UpdateStates;
        DeleventSystem.DungeonStart += UpdateStates;
        DeleventSystem.DungeonEnd += UpdateStates;
        DeleventSystem.DungeonEvent += UpdateStates;
        DeleventSystem.DungeonRewardFinished += UpdateStates;

        //dungeon cancel? connected to seperate cancel func?
        DeleventSystem.DungeonCancel += CancelDungeonState;


        DeleventSystem.TradeStart += UpdateStates;
        DeleventSystem.TradeEnd += UpdateStates;
        DeleventSystem.TradeStep += UpdateStates;

        //maybe connect to a seperate cancel func?
        DeleventSystem.TradeCancel += CancelTradeState;


        UpdateStates();
    }

    private void ClickSound()
    {
        AudioManager.PlayEffect("click");
    }


    private void UpdateStates()
    {
        tradeState = DatabaseManager.GetTradeState();
        dungeonState = DatabaseManager.GetDungeonRunState();


        //dont need switch
        if (dungeonState == ProgressState.Done)
        {
            dungeonProgressBar.gameObject.SetActive(false);
            dungeonTextGroup.SetActive(false);
            dungeonSingleTextGroup.SetActive(true);

            UpdateDungeonButton(ButtonState.Finished);
        }

        if (tradeState == ProgressState.Done)
        {

            tradeProgressBar.gameObject.SetActive(false);
            tradeTextGroup.SetActive(false);
            tradeSungleTextGroup.SetActive(true);

            UpdateTradeButton(ButtonState.Finished);

            tradeObserve.EndTrade();
        }


    }

    void Update()
    {
        if (DungeonManager._instance.currentCalcRun != null && dungeonState == ProgressState.Pending)
        {
            //set active progress bar
            dungeonProgressBar.gameObject.SetActive(true);
            dungeonTextGroup.SetActive(true);
            dungeonSingleTextGroup.SetActive(false);

            float value = (float)DungeonManager._instance.currentCalcRun.currentStep / (float)DatabaseManager._instance.dungeonData.currentRun.maxSteps;
            dungeonFocusProgressBar.fillAmount = value;
            dungeonProgressBar.fillAmount = value;
            setDungeonText(value, dungeonFocusProgressTime);
            setDungeonText(value, dungeonProgressTime);

        }

        if (tradeState == ProgressState.Pending)
        {
            //set active progress bar
            tradeProgressBar.gameObject.SetActive(true);
            tradeTextGroup.SetActive(true);
            tradeSungleTextGroup.SetActive(false);

            //implement own calc

            float barValue = (float)TradeManager._instance.CurrentStep / (float)TradeManager._instance.TargetStep;

            //recalc in sec
            float textValue = ((float)TradeManager._instance.TargetStep - (float)TradeManager._instance.CurrentStep) / ((float)TradeManager._instance.TargetStep)/6f;


            tradeFocusProgressBar.fillAmount = barValue;
            tradeProgressBar.fillAmount = barValue;
            setTradeText(textValue, tradeFocusProgressTime);
            setTradeText(textValue, tradeProgressTime);

            tradeObserve.UpdateObserve(barValue);
        }
    }

    //
    //cancel funcs
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void CancelDungeonState()
    {
        dungeonState = ProgressState.Empty;

        dungeonProgressBar.gameObject.SetActive(false);
        dungeonTextGroup.SetActive(false);
        dungeonSingleTextGroup.SetActive(true);
        
        UpdateDungeonButton(ButtonState.Unfocused);
        UpdateHubButton(ButtonState.Focused);
    }


    private void CancelTradeState()
    {
        tradeState = ProgressState.Empty;

        tradeProgressBar.gameObject.SetActive(false);
        tradeTextGroup.SetActive(false);
        tradeSungleTextGroup.SetActive(true);

        UpdateTradeButton(ButtonState.Unfocused);
        UpdateHubButton(ButtonState.Focused);

        UpdateHubState(HubState.HeroHub);

    }

    //click checks
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedTrade()
    {
        //check what state the trade is in
        switch (tradeState)
        {
            case ProgressState.Empty:
                //empty
                //------------------------------------

                //check where you are coming from
                switch (currentHubFocus)
                {
                    case HubState.DungeonHub:
                        UIEnablerManager.Instance.EnableCanvas();
                        UIEnablerManager.Instance.SwitchElements("DungeonObserve", "HeroHub", true);
                        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);

                        //button
                        if (dungeonState != ProgressState.Done)
                            UpdateDungeonButton(ButtonState.Unfocused);

                        break;
                    case HubState.HeroHub:
                        UpdateHubButton(ButtonState.Unfocused);
                        break;
                    default:
                        break;
                }

                //always
                UIEnablerManager.Instance.SwitchElements("General", "TradeSelect", true);
                UpdateTradeButton(ButtonState.Focused);


                break;


            case ProgressState.Pending:
                //pending
                //------------------------------------
                //check where you are coming from
                switch (currentHubFocus)
                {
                    case HubState.DungeonHub:

                        //enable background
                        UIEnablerManager.Instance.EnableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("DungeonObserve", "TradeObserve", true);

                        //change buttons
                        UpdateDungeonButton(ButtonState.Unfocused);
                        break;

                    case HubState.HeroHub:
                        //ui
                        UIEnablerManager.Instance.SwitchElements("HeroHub", "TradeObserve", true);
                        UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);

                        //buttons
                        UpdateHubButton(ButtonState.Unfocused);
                        break;

                    default:
                        break;
                }

                //do always
                UpdateTradeButton(ButtonState.Focused);

                //change hub focus state
                currentHubFocus = HubState.TradeHub;

                break;
            default:
                Debug.Log("no trade state");
                break;
        }

        //change focus state
        currentHubFocus = HubState.TradeHub;
    }

    private void ClickedDungeon()
    {

        //check what state the dungeon is in
        switch (dungeonState)
        {
            case ProgressState.Empty:
                //empty
                //------------------------------------
                //check where you coming from

                switch (currentHubFocus)
                {
                    case HubState.HeroHub:
                        UIEnablerManager.Instance.SwitchElements("HeroHub", "DungeonMapSelect", true);

                        UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);


                        break;
                    case HubState.TradeHub:
                        UIEnablerManager.Instance.SwitchElements("TradeObserve", "DungeonMapSelect", true);

                        if (tradeState != ProgressState.Done)
                            UpdateTradeButton(ButtonState.Unfocused);

                        UpdateHubButton(ButtonState.Focused);
                        break;

                    default:
                        break;
                }

                //always
                UIEnablerManager.Instance.DisableElement("General", true);

                break;

            case ProgressState.Pending:
                //pending
                //------------------------------------
                //check where you are coming from
                switch (currentHubFocus)
                {
                    case HubState.HeroHub:
                        //disable background
                        UIEnablerManager.Instance.DisableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("ShardAndBuff", "DungeonObserve", true);
                        UIEnablerManager.Instance.DisableElement("HeroHub", true);

                        //buttons
                        UpdateHubButton(ButtonState.Unfocused);

                        break;
                    case HubState.TradeHub:
                        //disable background
                        UIEnablerManager.Instance.DisableCanvas();

                        //
                        //ui
                        UIEnablerManager.Instance.SwitchElements("TradeObserve", "DungeonObserve", true);

                        //buttons
                        if (tradeState != ProgressState.Done)
                            UpdateTradeButton(ButtonState.Unfocused);

                        break;
                    default:
                        break;
                }

                //do always
                UpdateDungeonButton(ButtonState.Focused);

                break;
            default:
                Debug.Log("no trade state");
                break;
        }

        //change focus state
        currentHubFocus = HubState.DungeonHub;
    }

    private void ClickedHub()
    {
        InventoryUI.UpdateInventory();


        //check where you are coming from
        switch (currentHubFocus)
        {
            case HubState.DungeonHub:
                //enable background
                UIEnablerManager.Instance.EnableCanvas();

                //
                //ui
                UIEnablerManager.Instance.SwitchElements("DungeonObserve", "ShardAndBuff", true);
                UIEnablerManager.Instance.EnableElement("HeroHub", true);

                //buttons
                if (dungeonState != ProgressState.Done)
                    UpdateDungeonButton(ButtonState.Unfocused);


                break;
            case HubState.TradeHub:
                //ui
                UIEnablerManager.Instance.SwitchElements("TradeObserve", "ShardAndBuff", true);
                UIEnablerManager.Instance.EnableElement("HeroHub", true);

                //buttons
                if (tradeState != ProgressState.Done)
                    UpdateTradeButton(ButtonState.Unfocused);


                break;
            default:
                break;
        }

        //always do
        currentHubFocus = HubState.HeroHub;
        UpdateHubButton(ButtonState.Focused);
    }



    //focused
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void ClickedFocusedDungeon()
    {
        //do pop up
        UIEnablerManager.Instance.EnableElement("DungeonCancel", true);
    }

    private void ClickedFocusedTrade()
    {
        //do pop up
        UIEnablerManager.Instance.EnableElement("TradeCancel", true);
    }


    //rewards done
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void ClickedReadyDungeon()
    {
        isRewarding = true;

        //split into more steps with pop ups:
        DungeonManager._instance.EventRewardHeroHandling();
        DungeonManager._instance.EventRewardShardHandling();



        //actually start pop up flow
        UIEnablerManager.Instance.EnableElement("DungeonEnd", true);


        //change buttons and hub focus in bg
        switch (currentHubFocus)
        {
            case HubState.HeroHub:
                UIEnablerManager.Instance.EnableBlur();
                break;
            case HubState.DungeonHub:
                UIEnablerManager.Instance.EnableCanvas();
                UIEnablerManager.Instance.EnableBlur();
                UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);
                UIEnablerManager.Instance.SwitchElements("DungeonObserve", "HeroHub", true);

                UpdateDungeonButton(ButtonState.Unfocused);
                UpdateHubButton(ButtonState.Focused);
                break;
            case HubState.TradeHub:
                UIEnablerManager.Instance.EnableBlur();
                UIEnablerManager.Instance.SwitchElements("TradeObserve", "ShardAndBuff", true);
                UIEnablerManager.Instance.EnableElement("HeroHub", true);

                //buttons
                if (tradeState != ProgressState.Done)
                    UpdateTradeButton(ButtonState.Unfocused);

                else
                    UpdateTradeButton(ButtonState.Finished);
                break;
            default:
                break;
        }

    }



    //pop up flow
    //
    //reward flow pop ups
    private void ContinueEndText()
    {
        UIEnablerManager.Instance.DisableElement("DungeonEnd", true);

        if (DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
            UIEnablerManager.Instance.EnableElement("ShardReward", true);

        else
            UIEnablerManager.Instance.EnableElement("HeroGrowth", true);
    }

    private void ContinueShards()
    {
        UIEnablerManager.Instance.EnableBlur();
        UIEnablerManager.Instance.SwitchElements("ShardReward", "HeroGrowth", true);
    }

    private void ContinueHeroGrowth()
    {
        UIEnablerManager.Instance.EnableBlur();

        UIEnablerManager.Instance.SwitchElements("HeroGrowth", "HeroPull", true);
    }

    private void ContinueHeroReward()
    {
        DungeonManager._instance.AddRewardHeroToInventory();

        //update inventory UI
        InventoryUI.ResetExploring();
        InventoryUI.UpdateInventory();

        //close pop up flow
        UIEnablerManager.Instance.DisableElement("HeroPull", true);

        //wrap up
        UIEnablerManager.Instance.DisableBlur();

        DungeonManager._instance.WrapUpDungeon();
        if (DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;

        isRewarding = false;
    }

    private void ReleaseHeroReward()
    {
        InventoryUI.DoRelease = true;

        UIEnablerManager.Instance.DisableBlur();
        UIEnablerManager.Instance.DisableElement("HeroPull", true);
        UIEnablerManager.Instance.SwitchElements("General", "ReleaseCancel", false);
    }

    private void DiscardHeroReward()
    {
        DungeonManager._instance.DiscardRewardHero();
        UIEnablerManager.Instance.DisableElement("HeroPull", true);

        //update inventory UI
        InventoryUI.ResetExploring();
        InventoryUI.UpdateInventory();

        //wrap up
        UIEnablerManager.Instance.DisableBlur();


        DungeonManager._instance.WrapUpDungeon();
        if (DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;

        isRewarding = false;

    }




    //
    //release flow pop ups
    private void CancelRelease()
    {
        UIEnablerManager.Instance.EnableBlur();

        InventoryUI.DoRelease = false;


        UIEnablerManager.Instance.SwitchElements("ReleaseCancel", "General", true);
        UIEnablerManager.Instance.EnableElement("HeroPull", true);
    }

    private void ConfirmRelease()
    {
        InventoryUI.DoRelease = false;

        //switch old hero with pulled hero
        DatabaseManager._instance.activePlayerData.ReleaseHero(InventoryUI.releaseHero.uniqueId, false);
        DungeonManager._instance.AddRewardHeroToInventory();


        //update inventory UI
        InventoryUI.ResetExploring();
        InventoryUI.UpdateInventory();


        UIEnablerManager.Instance.SwitchElements("ReleaseSubmit", "General", true);

        //wrap up
        UIEnablerManager.Instance.DisableBlur();


        DungeonManager._instance.WrapUpDungeon();
        if (DeleventSystem.DungeonRewardFinished != null)
        {
            DeleventSystem.DungeonRewardFinished();
        }

        UpdateDungeonButton(ButtonState.Unfocused);
        currentHubFocus = HubState.HeroHub;

        isRewarding = false;

    }

    private void CannotConfirmRelease()
    {
        UIEnablerManager.Instance.EnableElement("ReleaseWarning", true);
    }

    private void CloseWarningPopUp()
    {
        UIEnablerManager.Instance.DisableElement("ReleaseWarning", true);
    }



    //tradedone
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private bool showsshards = false;

    [HideInInspector]public bool oneMatch;
    [HideInInspector]public bool allMatch;
    [HideInInspector]public List<Match> allMatchList = new List<Match>();
    [HideInInspector]public List<int[]> allMatchListPotential = new List<int[]>();


    private void ClickedReadyTrade()
    {
        //helper bool
        IsPulling = true;

        //fetch data
        UIEnablerManager.Instance.EnableElement("WaitingForTrade", true);
        TradeManager._instance.PullTradeOffers(OnPullTradeOffers);
    }

    //wait until data fetched
    private void OnPullTradeOffers()
    {
        tradeState = ProgressState.Empty;

        if (IsPulling)
        { 
            //UIEnablerManager.Instance.DisableElement("WaitingForTrade",true);

            allMatchList.Clear();
            allMatchListPotential.Clear();

            //change buttons and hub focus in bg
            switch (currentHubFocus)
            {
                case HubState.HeroHub:
                    UIEnablerManager.Instance.EnableCanvas();
                    break;
                case HubState.DungeonHub:
                    UIEnablerManager.Instance.EnableCanvas();
                    UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);
                    UIEnablerManager.Instance.SwitchElements("DungeonObserve", "HeroHub", true);

                    UpdateDungeonButton(ButtonState.Unfocused);
                    UpdateHubButton(ButtonState.Focused);
                    break;
                case HubState.TradeHub:
                    UIEnablerManager.Instance.EnableCanvas();
                    UIEnablerManager.Instance.SwitchElements("TradeObserve", "ShardAndBuff", true);
                    UIEnablerManager.Instance.EnableElement("HeroHub", true);

                    //buttons
                    if (dungeonState != ProgressState.Done )
                        UpdateDungeonButton(ButtonState.Unfocused);

                    else
                        UpdateDungeonButton(ButtonState.Finished);
                    break;
                default:
                    break;
            }


            UIEnablerManager.Instance.EnableBlur();

            //helper bools
            oneMatch = false;
            allMatch = true;

            totalMatchAmount = 0;

            //check how many matches
            foreach (Match match in TradeManager._instance.GetTradingResults())
            {
                if (match.matchedOffer != null)
                {
                    oneMatch = true;
                    totalMatchAmount++;
                    allMatchList.Add(match);

                    allMatchListPotential.Add(match.GetBuffDiff());
                }

                else
                {
                    allMatch = false;
                }
            }


            currentMatchNumber = 0;

            exchangeUI.totalMatch = totalMatchAmount;
            exchangeUI.currentMatch = currentMatchNumber;
            exchangeUI.allMatch = allMatch;
            exchangeUI.allMatchList = allMatchList;
            exchangeUI.allMatchListPotential = allMatchListPotential;


                //actually start pop up flow
                if (allMatch)
                {
                    UIEnablerManager.Instance.EnableElement("ExchangeHeroes", true);
                    NextHeroExchange.GetComponent<Button>().enabled = true;
                    FinishHeroExchange.GetComponent<Button>().enabled = true;
                    FinishTrade.GetComponent<Button>().enabled = true;

                }


            else if (oneMatch)
                {
                    UIEnablerManager.Instance.EnableElement("ExchangeHeroes", true);
                    NextHeroExchange.GetComponent<Button>().enabled = true;
                    FinishHeroExchange.GetComponent<Button>().enabled = true;
                    FinishTrade.GetComponent<Button>().enabled = true;
                }

            //no matches
            else
                {

                UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);

                if (DatabaseManager._instance.tradeData.ownOffers.Count < 4)
                    {
                        UIEnablerManager.Instance.DisableBlur();
                        UIEnablerManager.Instance.EnableElement("AddHeroToTrade", true);
                        UIEnablerManager.Instance.SwitchElements("General", "AddHeroDone", true);
                        UIEnablerManager.Instance.EnableElement("AddHero", true);
                        //UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);
                }

                    else
                    {
                        ContinueToSwipe();
                    }

                IsPulling = false;
            }
        }
    }



    //
    //pop up flow trade
    private void ContinueExchange()
    {
        NextHeroExchange.GetComponent<Button>().enabled = false;

        currentMatchNumber++;
        exchangeUI.currentMatch = currentMatchNumber;

        StartCoroutine(DisableEnableExchange());
    }

    private void FinishExchange()
    {
        UIEnablerManager.Instance.DisableElement("BuffReward", true);


        FinishHeroExchange.GetComponent<Button>().enabled = false;

        UIEnablerManager.Instance.DisableElement("ExchangeHeroes", true);


        //update logic
        TradeManager._instance.ApplySuccessfulTrades();

        TradeInventoryUI.RefreshTradeUI();
        InventoryUI.UpdateInventory();

        //ui
        UIEnablerManager.Instance.DisableBlur();
        UIEnablerManager.Instance.DisableElement("ExchangeHeroes", true);
        UIEnablerManager.Instance.EnableElement("AddHeroToTrade", true);
        UIEnablerManager.Instance.SwitchElements("General", "AddHeroDone", true);
        UIEnablerManager.Instance.EnableElement("AddHero", true);

        currentHubFocus = HubState.TradeHub;


    }


    //add more heroes to trade
    int addedOffers = 0;

    private void DoNotAddHeroToRoster()
    {
        //UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);


        InventoryUI.DoAdd = false;

        //go to trade swipe
        UIEnablerManager.Instance.DisableElement("AddHeroToTrade", true);

        //disable addhero ui
        UIEnablerManager.Instance.DisableElement("AddHero", false);
        UIEnablerManager.Instance.DisableElement("AddHeroDone", false);
        UIEnablerManager.Instance.DisableElement("AddHeroSubmit", false);
        UIEnablerManager.Instance.DisableElement("AddHeroBlocked", false);
        //send data
        ContinueToSwipe();
    }

    private void ShowAddHeroToRoster()
    {
        addedOffers = 0;

        InventoryUI.DoAdd = true;

        UIEnablerManager.Instance.DisableElement("AddHeroToTrade", true);
        UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);
    }

    private void CannotAddHeroSubmit()
    {
        UIEnablerManager.Instance.EnableElement("AddWarning", true);
    }

    private void CloseAddWarningConfirm()
    {
        UIEnablerManager.Instance.DisableElement("AddWarning", true);
    }


    private void AddHeroSubmit()
    {
        addedOffers++;

        TradeSlot tradeSlot = null;


        foreach (TradeSlot slot in TradeInventoryUI.tradeSlots)
        {
            if (slot.playerHero == null)
            {
                tradeSlot = slot;
                break;
            }
        }

        if (tradeSlot == null)
            return;

        TradeInventoryUI.AssignHeroToSlot(InventoryUI.addHero, tradeSlot.slotID, InventoryUI.addHeroSlotId);

        //upload to google
        PlayerHero[] playerHeroestoAdd = { InventoryUI.addHero };
        TradeManager._instance.UploadOffer(playerHeroestoAdd);

        InventoryUI.heroSlots[InventoryUI.addHeroSlotId].changeStatus(HeroStatus.Trading);
        InventoryUI.heroSlots[InventoryUI.addHeroSlotId].updateHero(InventoryUI.heroSlots[InventoryUI.addHeroSlotId].playerHero, InventoryUI.CheckForSprite(InventoryUI.heroSlots[InventoryUI.addHeroSlotId].playerHero), DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[InventoryUI.heroSlots[InventoryUI.addHeroSlotId].playerHero.heroId].rarity, tradeSlot.slotID, -1);

        TradeInventoryUI.AssignHeroToSlot(InventoryUI.heroSlots[InventoryUI.addHeroSlotId].playerHero, tradeSlot.slotID, InventoryUI.heroSlots[InventoryUI.addHeroSlotId].playerHero.invIndex);

        if (DatabaseManager._instance.tradeData.ownOffers.Count + addedOffers >= 4)
            AddHeroFinish();

        else
        {
            UIEnablerManager.Instance.EnableElement("AddHeroDone", false);
            UIEnablerManager.Instance.DisableElement("AddHeroSubmit", false);
            UIEnablerManager.Instance.DisableElement("AddHeroBlocked", false);
        }
    }

   private void AddHeroFinish()
    {
        //close addhero ui and go to swipe
        InventoryUI.DoAdd = false;

        UIEnablerManager.Instance.DisableElement("General", false);
        UIEnablerManager.Instance.DisableElement("AddHeroDone", false);
        UIEnablerManager.Instance.DisableElement("AddHeroSubmit", false);
        UIEnablerManager.Instance.DisableElement("AddHeroBlocked", false);
        UIEnablerManager.Instance.DisableElement("AddHero", true);
        UIEnablerManager.Instance.DisableElement("HeroHub", true);

        ContinueToSwipe();
    }

    //
    private void ContinueToSwipe()
    {
        InventoryUI.UpdateInventory();

        //update trade inventory
        InventoryUI.DoAdd = false;

        //go to swipe inventory
        UIEnablerManager.Instance.DisableElement("HeroHub", true);
        UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);
        UIEnablerManager.Instance.EnableElement("TradeSwipe", true);
        UIEnablerManager.Instance.DisableElement("General", true);
        UIEnablerManager.Instance.EnableElement("WaitingForTrade", true);
    }


    //no more heroes in trade inventory
    private void FinishedTrade()
    {
        UIEnablerManager.Instance.DisableElement("BuffReward", true);
        UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);

        UIEnablerManager.Instance.DisableBlur();
        UIEnablerManager.Instance.DisableElement("ExchangeHeroes", true);

        FinishTrade.GetComponent<Button>().enabled = false;

        TradeManager._instance.ApplySuccessfulTrades();

        tradeState = ProgressState.Empty;

        InventoryUI.DoAdd = false;

        InventoryUI.ResetTrade();
        InventoryUI.UpdateInventory();

        UpdateTradeButton(ButtonState.Unfocused);
        UpdateHubButton(ButtonState.Focused);

        currentHubFocus = HubState.HeroHub;
    }


    private void ShowBuffUI()
    {
        showsshards = true;

        UIEnablerManager.Instance.EnableElement("BuffReward", true);
    }

    //update buttons
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //
    private void setDungeonText(float value, TextMeshProUGUI textMesh)
    {
        int recalcValue = (int)(value * 100f);

        textMesh.text = $"{recalcValue} %";
    }

    private void setTradeText(float value, TextMeshProUGUI textMesh)
    {
        int recalcValue = (int)(value * 100f);

        if (recalcValue != 0)
            textMesh.text = $"{recalcValue} min";

        else
        {
           int secCalc = (int)(value * 100f * 60f);
           textMesh.text = $"{secCalc} sec";

        }
    }
    

    public void UpdateTradeButton(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Unfocused:
                tradeFocusedButton.SetActive(false);
                tradeReadyButton.SetActive(false);
                tradeButton.SetActive(true);
                break;
            case ButtonState.Focused:
                tradeReadyButton.SetActive(false);
                tradeButton.SetActive(false);
                tradeFocusedButton.SetActive(true);
                break;
            case ButtonState.Finished:
                tradeFocusedButton.SetActive(false);
                tradeButton.SetActive(false);
                tradeReadyButton.SetActive(true);
                break;
            default:
                Debug.Log("using unknown button state to update trade button");
                break;
        }
    }

    public void UpdateDungeonButton(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Unfocused:
                dungeonReadyButton.SetActive(false);
                dungeonFocusedButton.SetActive(false);
                dungeonButton.SetActive(true);
                break;
            case ButtonState.Focused:
                dungeonReadyButton.SetActive(false);
                dungeonButton.SetActive(false);
                dungeonFocusedButton.SetActive(true);
                break;
            case ButtonState.Finished:
                dungeonFocusedButton.SetActive(false);
                dungeonButton.SetActive(false);
                dungeonReadyButton.SetActive(true);
                break;
            default:
                Debug.Log("using unknown button state to update dungeon button");
                break;
        }
    }

    public void UpdateHubButton(ButtonState state)
    {
        switch (state)
        {
            case ButtonState.Unfocused:
                hubFocusedButton.SetActive(false);
                hubButton.SetActive(true);
                break;
            case ButtonState.Focused:
                hubButton.SetActive(false);
                hubFocusedButton.SetActive(true);
                break;
            default:
                Debug.Log("using unknown button state to update trade button");
                break;
        }
    }
    
    public void UpdateHubState(HubState state)
    {
        currentHubFocus = state;
    }



    IEnumerator DisableEnableExchange()
    {
        if(showsshards)
        {
            UIEnablerManager.Instance.DisableElement("BuffReward", true);
            showsshards = false;
        }

        UIEnablerManager.Instance.DisableElement("ExchangeHeroes", false);

        yield return new WaitForSeconds(.6f);

        UIEnablerManager.Instance.EnableElement("ExchangeHeroes", true);

        NextHeroExchange.GetComponent<Button>().enabled = true;
        FinishHeroExchange.GetComponent<Button>().enabled = true;
        FinishTrade.GetComponent<Button>().enabled = true;

    }
}

