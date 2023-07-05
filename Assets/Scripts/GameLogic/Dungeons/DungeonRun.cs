using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DungeonRun
{
    public bool valid = false; //gets set by DungeonManager when starting or proceeding a run
    public List<PlayerHero> party;
    public string date;
    public int dungeonSeed;
    public DailyDungeon dungeon;
    public int initialRewardTier; //dungeon assigned rt + rewardbuff
    public int maxSteps;
    public List<RandomNum> randomNums;
    public List<int>  otherRandNums;
    
}

[System.Serializable]
public class CalculatedDungeonRun
{
    public CalculatedDungeonRun(DungeonRun _dungeonRun)
    {
        dungeonRun = _dungeonRun;
        ended = false;
        currentStep = 0;
        currentActivity = DungeonActivity.startQuest;
        startActivitySteps = 0;
        remainingActivitySteps = 0;
        int baseReward = 0;        
        if(dungeonRun.dungeon.type == DungeonType.basic)
        {
            baseReward =dungeonRun.initialRewardTier;
        }
        else
        {
            baseReward = dungeonRun.initialRewardTier;
        }
        rewardHealthBar = (baseReward) * 10;
        currentNode = dungeonRun.dungeon.dungeonLayout.startNode;

        pStatGrowth = 0;
        mStatGrowth = 0;
        sStatGrowth = 0;

        nextHero = 0;
        randomNums = new Dictionary<int, int>();
        foreach (RandomNum randNum in _dungeonRun.randomNums)
        {
            randomNums.Add(randNum.step, randNum.num);
        }
    }

    public int otherRandCounter = 0;
    public int OtherRandomNum(int min, int max)
    {
        int result = 0;
        if (DatabaseManager._instance.dungeonData.currentRun.otherRandNums == null)
            DatabaseManager._instance.dungeonData.currentRun.otherRandNums = new List<int>();
        if (DatabaseManager._instance.dungeonData.currentRun.otherRandNums.Count > otherRandCounter)
        {
            result = DatabaseManager._instance.dungeonData.currentRun.otherRandNums[otherRandCounter];
            if (result < min)
                result = min;
            if (result > max)
                result = max;
            otherRandCounter++;
        }
        else
        {
            result = UnityEngine.Random.Range(min, max);
            DatabaseManager._instance.dungeonData.currentRun.otherRandNums.Add(result);
            otherRandCounter = DatabaseManager._instance.dungeonData.currentRun.otherRandNums.Count;
        }           
        return result;
    }

    public int RandomNum(int min, int max)
    {
        return OtherRandomNum(min, max);
        /*
        if (randomNums.ContainsKey(currentStep))
        {
            return randomNums[currentStep];
        }
        else
        {
            // In theory this should only be the case when setting up with calcMaxStep
            int randomNum = UnityEngine.Random.Range(min, max);
            randomNums.Add(currentStep, randomNum);
            DatabaseManager._instance.dungeonData.currentRun.randomNums.Add(new RandomNum { num = randomNum, step = currentStep });
            return randomNum;
        }
        */
    }

    int numRewardHealthLowered = 0;
    public int AffectRewardHealth(int _amount)
    {
        if(_amount >= 0)
        {
            numRewardHealthLowered = 0;
            rewardHealthBar += _amount;
        }
        else
        {            
            if(numRewardHealthLowered < 7)
            {
                rewardHealthBar += _amount;
                numRewardHealthLowered++;
            }
        }
        
        if (rewardHealthBar < 4)
            rewardHealthBar = 4;
        else if (rewardHealthBar >= 100)
            rewardHealthBar = 100;
        if (DungeonManager.events)
            DeleventSystem.RewardHealthChanged?.Invoke();
        return rewardHealthBar;
    }

    public bool Finished()
    {
        return ended;
    }

    Dictionary<int, int> randomNums;

    DungeonRun dungeonRun;
    public bool ended;
    public int currentStep;
    public DungeonActivity currentActivity;
    public int startActivitySteps;
    public int remainingActivitySteps;
    public int rewardHealthBar; //initial rewardbuff *10 -> gets later devided by 10 to get reward tier again
    public DungeonNode currentNode;
    public int pStatGrowth;
    public int mStatGrowth;
    public int sStatGrowth;

    public int nextHero;

    List<LogEntry> dungeonLog = new List<LogEntry>();
    public LogEntry[] dungeonLogArr;
    public void UpdateLog(string _newEntry)
    {
        string replacedText = TextReplacer.ReplaceWordsBulk(_newEntry);
        if (dungeonLog == null)
            dungeonLog = new List<LogEntry>();
        dungeonLog.Add(new LogEntry {step = currentStep, entry = replacedText });
        dungeonLogArr = dungeonLog.ToArray();
        if (DungeonManager.events)
            DeleventSystem.DungeonLog?.Invoke();
        else
        {
            int tempNextHero = nextHero;
            if (DatabaseManager._instance.dungeonData.currentRun.party.Count <= nextHero)
            {
                tempNextHero = 0;
            }
            //PushManager.ScheduleNotification("new DungeonLog", replacedText, DateTime.Parse(DatabaseManager._instance.dungeonData.currentRun.date).ToLocalTime().AddSeconds(currentStep), DatabaseManager._instance.dungeonData.currentRun.party[tempNextHero].heroId);
        }
    }
    public void SetActivitySteps(int _newSteps)
    {
        startActivitySteps = _newSteps;
        remainingActivitySteps = _newSteps;
    }
}

public enum DungeonActivity
{
    eventStart,
    eventEnd,
    eventHandling,
    pathHandling,
    pathChoosing,
    startQuest,
    endQuest
}

[System.Serializable]
public struct LogEntry
{
    public int step;
    public string entry;
}

[System.Serializable]
public struct RandomNum
{
    public int step;
    public int num;
}
