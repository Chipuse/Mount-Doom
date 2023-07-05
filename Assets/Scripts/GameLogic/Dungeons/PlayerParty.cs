using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public static bool visualsActive = false;
    public static PlayerParty _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public GameObject visuals;
    //public TMP_Text activityText;
    //public TMP_Text steps;
    //public TMP_Text party;
    //public TMP_Text growths;
    //public TMP_Text reward;
    //public CheapProgressBar pgBar;

    // Start is called before the first frame update
    void Start()
    {
        if (visuals == null)
            visuals = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    void SetPosition()
    {
        if(!DatabaseManager.CheckDatabaseValid() || DatabaseManager._instance.dungeonData.currentRun == null || !DatabaseManager._instance.dungeonData.currentRun.valid || DungeonManager._instance.currentCalcRun == null || DungeonManager._instance.currentCalcRun.currentNode == null)
        {
            visuals.SetActive(false);
            visualsActive = false;
            return;
        }
        if (visuals != null && visuals.activeSelf != true)
        {
            visuals.SetActive(true);
            visualsActive = true;
        }

        //pgBar.SetVal(DungeonManager._instance.currentCalcRun.remainingActivitySteps, DungeonManager._instance.currentCalcRun.startActivitySteps);

        //steps.text = (DungeonManager._instance.currentCalcRun.startActivitySteps - DungeonManager._instance.currentCalcRun.remainingActivitySteps).ToString() + "/" + DungeonManager._instance.currentCalcRun.startActivitySteps.ToString();
        string partyText = "";
        foreach (var hero in DatabaseManager._instance.dungeonData.currentRun.party)
        {
            partyText += hero.heroId + " | ";
        }
        //party.text = partyText;

        //growths.text = DungeonManager._instance.currentCalcRun.pStatGrowth.ToString() + "|" + DungeonManager._instance.currentCalcRun.mStatGrowth.ToString() + "|" + DungeonManager._instance.currentCalcRun.sStatGrowth.ToString();
        //reward.text = DungeonManager._instance.currentCalcRun.rewardHealthBar.ToString();


        switch (DungeonManager._instance.currentCalcRun.currentActivity)
        {
            case DungeonActivity.eventHandling:
                //activityText.text = "Dealing with: " + DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.eventName;
                //steps.text = (DungeonManager._instance.currentCalcRun.currentNode.eventHealth).ToString() + "/" + DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth.ToString();

                transform.position = DungeonManager._instance.currentCalcRun.currentNode.transform.position;
                break;
            case DungeonActivity.pathHandling:
                int temp = DungeonManager._instance.currentCalcRun.currentNode.chosenPathIndex;
                //activityText.text = "Walking path: " + DungeonManager._instance.currentCalcRun.currentNode.nextPaths[temp];
                Vector3 offsetStep = new Vector3();
                offsetStep = (DungeonManager._instance.currentCalcRun.currentNode.nextNodes[temp].transform.position - DungeonManager._instance.currentCalcRun.currentNode.transform.position) / 20;
                transform.position = DungeonManager._instance.currentCalcRun.currentNode.nextNodes[temp].transform.position - offsetStep * DungeonManager._instance.currentCalcRun.remainingActivitySteps;
                break;
            case DungeonActivity.pathChoosing:
                //activityText.text = "Choosing a Path";
                transform.position = DungeonManager._instance.currentCalcRun.currentNode.transform.position;
                break;
            case DungeonActivity.startQuest:
                //if(DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
                    //activityText.text = DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].startText;
                //else
                    //activityText.text = DatabaseManager._instance.eventData.doomQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].startText;
                transform.position = DungeonManager._instance.currentCalcRun.currentNode.transform.position;
                break;
            case DungeonActivity.endQuest:
                //if (DatabaseManager._instance.dungeonData.currentRun.dungeon.type == DungeonType.basic)
                    //activityText.text = DatabaseManager._instance.eventData.basicQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].endText;
                //else
                    //activityText.text = DatabaseManager._instance.eventData.doomQuestDict[DatabaseManager._instance.dungeonData.currentRun.dungeon.questName].endText;

                transform.position = DungeonManager._instance.currentCalcRun.currentNode.transform.position;
                break;
            default:
                break;
        }
    }
}
