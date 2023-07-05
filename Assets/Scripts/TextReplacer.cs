using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextReplacer
{
    static public string[] replacements = new string[]
    {
        "$Enemy",
        "$RandEnemy",
        "$NodeType",
        "$NextNodeType",
        "$PathType",
        "$EventName",
        "$NextEventName",
        "$HeroId",
        "$HeroJob",
        "$HeroRace",
        "$HeroRar",
        "$HeroVal",
        "$HeroPhy",
        "$HeroMag",
        "$HeroSoc",
        "$RandHeroId",
        "$RandHeroJob",
        "$RandHeroRace",
        "$RandHeroRar",
        "$RandHeroVal",
        "$RandHeroPhy",
        "$RandHeroMag",
        "$RandHeroSoc",
        "$FirstHeroId",
        "$FirstHeroJob",
        "$FirstHeroRace",
        "$FirstHeroRar",
        "$FirstHeroVal",
        "$FirstHeroPhy",
        "$FirstHeroMag",
        "$FirstHeroSoc"
    };
    public static string ReplaceWordsBulk(string _input)
    {
        string result = _input;
        foreach (var entry in replacements) {
            if(result.Contains(entry))
                result = result.Replace(entry, ReplaceWord(entry));
        }
        return result;
    }

    static string ReplaceWord(string _word)
    {
        string result = "---";
        if(!DatabaseManager.CheckDatabaseValid() || DatabaseManager._instance.dungeonData.currentRun == null || !DatabaseManager._instance.dungeonData.currentRun.valid || DungeonManager._instance.currentCalcRun == null)
        {
            return "upsi";
        }
        switch (_word)
        {
            case "$HeroId":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId;
                break;
            case "$HeroJob":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId].job;
                break;
            case "$HeroRace":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId].race;

                break;
            case "$HeroRar":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[ DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].heroId].rarity.ToString();

                break;
            case "$HeroVal":
                switch (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].pVal.ToString();
                        break;
                    case "magical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].mVal.ToString();
                        break;
                    case "social":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].sVal.ToString();
                        break;
                    default:
                        break;
                }

                break;
            case "$HeroPhy":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].pVal.ToString();
                break;
            case "$HeroMag":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].mVal.ToString();
                break;
            case "$HeroSoc":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.nextHero].sVal.ToString();
                break;
            case "$RandHeroId":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId;

                break;
            case "$RandHeroJob":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId].job;
                break;
            case "$RandHeroRace":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId].race;
                break;
            case "$RandHeroRar":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].heroId].rarity.ToString();
                break;
            case "$RandHeroVal":
                switch (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].pVal.ToString();
                        break;
                    case "magical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].mVal.ToString();
                        break;
                    case "social":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].sVal.ToString();
                        break;
                    default:
                        break;
                }
                break;
            case "$RandHeroPhy":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].pVal.ToString();
                break;
            case "$RandHeroMag":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].mVal.ToString();
                break;
            case "$RandHeroSoc":
                result = DatabaseManager._instance.dungeonData.currentRun.party[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.dungeonData.currentRun.party.Count)].sVal.ToString();
                break;
            //FirstHero
            case "$FirstHeroId":
                result = DatabaseManager._instance.dungeonData.currentRun.party[0].heroId;

                break;
            case "$FirstHeroJob":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[0].heroId].job;
                break;
            case "$FirstHeroRace":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[0].heroId].race;
                break;
            case "$FirstHeroRar":
                result = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[DatabaseManager._instance.dungeonData.currentRun.party[0].heroId].rarity.ToString();
                break;
            case "$FirstHeroVal":
                switch (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType)
                {
                    case "physical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[0].pVal.ToString();
                        break;
                    case "magical":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[0].mVal.ToString();
                        break;
                    case "social":
                        result = DatabaseManager._instance.dungeonData.currentRun.party[0].sVal.ToString();
                        break;
                    default:
                        break;
                }
                break;
            case "$FirstHeroPhy":
                result = DatabaseManager._instance.dungeonData.currentRun.party[0].pVal.ToString();
                break;
            case "$FirstHeroMag":
                result = DatabaseManager._instance.dungeonData.currentRun.party[0].mVal.ToString();
                break;
            case "$FirstHeroSoc":
                result = DatabaseManager._instance.dungeonData.currentRun.party[0].sVal.ToString();
                break;
            case "$Enemy":
                result = DungeonManager._instance.currentCalcRun.currentNode.eventEnemy;
                break;
            case "$RandEnemy":
                result = DatabaseManager._instance.eventData.textFlavours.textsEnemyNames[DungeonManager._instance.currentCalcRun.RandomNum(0, DatabaseManager._instance.eventData.textFlavours.textsEnemyNames.Length)].name;
                break;
            case "$NodeType":
                result = DungeonManager._instance.currentCalcRun.currentNode.nodeType;
                break;
            case "$NextNodeType":
                if (DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex != -1)
                {
                    result = DungeonManager._instance.currentCalcRun.currentNode.nextNodes[DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex].nodeType;
                }
                break;
            case "$PathType":
                if(DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex != -1)
                {
                    result = DungeonManager._instance.currentCalcRun.currentNode.nextPaths[DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex];
                }
                break;
            case "$EventName":
                result = DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.eventName;
                break;
            case "$NextEventName":
                if (DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex != -1)
                {
                    result = DungeonManager._instance.currentCalcRun.currentNode.nextNodes[DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex].nodeEvent.eventName;
                }
                break;
            default:
                break;
        }

        return result;
    }
}

[System.Serializable]
public class Replacement
{
    public string from;
    public string to;
}
