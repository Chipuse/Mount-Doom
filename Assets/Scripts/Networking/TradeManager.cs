using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
            StartCoroutine(AutoplayRoutine());
        }
        else
        {
            Destroy(this);
        }
    }

    public static bool SuccesfullTrades()
    {
        if(DatabaseManager.CheckDatabaseValid() && DatabaseManager._instance.tradeData != null && DatabaseManager._instance.tradeData.tradeOffers.Count > 0 )
        {
            //Do the check
            DatabaseManager._instance.tradeData.UpdateOwnOffers();
            foreach (var item in DatabaseManager._instance.tradeData.ownOffers)
            {
                if (item.available != "" && item.playerId == DatabaseManager._instance.activePlayerData.playerId)
                    return true;
            }
        }
        return false;
    }

    private bool AutoPLay = true;
    public float AutoPlayWaitTimeSec = 1.0f;

    public int TargetStep = 900;
    public int CurrentStep = 0;
    bool done = false;

    public ProgressState GetProgressState()
    {
        ProgressState result = ProgressState.Empty;
        if(DatabaseManager._instance.tradeData.ownOffers != null && DatabaseManager._instance.tradeData.ownOffers.Count > 0)
        {
            result = ProgressState.Pending;
            if(CurrentStep >= TargetStep)
            {
                result = ProgressState.Done;
            }
        }
        return result;
    }

    public void FastForwardToStep(int _targetStep)
    {
        CurrentStep = _targetStep;
        PushManager.ScheduleNotification("Trade results available!", "Check your trade results. Maybe you got something nice.", DateTime.Parse(DatabaseManager._instance.activePlayerData.tradeStartDate).ToLocalTime().AddSeconds(TargetStep));
        done = false;
        DeleventSystem.TradeStep?.Invoke();
    }

    public int GetCurrentStep()
    {
        double elapsedSeconds = 0;
        if (DatabaseManager._instance.activePlayerData.tradeStartDate != "" && DatabaseManager._instance.tradeData.ownOffers != null && DatabaseManager._instance.tradeData.ownOffers.Count > 0)
        {
            var bla = DateTime.Parse(DatabaseManager._instance.activePlayerData.tradeStartDate).ToUniversalTime();
            var blub = DateTime.Now.ToUniversalTime();
            var bli = DateTime.UtcNow;
            elapsedSeconds = DateTime.Now.ToUniversalTime().Subtract(DateTime.Parse(DatabaseManager._instance.activePlayerData.tradeStartDate).ToUniversalTime()).TotalSeconds;
            if (AutoPlayWaitTimeSec != 0)
            {
                elapsedSeconds /= AutoPlayWaitTimeSec;
            }
        }
        return (int)elapsedSeconds;
    }

    private void NextStepTrade()
    {
        if(CurrentStep < TargetStep)
        {
            CurrentStep++;
            DeleventSystem.TradeStep?.Invoke();
        }
        else if (done == false)
        {
            done = true;
            AudioManager.PlayEffect("done");
            DeleventSystem.TradeEnd?.Invoke();
        }
    }

    public void StartTrade(int _targetStep = 20)
    {
        done = false;
        TargetStep = _targetStep;
        CurrentStep = 0;
        DatabaseManager._instance.activePlayerData.tradeStarted += 1;
        DatabaseManager._instance.activePlayerData.tradeStartDate = DateTime.Now.ToUniversalTime().ToString("u");
        PushManager.ScheduleNotification("Trade results available!", "Check your trade results. Maybe you got something nice.", DateTime.Parse(DatabaseManager._instance.activePlayerData.tradeStartDate).ToLocalTime().AddSeconds(TargetStep));
        DeleventSystem.TradeStart?.Invoke();
    }
    IEnumerator AutoplayRoutine()
    {
        while (AutoPLay)
        {
            //Update Loop for dungeonRun
            if (DatabaseManager._instance.tradeData != null)
            {
                NextStepTrade();
            }
            yield return new WaitForSeconds(AutoPlayWaitTimeSec);
        }
    }

    public void UploadOffer(PlayerHero heroOne, PlayerHero heroTwo = null, PlayerHero heroThree = null, PlayerHero heroFour = null)
    {
        if(heroFour != null)
            UploadOffer(new PlayerHero[] { heroOne, heroTwo, heroThree, heroFour });
        else if(heroThree != null)
            UploadOffer(new PlayerHero[] { heroOne, heroTwo, heroThree });
        else if(heroTwo != null)
            UploadOffer(new PlayerHero[] { heroOne, heroTwo});
        else if(heroOne != null)
            UploadOffer(new PlayerHero[] { heroOne});
    }

    public void UploadOffer(PlayerHero[] _heroes)
    {
        UploadOfferData _data = new UploadOfferData { heroes = _heroes, playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password } };
        _data.date = DateTime.Now.ToUniversalTime().ToString("u");
        string message = JsonUtility.ToJson(_data);
        var test = JsonUtility.FromJson<PlayerHero[]>(message);
        DatabaseManager._instance.activePlayerData.ResetBlackList();
        ServerCommunicationManager._instance.GetInfo(Request.UploadOffer, message);

        PullTradeOffers();
    }
    [System.Serializable]
    public class UploadOfferData
    {
        public LoginInfo playerInfo;
        public PlayerHero[] heroes;
        public string date;
    }

    public void UpdateOffer(int _tradeOfferId, PlayerHero[] _ownTradeOfferIds)
    {
        //Can only happen when player has units in trade
        //Adds chosen OfferIds from own hero offers to current TradeOffer
        UploadUpdateOffer newUpdate = new UploadUpdateOffer { playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }, uniqueIds = new List<int>() };
        newUpdate.interestingId = _tradeOfferId;
        newUpdate.offerIdMode = false;
        foreach (var item in _ownTradeOfferIds)
        {
            newUpdate.uniqueIds.Add(item.uniqueId);
        }
        string message = JsonUtility.ToJson(newUpdate);
        ServerCommunicationManager._instance.GetInfo(Request.UpdateOffer, message);
    }

    public void UpdateOffer(int _tradeOfferId, int[] _ownTradeOfferIds)
    {
        //Can only happen when player has units in trade
        //Adds chosen OfferIds from own hero offers to current TradeOffer
        UploadUpdateOffer newUpdate = new UploadUpdateOffer { playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password }, uniqueIds = new List<int>() };
        newUpdate.interestingId = _tradeOfferId;
        newUpdate.offerIdMode = true;
        foreach (var item in _ownTradeOfferIds)
        {
            newUpdate.uniqueIds.Add(item);
        }
        string message = JsonUtility.ToJson(newUpdate);
        ServerCommunicationManager._instance.GetInfo(Request.UpdateOffer, message);
    }

    [System.Serializable]
    public class UploadUpdateOffer
    {
        public bool offerIdMode;
        public LoginInfo playerInfo;
        public int interestingId;
        public List<int> uniqueIds;
    }


    public void PullTradeOffers(DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        // gets current status of the online backend concerning trades.
        // 1) when getting new trade offers for "swiping"
        // 2) when checking whether a trade was successful -> happens after x amount of time
        ServerCommunicationManager._instance.DoServerRequest(Request.PullTradeOffers, _simpleEvent, _messageEvent);
    }

    public void OnPullTradeOffers()
    {
        //Analyze and check for trades that have potentially happened
    }

    public void DeleteOffers(TradeOffer[] _tradeOffers, DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        UploadDeleteOffers uploadData = new UploadDeleteOffers { offersToDelete = new List<TradeOffer>(), playerInfo = new LoginInfo { playerId = DatabaseManager._instance.activePlayerData.playerId, password = DatabaseManager._instance.activePlayerData.password } };
        foreach (var item in _tradeOffers)
        {
            uploadData.offersToDelete.Add(item);
            DatabaseManager._instance.tradeData.tradeOffers.Remove(item);
            if(item.available != DatabaseManager._instance.activePlayerData.playerId && item.playerId == DatabaseManager._instance.activePlayerData.playerId)
            {
                foreach (var hero in DatabaseManager._instance.activePlayerData.inventory)
                {
                    if(hero.uniqueId == item.uniqueId)
                    {
                        hero.status = HeroStatus.Idle;
                    }
                }
            }
        }
        DatabaseManager._instance.tradeData.UpdateOwnOffers();
        DatabaseManager._instance.activePlayerData.ResetBlackList();
        string message = JsonUtility.ToJson(uploadData);
        ServerCommunicationManager._instance.GetInfo(Request.DeleteOffers, message, _simpleEvent, _messageEvent);
    }

    [System.Serializable]
    public class UploadDeleteOffers
    {
        public LoginInfo playerInfo;
        public List<TradeOffer> offersToDelete;
    }

    public List<TradeOffer> GetSwipeBatch(int _maxCountOffers = 12)
    {
        List<TradeOffer> result = new List<TradeOffer>();
        while (DatabaseManager._instance.tradeData.openOffers.Count > 0 && result.Count < DatabaseManager._instance.tradeData.openOffers.Count && result.Count < _maxCountOffers)
        {
            TradeOffer temp = DatabaseManager._instance.tradeData.openOffers[UnityEngine.Random.Range(0, DatabaseManager._instance.tradeData.openOffers.Count)];
            if (!result.Contains(temp))
            {
                result.Add(temp);
            }
        }
        return result;
    }

    public void ApplySuccessfulTrades()
    {
        //foreach sucessful trade find own hero that got traded -> replace hero in inventory with new hero -> delete successful offers -> upload player data
        DatabaseManager._instance.tradeData.UpdateOwnOffers();
        List<TradeOffer> toDelete = new List<TradeOffer>();
        foreach (var trade in DatabaseManager._instance.tradeData.ownOffers)
        {
            if (trade.available != "" && trade.playerId == DatabaseManager._instance.activePlayerData.playerId)
            {
                DatabaseManager._instance.activePlayerData.tradeCounter++;
                SwapHeros(trade);
                toDelete.Add(trade);
                DatabaseManager._instance.activePlayerData.AffectRewardTierBuff(1);
            }
        }
        DeleteOffers(toDelete.ToArray());
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData); 

        //Then clear all other own trades
        //CancelOwnTrades();
    }

    public void SwapHeros(TradeOffer _incoming)
    {
        //the unique ID still refers to the old hero
        PlayerHero newHero = HeroCreator.GetHeroById(_incoming.heroId);
        newHero.lastOwner = _incoming.lastOwner;
        newHero.origOwner = _incoming.origOwner;
        newHero.traded = _incoming.traded + 1;
        newHero.runs = _incoming.runs;
        PlayerHero oldHero = null;
        foreach (var hero in DatabaseManager._instance.activePlayerData.inventory)
        {
            if (hero.uniqueId == _incoming.uniqueId)
            {
                oldHero = hero;
            }
            if (oldHero != null)
                break;
        }
        if(oldHero != null)
        {
            DatabaseManager._instance.activePlayerData.inventory.Remove(oldHero);
            newHero.invIndex = oldHero.invIndex;
            newHero = ApplyTradePotentialBuffs(newHero, oldHero.heroId);
        }
        newHero.uniqueId = -1;
        newHero.status = HeroStatus.Idle;
        DatabaseManager._instance.activePlayerData.inventory.Add(newHero);
        DatabaseManager.ValidateInventory();
    }
    PlayerHero ApplyTradePotentialBuffs(PlayerHero _hero, string _oldHeroId)
    {
        DefaultHero newHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[_hero.heroId];
        DefaultHero oldHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[_oldHeroId];

        int rarityDiff = oldHero.rarity - newHero.rarity;
        if (newHero.rarity != 5 && rarityDiff > 0)
        {
            float potentialStep = (float)(newHero.pMaxPot - newHero.pDefPot) / (float)(5 - newHero.rarity);
            _hero.pPot += (int)(potentialStep * rarityDiff);

            potentialStep = (float)(newHero.mMaxPot - newHero.mDefPot) / (float)(5 - newHero.rarity);
            _hero.mPot += (int)(potentialStep * rarityDiff);

            potentialStep = (float)(newHero.sMaxPot - newHero.sDefPot) / (float)(5 - newHero.rarity);
            _hero.sPot += (int)(potentialStep * rarityDiff);
        }
        return _hero;
    }

    

    public void CancelOwnTrades(DeleventSystem.SimpleEvent _simpleEvent = null, DeleventSystem.MessageEvent _messageEvent = null)
    {
        DeleteOffers(DatabaseManager._instance.tradeData.ownOffers.ToArray());
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData, _simpleEvent, _messageEvent);
    }

    public List<Match> GetTradingResults()
    {
        List<Match> result = new List<Match>();
        foreach (var item in DatabaseManager._instance.tradeData.ownOffers)
        {
            Match temp = new Match();
            temp.ownHero = DatabaseManager._instance.activePlayerData.GetHeroByUniqueId(item.uniqueId);
            if (item.available != "")
                temp.matchedOffer = item;
            if(temp.ownHero != null)
            {
                result.Add(temp);
            }
        }
        return result;
    }
}

