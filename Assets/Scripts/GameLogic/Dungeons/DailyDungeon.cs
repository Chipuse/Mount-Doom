using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class DailyDungeon
{
    public int dailySeed;
    public string layoutId;
    public string date;
    public string questName;
    public DungeonType type;
    public int difficultyIndex;

    //does not get serialized. Gets set on dungeon layout setup
    [System.NonSerialized]  public DungeonLayOut dungeonLayout;

    public void InitDungeonLayout()
    {
        if(dungeonLayout != null)
        {
            MonoBehaviour.Destroy(dungeonLayout.gameObject);
        }
        if (!DungeonManager._instance.layoutList.layoutPrefabs.ContainsKey(layoutId))
        {
            layoutId = DungeonManager._instance.layoutList.layouts[0].name;
        }
        GameObject go = GameObject.Instantiate(DungeonManager._instance.layoutList.layoutPrefabs[layoutId], new Vector3(0, 0, 0), Quaternion.identity);
        dungeonLayout =  go.GetComponent<DungeonLayOut>();
        if(DatabaseManager._instance.rewardTable.dungeonDifficulties.Count > difficultyIndex)
            dungeonLayout.SetupDungeonDailySeed(dailySeed, DatabaseManager._instance.rewardTable.dungeonDifficulties[difficultyIndex]);
        else
            dungeonLayout.SetupDungeonDailySeed(dailySeed);
    }
}

[System.Serializable]
public class DungeonData
{
    public DailyDungeon[] dailyDungeons;
    public DungeonRun currentRun;
}
