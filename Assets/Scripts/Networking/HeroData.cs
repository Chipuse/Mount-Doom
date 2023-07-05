using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerHero
{
    public string heroId;
    public HeroStatus status;

    public int uniqueId;

    public int pVal;
    public int pPot;
    public int mVal;
    public int mPot;
    public int sVal;
    public int sPot;

    public string lastOwner;
    public string origOwner;
    public int invIndex;
    public int traded;
    public int runs;

    public int CalcGrowth(int _amount, StatType _statType, DungeonType _dType = DungeonType.basic)
    {
        int result = _amount;
        switch (_dType)
        {
            case DungeonType.basic:
                result = (int)((float)result * 2.0f);
                break;
            case DungeonType.doom:
                result = (int)((float)result * 3.0f);
                break;
            default:
                break;
        }
        if (result < DungeonManager.PityGrowth)
            result = DungeonManager.PityGrowth;
        switch (_statType)
        {
            case StatType.physical:
                if (result > pPot / DungeonManager.MaxGrowth)
                    result = pPot / DungeonManager.MaxGrowth;

                if (pVal + result >= pPot)
                {
                    result = pPot - pVal;
                }
                else if (pVal + result <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].pMin)
                {
                    result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].pMin - pVal;
                }
                break;
            case StatType.magical:
                if (result > mPot / DungeonManager.MaxGrowth)
                    result = mPot / DungeonManager.MaxGrowth;

                if (mVal + result >= mPot)
                {
                    result = mPot - mVal;
                }
                else if (mVal + result <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].mMin)
                {
                    result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].mMin - mVal;
                }
                break;
            case StatType.social:
                if (result > sPot / DungeonManager.MaxGrowth)
                    result = sPot / DungeonManager.MaxGrowth;

                if (sVal + result >= sPot)
                {
                    result = sPot - sVal;
                }
                else if (sVal + result <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].sMin)
                {
                    result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].sMin - sVal;
                }
                break;
            default:
                break;
        }

        

        return result;
    }

    public void ApplyGrowth(int _pGrowth, int _mGrowth, int _sGrowth, DungeonType dType = DungeonType.basic)
    {
        pVal += CalcGrowth( _pGrowth, StatType.physical, dType);
        
        mVal += CalcGrowth(_mGrowth, StatType.magical, dType);
        if (mVal >= mPot)
        {
            mVal = mPot;
        }
        else if (mVal <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].mMin)
        {
            mVal = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].mMin;
        }
        sVal += CalcGrowth(_sGrowth, StatType.social, dType);
        if (sVal >= sPot)
        {
            sVal = sPot;
        }
        else if (sVal <= DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].sMin)
        {
            sVal = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId].sMin;
        }
    }
}

[System.Serializable]
public class IncomingHeroData
{
    public DefaultHero[] defaultHeroList;
    public Dictionary<string, DefaultHero> defaultHeroDictionary;

    public void FillDictionary()
    {
        if (defaultHeroList == null)
            return;
        defaultHeroDictionary = new Dictionary<string, DefaultHero>();
        foreach (var hero in defaultHeroList)
        {
            if(!defaultHeroDictionary.ContainsKey(hero.heroId))
                defaultHeroDictionary.Add(hero.heroId, hero);
        }
    }

    public DefaultHero GetDefHeroByIndex(int _index)
    {
        if(_index < defaultHeroList.Length)
        {
            return defaultHeroList[_index];
        }
        return null;
    }

    public int DefHeroIndex(string _heroId) 
    {
        for (int i = 0; i < defaultHeroList.Length; i++)
        {
            if (defaultHeroList[i].heroId == _heroId)
                return i;
        }
        return -1;
    }
}

[System.Serializable]
public class DefaultHero
{
    public string heroId;
    public string description;
    public string race;
    public string job;
    public int rarity;
    public string texture;

    public int pDef;
    public int pMin;
    public int pDefPot;
    public int pMaxPot;

    public int mDef;
    public int mMin;
    public int mDefPot;
    public int mMaxPot;

    public int sDef;
    public int sMin;
    public int sDefPot;
    public int sMaxPot;

    public string nodeBuff;
    public string nodeDebuff;
    public string pathAff;
}

public enum StatType
{
    physical,
    magical,
    social
}

public enum HeroRace
{
    Human,
    Bird,
    Lizardfolk
}

public enum HeroJob
{
    Fighter,
    Mage,
    Student
}

public enum HeroStatus
{
    Idle,
    Trading,
    Exploring
}

public enum MapNode
{
    Triangle,
    Star,
    Rectangle
}

public enum PathType
{
    Yellow,
    Black,
    Red
}

[System.Serializable]
public struct Stat
{
    //gets fetched from default Hero lookup
    public float defaultVal;
    public float minVal;
    public float maxVal;
    public float defaultPot;
    public float maxPot;
    
    //changes overtime
    public float currVal;
    public float currPot;
}
