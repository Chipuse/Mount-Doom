using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MapClick : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI QuestName;
    [SerializeField] TextMeshProUGUI QuestDescription;
    [SerializeField] GameObject DisablePanel;


    DungeonType QuestType = DungeonType.basic;


    int DailyDungeonIndex = 0;

    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => { MapClicked(); });
    }

    private void OnEnable()
    {
        //fill texts
        GameObject me = this.gameObject;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i).gameObject == me)
            {
                DailyDungeonIndex = i;
            }
        }
        DungeonEvent quest;
        switch (DatabaseManager._instance.dungeonData.dailyDungeons[DailyDungeonIndex].type)
        {
            case DungeonType.basic:
                quest = DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.dailyDungeons[DailyDungeonIndex].questName];
                QuestType = DungeonType.basic;
                DisablePanel.SetActive(false);

                break;
            case DungeonType.doom:
                quest = DatabaseManager._instance.eventData.doomQuestDict[DatabaseManager._instance.dungeonData.dailyDungeons[DailyDungeonIndex].questName];
                QuestType = DungeonType.doom;
                if (DatabaseManager.DoomDungeonAvailable())
                {
                    DisablePanel.SetActive(false);
                    gameObject.GetComponent<Button>().interactable = true;
                }
                else
                {
                    //Disable
                    DisablePanel.SetActive(true);
                    gameObject.GetComponent<Button>().interactable = false;

                }
                break;
            default:
                quest = DatabaseManager._instance.eventData.basicQuestDeck[0];
                Debug.LogWarning("Quest Doesnt Exist");
                break;
        }
        QuestName.text = quest.eventName;
        QuestDescription.text = quest.description;

    }

    private void OnDisable()
    {
        
    }

    private void MapClicked()
    {
        AudioManager.PlayEffect("click");

        //go to the map instance
        //
        //
        //enable dungeon detail select footer
        if (QuestType == DungeonType.doom)
        {
            if (DatabaseManager.DoomDungeonAvailable())
            {                
                //Enable
                DungeonManager._instance.chosenDailyDungeonIndex = DailyDungeonIndex + 1;
                DungeonManager._instance.ShowDungeonLayout(DailyDungeonIndex);
                UIEnablerManager.Instance.SwitchElements("DungeonMapSelect", "DungeonDetailSelect", true);

                //disable background
                UIEnablerManager.Instance.DisableCanvas();
            }
            else
            {
                //Disable
                Debug.LogWarning("Not Enough Shards");
            }
        }
        else
        {
            DungeonManager._instance.chosenDailyDungeonIndex = DailyDungeonIndex + 1;
            DungeonManager._instance.ShowDungeonLayout(DailyDungeonIndex);
            UIEnablerManager.Instance.SwitchElements("DungeonMapSelect", "DungeonDetailSelect", true);

            //disable background
            UIEnablerManager.Instance.DisableCanvas();
        }
    }
}
