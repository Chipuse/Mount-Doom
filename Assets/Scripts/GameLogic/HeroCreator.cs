using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCreator
{
    public static PlayerHero GetHeroByRewardTier(int _rewardTier)
    {
        if (_rewardTier < 1)
            _rewardTier = 1;
        else if (_rewardTier > DatabaseManager._instance.rewardTable.rewardTiers.Count)
            _rewardTier = DatabaseManager._instance.rewardTable.rewardTiers.Count;
        if (_rewardTier > DatabaseManager._instance.rewardTable.rewardTiers.Count)
        {
            //Fallback will not be hitted anymooooooore
            return GetrandomHero();
        }
        RewardTier currentTier = DatabaseManager._instance.rewardTable.rewardTiers[_rewardTier - 1];
        int ranNum = UnityEngine.Random.Range(1, 101);
        int chosenRarity = 0;
        for (int i = 0; i < currentTier.chances.Count; i++)
        {
            ranNum -= currentTier.chances[i];
            if (ranNum <= 0)
            {
                chosenRarity = i + 1;
                break;
            }
        }
        return GetRandomHeroOfRarity(chosenRarity);
    }

    public static PlayerHero GetHeroById(string _heroId)
    {
        PlayerHero result = null;
        if (DatabaseManager._instance.defaultHeroData.defaultHeroDictionary.ContainsKey(_heroId))
        {
            DefaultHero temp = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[_heroId];
            result = new PlayerHero {
                heroId = _heroId,
                pPot = temp.pDefPot,
                pVal = temp.pDef,
                mPot = temp.mDefPot,
                mVal = temp.mDef,
                sPot = temp.sDefPot,
                sVal = temp.sDef,
                status = HeroStatus.Idle,
                lastOwner = DatabaseManager._instance.activePlayerData.playerId,
                origOwner = DatabaseManager._instance.activePlayerData.playerId,
                traded = 0,
                runs = 0
            };
            
            for (int i = 0; i < DatabaseManager.maxInventorySize; i++)
            {
                result.invIndex = i;
                foreach (var item in DatabaseManager._instance.activePlayerData.inventory)
                {
                    if (item.invIndex == i)
                    {
                        //index already taken move on to the next one
                        result.invIndex = -1;
                        break;
                    }
                }
                if (result.invIndex != -1)
                    break;
            }
        }
        DatabaseManager.ValidatePlayerHero(result);
        return result;
    }

    public static PlayerHero GetRandomHeroOfRarity(int _rarity)
    {
        PlayerHero result = null;
        List<string> tempHeroIdList = new List<string>();
        foreach (var defaultHero in DatabaseManager._instance.defaultHeroData.defaultHeroList)
        {
            if (defaultHero.rarity == _rarity)
                tempHeroIdList.Add(defaultHero.heroId);
        }
        string randomHeroId = tempHeroIdList[UnityEngine.Random.Range(0, tempHeroIdList.Count)];
        result = GetHeroById(randomHeroId);
        return result;
    }

    public static PlayerHero GetrandomHero()
    {
        PlayerHero result = null;
        string randomHeroId = DatabaseManager._instance.defaultHeroData.defaultHeroList[UnityEngine.Random.Range(0, DatabaseManager._instance.defaultHeroData.defaultHeroList.Length)].heroId;
        result = GetHeroById(randomHeroId);
        return result;
    }
}
