using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager _instance;
    public static int PityGrowth = 2;
    public static int MaxGrowth = 8;
    public static bool events = true;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            layoutList = Resources.Load<LayoutList>("LayoutList");
            layoutList.CreateDictionary();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public int chosenDailyDungeonIndex = 0;

    public LayoutList layoutList;
    public CalculatedDungeonRun currentCalcRun;
    public PlayerHero rewardHero;
    public bool CheckCalcRun()
    {
        if (currentCalcRun != null && currentCalcRun.currentNode != null)
            return true;
        return false;
    }

    private bool AutoPLay = true;
    public float AutoPlayWaitTimeSec = 1.0f;

    void OnEnable()
    {        
        StartCoroutine(AutoplayRoutine());
        currentCalcRun = null;
    }
    private void OnDisable()
    {        
        StopCoroutine(AutoplayRoutine());
    }

    public void ShowDungeonLayout(int _dailyDungeonIndex)
    {
        HideDungeonLayouts();
        if(_dailyDungeonIndex < DatabaseManager._instance.dungeonData.dailyDungeons.Length)
        {
            DatabaseManager._instance.dungeonData.dailyDungeons[_dailyDungeonIndex].dungeonLayout.gameObject.SetActive(true);
            DatabaseManager._instance.dungeonData.dailyDungeons[_dailyDungeonIndex].dungeonLayout.FocusCamera();
        }            
    }

    public void HideDungeonLayouts()
    {
        foreach (var dd in DatabaseManager._instance.dungeonData.dailyDungeons)
        {
            dd.dungeonLayout.gameObject.SetActive(false);
        }
    }

    public int CurrentStep()
    {
        double elapsedSeconds = 0;
        if (DatabaseManager._instance.dungeonData.currentRun != null && DatabaseManager._instance.dungeonData.currentRun.date != null)
        {
            var bla = DateTime.Parse(DatabaseManager._instance.dungeonData.currentRun.date).ToUniversalTime();
            var blub = DateTime.Now.ToUniversalTime();
            var bli = DateTime.UtcNow;
            elapsedSeconds = DateTime.Now.ToUniversalTime().Subtract(DateTime.Parse(DatabaseManager._instance.dungeonData.currentRun.date).ToUniversalTime()).TotalSeconds;
            if(AutoPlayWaitTimeSec != 0)
            {
                elapsedSeconds /= AutoPlayWaitTimeSec;
            }
        }
        return (int)elapsedSeconds;
    }

    public void CreateDailyDungeons()
    {
        //create a new set of dailyDungeons
        if(DatabaseManager._instance.dungeonData == null)
        {
            DatabaseManager._instance.dungeonData = new DungeonData();
            DatabaseManager._instance.dungeonData.currentRun = new DungeonRun { valid = false };
        }
        if(DatabaseManager._instance.dungeonData.currentRun != null && DatabaseManager._instance.dungeonData.currentRun.valid)
        {
            DatabaseManager._instance.dungeonData.currentRun.dungeon.InitDungeonLayout();
            DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(false);
        }
        if (DatabaseManager._instance.dungeonData.dailyDungeons != null && DatabaseManager._instance.dungeonData.dailyDungeons.Length > 0)
        {
            //check if current daily dungeon data is older than 24 hours
            if (DateTime.Parse(DatabaseManager._instance.dungeonData.dailyDungeons[0].date).ToUniversalTime().Date == DateTime.Now.ToUniversalTime().Date)
            {
                Debug.Log("Current daily dungeons are still valid");
                InitDungeonLayouts();
                return;
            }
        }
        List<DailyDungeon> tempList = new List<DailyDungeon>();
        int numBasicDungeons = 3;
        for (int i = 0; i < numBasicDungeons; i++)
        {
            DailyDungeon tempDungeon = new DailyDungeon
            {
                dailySeed = UnityEngine.Random.Range(1, 3000),
                layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
                date = DateTime.Now.ToUniversalTime().ToString("u"),
                questName = DatabaseManager._instance.eventData.basicQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.basicQuestDeck.Length)].eventName,
                type = DungeonType.basic,
                difficultyIndex = i
            };
            tempList.Add(tempDungeon);
        }
        DailyDungeon tempDoomDungeon = new DailyDungeon
        {
            dailySeed = UnityEngine.Random.Range(1, 3000),
            layoutId = layoutList.layouts[UnityEngine.Random.Range(0, layoutList.layouts.Length)].name,
            date = DateTime.Now.ToUniversalTime().ToString("u"),
            questName = DatabaseManager._instance.eventData.doomQuestDeck[UnityEngine.Random.Range(0, DatabaseManager._instance.eventData.doomQuestDeck.Length)].eventName,
            type = DungeonType.doom,
            difficultyIndex = DatabaseManager._instance.rewardTable.dungeonDifficulties.Count - 1
        };
        tempList.Add(tempDoomDungeon);
        DatabaseManager._instance.dungeonData.dailyDungeons = tempList.ToArray();
        DatabaseManager._instance.SaveGameDataLocally();
        InitDungeonLayouts();
    }

    public void InitDungeonLayouts()
    {
        foreach (var dailyDungeon in DatabaseManager._instance.dungeonData.dailyDungeons)
        {
            dailyDungeon.InitDungeonLayout();
            dailyDungeon.dungeonLayout.gameObject.SetActive(false);
        }
    }

    public DungeonRun CreateDungeonRun(int _dailyDungeon)
    {        
        DungeonRun result = null;
        List<PlayerHero> chosenParty = new List<PlayerHero>();
        foreach (PlayerHero hero in DatabaseManager._instance.activePlayerData.inventory)
        {
            if (hero.status == HeroStatus.Exploring)
                chosenParty.Add(hero);
        }
        if (_dailyDungeon <= DatabaseManager._instance.dungeonData.dailyDungeons.Length && _dailyDungeon > 0 && chosenParty.Count > 0)
        {
            DailyDungeon chosenDungeon = DatabaseManager._instance.dungeonData.dailyDungeons[_dailyDungeon - 1];
            result = new DungeonRun
            {
                dungeon = chosenDungeon,
                date = DateTime.Now.ToUniversalTime().ToString("u"),
                valid = false,
                dungeonSeed = UnityEngine.Random.Range(0, 500),
                initialRewardTier = DatabaseManager._instance.activePlayerData.rewardTierBuff,
                randomNums = new List<RandomNum>()
            };
            DatabaseManager._instance.activePlayerData.rewardTierBuff = 0;
            if (chosenDungeon.type == DungeonType.basic)
            {
                result.initialRewardTier = 3 + DatabaseManager._instance.activePlayerData.rewardTierBuff;
                if (result.initialRewardTier > 5)
                    result.initialRewardTier = 5;
            }
            else
            {
                result.initialRewardTier = 5 + DatabaseManager._instance.activePlayerData.rewardTierBuff;
                if (result.initialRewardTier > 9)
                    result.initialRewardTier = 9;
            }
            result.party = chosenParty;
            result.valid = true;
            DatabaseManager._instance.dungeonData.currentRun = result;
            DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
            CalculateMaxStep(result);
            result.valid = true;
        }
        return result;
    }

    public void NextStepRun()
    {
        if (currentCalcRun == null || !DatabaseManager._instance.dungeonData.currentRun.valid) //currentCalcRun.currentStep > DatabaseManager._instance.dungeonData.currentRun.maxSteps)
            return;
        StepCalcRun();
    }

    //refresh Run and calc until step
    public void CalculateRun(int _targetStep)
    {
        events = false;
        currentCalcRun = null;
        StartCalcRun();
        if (_targetStep > DatabaseManager._instance.dungeonData.currentRun.maxSteps)
            _targetStep = DatabaseManager._instance.dungeonData.currentRun.maxSteps;
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.ResetNodes();
        for (int i = 0; i <= _targetStep; i++)
        {
            NextStepRun();
        }
        events = true;
    }

    public void RevalidateMaxStepsAndRandomNums()
    {
        DatabaseManager._instance.dungeonData.currentRun.randomNums = new List<RandomNum>();
        CalculateMaxStep(DatabaseManager._instance.dungeonData.currentRun);
    }

    //calculate until max step for setup purposes
    void CalculateMaxStep(DungeonRun _dungeonRun)
    {
        events = false;
        currentCalcRun = null;

        currentCalcRun = new CalculatedDungeonRun(DatabaseManager._instance.dungeonData.currentRun);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.ResetNodes();
        UnityEngine.Random.InitState(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
        EnterNewActivityState(DungeonActivity.startQuest, false);

        while (currentCalcRun.currentNode != DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.endNode || currentCalcRun.remainingActivitySteps > 0)
        {
            StepCalcRun();
        }

        _dungeonRun.maxSteps = currentCalcRun.currentStep;
        PushManager.ScheduleNotification("Quest Finished!", "The party completed: " + DatabaseManager._instance.dungeonData.currentRun.dungeon.questName, DateTime.Parse(DatabaseManager._instance.dungeonData.currentRun.date).ToLocalTime().AddSeconds(currentCalcRun.currentStep), DatabaseManager._instance.dungeonData.currentRun.party[UnityEngine.Random.Range(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);

        UnityEngine.Random.InitState((int)DateTime.Now.ToUniversalTime().Ticks);
        currentCalcRun = null;
        events = true;
    }

    void StartCalcRun()
    {
        if (DatabaseManager._instance.dungeonData.currentRun == null || DatabaseManager._instance.dungeonData.currentRun.valid == false)
            return;
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.SetupDungeonRunSeed(DatabaseManager._instance.dungeonData.currentRun.dungeonSeed);
        currentCalcRun = new CalculatedDungeonRun(DatabaseManager._instance.dungeonData.currentRun);
        EnterNewActivityState(DungeonActivity.startQuest, false);
    }

    void StepCalcRun()
    {
        currentCalcRun.currentStep++;
        if(events)
            DeleventSystem.DungeonStep?.Invoke();
        switch (currentCalcRun.currentActivity)
        {
            case DungeonActivity.eventStart:
                CalcEventStart();
                break;
            case DungeonActivity.eventEnd:
                CalcEventEnd();
                break;
            case DungeonActivity.eventHandling:
                CalcRunEventHandling();
                break;
            case DungeonActivity.pathHandling:
                CalcRunPathHandling();
                break;
            case DungeonActivity.pathChoosing:
                CalcRunPathChoosing();
                break;
            case DungeonActivity.startQuest:
                CalcRunStartQuest();
                break;
            case DungeonActivity.endQuest:
                CalcRunEndQuest();
                break;
            default:
                break;
        }
    }

    void EnterNewActivityState(DungeonActivity _newActivity, bool _doExit = true)
    {
        if (_doExit)
        {
            //Exit behavior if necessary
            switch (currentCalcRun.currentActivity)
            {
                case DungeonActivity.eventStart:
                    break;
                case DungeonActivity.eventEnd:
                    break;
                case DungeonActivity.eventHandling:
                    break;
                case DungeonActivity.pathHandling:
                    break;
                case DungeonActivity.pathChoosing:
                    break;
                case DungeonActivity.startQuest:
                    break;
                case DungeonActivity.endQuest:
                    break;
                default:
                    break;
            }
        }

        //currently flat 20 steps per interaction
        currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.fallBack);
        currentCalcRun.currentActivity = _newActivity;
        //enter behvior/steup if necessary
        switch (_newActivity)
        {
            case DungeonActivity.eventStart:
                CameraMover.SetTargetPos(currentCalcRun.currentNode.transform.position);
                currentCalcRun.UpdateLog(currentCalcRun.currentNode.nodeEvent.startText);
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.eventStart);
                if (events)
                    DeleventSystem.DungeonEventStart?.Invoke();
                EnterNewActivityState(DungeonActivity.eventHandling);
                break;
            case DungeonActivity.eventEnd:
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.eventEnd);
                switch (currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        currentCalcRun.pStatGrowth += currentCalcRun.currentNode.currentGrowth;
                        break;
                    case "magical":
                        currentCalcRun.mStatGrowth += currentCalcRun.currentNode.currentGrowth; 
                        break;
                    case "social":
                        currentCalcRun.sStatGrowth += currentCalcRun.currentNode.currentGrowth; 
                        break;
                    default:
                        break;
                }
                currentCalcRun.UpdateLog(currentCalcRun.currentNode.nodeEvent.endText);
                if (events)
                    DeleventSystem.DungeonEventEnd?.Invoke();
                break;
            case DungeonActivity.eventHandling:
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.eventTurn);
                break;
            case DungeonActivity.pathHandling:
                CameraMover.SetTargetPos(currentCalcRun.currentNode.PathPosition(currentCalcRun.currentNode.chosenPathIndex));
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.pathHandling);
                currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.textFlavours.GetRandomPathHandlingText(currentCalcRun.currentNode.nextPaths[currentCalcRun.currentNode.chosenPathIndex]));
                break;
            case DungeonActivity.pathChoosing:
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.pathChoosing);
                currentCalcRun.currentNode.chosenPathIndex = DoPathChoosing();
                currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.textFlavours.GetRandomPathChoosingText(currentCalcRun.currentNode.nextPaths[currentCalcRun.currentNode.chosenPathIndex]));
                break;
            case DungeonActivity.startQuest:
                CameraMover.SetTargetPos(DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.startNode.transform.position);
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.questStart);
                if (DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
                    currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].startText);
                else
                    currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.doomQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].startText);
                break;
            case DungeonActivity.endQuest:
                CameraMover.SetTargetPos(DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.endNode.transform.position);
                currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.questEnd);
                if (DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
                    currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].shortEndText);
                else
                    currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.doomQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].shortEndText);
                break;
            default:
                break;
        }


    }

    int DoPathChoosing()
    {
        int result = 0;
        Dictionary<string, int> pathPriorities = new Dictionary<string, int>();
        foreach (var item in currentCalcRun.currentNode.nextPaths)
        {
            if(!pathPriorities.ContainsKey(item))
                pathPriorities.Add(item, 0);
        }
        foreach (var item in DatabaseManager._instance.dungeonData.currentRun.party)
        {
            if (pathPriorities.ContainsKey(DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[item.heroId].pathAff))
            {
                pathPriorities[DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[item.heroId].pathAff] += 1;
            }
        }
        int highest = 0;
        List<int> highPrioIndex = new List<int>();
        for(int i = 0; i < currentCalcRun.currentNode.nextPaths.Count; i++)
        {
            if(pathPriorities[currentCalcRun.currentNode.nextPaths[i]] == highest)
            {
                highPrioIndex.Add(i);
            }
            else if (pathPriorities[currentCalcRun.currentNode.nextPaths[i]] > highest)
            {
                highPrioIndex = new List<int>();
                highest = pathPriorities[currentCalcRun.currentNode.nextPaths[i]];
                highPrioIndex.Add(i);
            }
        }
        if(highPrioIndex.Count > 0)
            result = highPrioIndex[currentCalcRun.RandomNum(0, highPrioIndex.Count)];
        return result;
    }

    void CalcEventStart()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            EnterNewActivityState(DungeonActivity.eventHandling);
        }
    }

    void CalcEventEnd()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            currentCalcRun.AffectRewardHealth(6);
            EnterNewActivityState(DungeonActivity.pathChoosing);
        }
    }

    void CalcRunEventHandling()
    {
        if(currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            if (currentCalcRun.currentNode.eventHealth > 0)
            {

                if (currentCalcRun.nextHero >= DatabaseManager._instance.dungeonData.currentRun.party.Count)
                {
                    //fineished a round
                    currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.eventStart);
                    currentCalcRun.nextHero = 0;
                    //display end round text for event
                    currentCalcRun.currentNode.currentGrowth--;
                    if (currentCalcRun.currentNode.currentGrowth < -3)
                        currentCalcRun.currentNode.currentGrowth = -3;
                    currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.textFlavours.GetRandomEnemyTurnText(DungeonManager._instance.currentCalcRun.currentNode.nodeType, DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType));
                    currentCalcRun.AffectRewardHealth(-2);
                }
                else
                {
                    //intriguing event logic
                    currentCalcRun.SetActivitySteps(DatabaseManager._instance.eventData.eventSteps.eventStart);
                    //trigger a zwischentext?
                    currentCalcRun.UpdateLog(DatabaseManager._instance.eventData.textFlavours.GetRandomHeroTurnText(DungeonManager._instance.currentCalcRun.currentNode.nodeType, DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType));
                    switch (currentCalcRun.currentNode.nodeEvent.statType)
                    {
                        case "physical":
                            currentCalcRun.currentNode.eventHealth -= DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].pVal;
                            break;
                        case "magical":
                            currentCalcRun.currentNode.eventHealth -= DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].mVal;
                            break;
                        case "social":
                            currentCalcRun.currentNode.eventHealth -= DatabaseManager._instance.dungeonData.currentRun.party[currentCalcRun.nextHero].sVal;
                            break;
                        default:
                            break;
                    }
                    currentCalcRun.nextHero++;

                    if (events)
                        DeleventSystem.DungeonEvent?.Invoke();
                }
                if (currentCalcRun.currentNode.eventHealth < 0)
                {
                    currentCalcRun.currentNode.eventHealth = 0;
                    //event finished
                }
            }
            else
            {
                EnterNewActivityState(DungeonActivity.eventEnd);
            }
            
        }
    }

    void CalcRunPathHandling()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            currentCalcRun.currentNode = currentCalcRun.currentNode.nextNodes[currentCalcRun.currentNode.chosenPathIndex];
            if(currentCalcRun.currentNode.nextNodes == null || currentCalcRun.currentNode.nextNodes.Length <= 0)
            {
                EnterNewActivityState(DungeonActivity.endQuest);
            }
            else
            {
                EnterNewActivityState(DungeonActivity.eventStart);
            }

        }
    }

    void CalcRunPathChoosing()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            EnterNewActivityState(DungeonActivity.pathHandling);
        }
    }

    void CalcRunStartQuest()
    {
        if(currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else
        {
            EnterNewActivityState(DungeonActivity.pathChoosing);
        }
    }

    void CalcRunEndQuest()
    {
        if (currentCalcRun.remainingActivitySteps > 0)
        {
            currentCalcRun.remainingActivitySteps--;
        }
        else if(!currentCalcRun.ended)
        {
            currentCalcRun.ended = true;
            DatabaseManager._instance.SaveGameDataLocally();
            Debug.Log("Run Ended");
            if (events)
            {
                AudioManager.PlayEffect("done");
                DeleventSystem.dungeonRunFinished?.Invoke();
            }
        }
    }


    //should only be called when the dungeon Run gets dissolved
    public void ApplyGrowth()
    {
        foreach (var hero in DatabaseManager._instance.dungeonData.currentRun.party)
        {
            hero.ApplyGrowth(currentCalcRun.pStatGrowth, currentCalcRun.mStatGrowth, currentCalcRun.sStatGrowth);
            hero.runs += 1;
            hero.status = HeroStatus.Idle;
        }
    }

    public void CancelHeroesDungeon()
    {
        foreach (var hero in DatabaseManager._instance.dungeonData.currentRun.party)
        {
            hero.status = HeroStatus.Idle;
        }
    }

    public void EventRewardShardHandling()
    {
        if(DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
        {
            DatabaseManager._instance.activePlayerData.shards++;
            if (DatabaseManager._instance.activePlayerData.shards >= 5)
                DatabaseManager._instance.activePlayerData.shards = 5;
        }
    }

    public void EventRewardHeroHandling()
    {
        int rewardTier = currentCalcRun.rewardHealthBar / 10;
        if(currentCalcRun.rewardHealthBar % 10 != 0)
        {
            //integer division rounds down but we want it to go up
            rewardTier += 1;
        }
        rewardHero = HeroCreator.GetHeroByRewardTier(rewardTier);
        
    }

    public void AddRewardHeroToInventory()
    {
        if(rewardHero != null)
        {
            rewardHero.invIndex = -1;
            DatabaseManager._instance.activePlayerData.inventory.Add(rewardHero);
            DatabaseManager.ValidateInventory();
        }
    }

    public void DiscardRewardHero()
    {
        rewardHero = null;
    }

    public void WrapUpDungeon()
    {
        DatabaseManager._instance.activePlayerData.mtdCounter++;
        DatabaseManager._instance.activePlayerData.dungeonCleared += 1;
        ApplyGrowth();
        currentCalcRun = null;

        DatabaseManager._instance.dungeonData.currentRun.valid = false;
        Destroy(DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.InitDungeonLayout();
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(false);


        DatabaseManager._instance.SaveGameDataLocally();
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData);
        ServerCommunicationManager._instance.DoServerRequest(Request.PushDungeonData);
    }

    public void CancelDungeon()
    {
        CancelHeroesDungeon();
        currentCalcRun = null;
        DatabaseManager._instance.dungeonData.currentRun.valid = false;
        Destroy(DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject);
        DatabaseManager._instance.dungeonData.currentRun.dungeon.InitDungeonLayout(); 
        DatabaseManager._instance.dungeonData.currentRun.dungeon.dungeonLayout.gameObject.SetActive(false);


        DatabaseManager._instance.SaveGameDataLocally();
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData);
        ServerCommunicationManager._instance.DoServerRequest(Request.PushDungeonData);
    }



    IEnumerator AutoplayRoutine()
    {
        while (AutoPLay)
        {
            //Update Loop for dungeonRun
            if(CheckCalcRun() && DatabaseManager._instance.dungeonData.currentRun.valid)
            {
                NextStepRun();
            }
            yield return new WaitForSeconds(AutoPlayWaitTimeSec);
        }
    }
}
