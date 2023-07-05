using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI endText;

    private void OnEnable()
    {
        DungeonEvent quest;
        if (DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
            quest = DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName];
        else
            quest = DatabaseManager._instance.eventData.doomQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName];

        endText.text = quest.endText;
    }
}