public class Match
{
    public PlayerHero ownHero;
    public TradeOffer matchedOffer;

    public int[] GetBuffDiff()
    {
        int[] buffs = new int[] { 0, 0, 0 };
        if (ownHero == null || matchedOffer == null)
            return buffs;
        DefaultHero newHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[matchedOffer.heroId];
        DefaultHero oldHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ownHero.heroId];

        int rarityDiff = oldHero.rarity - newHero.rarity;
        if (newHero.rarity != 5 && rarityDiff > 0)
        {
            float potentialStep = (float)(newHero.pMaxPot - newHero.pDefPot) / (float)(5 - newHero.rarity);
            buffs[0] = (int)(potentialStep * rarityDiff);

            potentialStep = (float)(newHero.mMaxPot - newHero.mDefPot) / (float)(5 - newHero.rarity);
            buffs[1] = (int)(potentialStep * rarityDiff);

            potentialStep = (float)(newHero.sMaxPot - newHero.sDefPot) / (float)(5 - newHero.rarity);
            buffs[2] = (int)(potentialStep * rarityDiff);
        }
        return buffs;
    }

    public int[] GetCalcPotentials()
    {
        int[] potentials = new int[] { 0, 0, 0 };
        if (ownHero == null || matchedOffer == null)
            return potentials;
        DefaultHero newHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[matchedOffer.heroId];
        int[] buffs = GetBuffDiff();
        potentials[0] = newHero.pDefPot + buffs[0];
        potentials[1] = newHero.mDefPot + buffs[1];
        potentials[2] = newHero.sDefPot + buffs[2];
        return potentials;
    }
}
