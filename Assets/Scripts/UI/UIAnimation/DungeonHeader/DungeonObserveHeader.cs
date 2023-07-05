using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DungeonObserveHeader : MonoBehaviour
{
    #region vars
    [SerializeField] TextMeshProUGUI rewardTierText;
    [Space]
    [SerializeField] TextMeshProUGUI DialogText;
    [Space]
    [SerializeField] TextMeshProUGUI EventTitle;
    [SerializeField] TextMeshProUGUI EventType;
    [SerializeField] TextMeshProUGUI EventValue;

    [SerializeField] Image rewardBar;
    [SerializeField] Image eventBar;

    [SerializeField] GameObject EventInfoGroup;
    [SerializeField] GameObject DialogGroup;

    [SerializeField] float animSpeed;
    [SerializeField] Color physColor;
    [SerializeField] Color magColor;
    [SerializeField] Color socColor;






    //anim calc helper reward tier
    private int rewardTier;
    private int oldRewardTier;
    private int oldRewardHealth;

    //helper text anim
    private string oldText;
    private int formerHealth;

    #endregion

    //init
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        DeleventSystem.DungeonStep += UpdateAfterStep;

        DeleventSystem.DungeonStart += InitVisuals;
        DeleventSystem.DungeonEvent += UpdateEvent;
        
        DeleventSystem.DungeonEventStart += EventStart;
        DeleventSystem.DungeonEventEnd += EventEnd;
        DeleventSystem.RewardHealthChanged += UpdateRewardHealth;
    }


    //init when activated
    private void OnEnable()
    {
        CatchUpVisuals();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void CatchUpVisuals()
    {
            //stop animations
            LeanTween.cancelAll();

            rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 + 1;

            //bar
            if (rewardTier != 1)
            {
                if (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10 != 0)
                    rewardBar.fillAmount = (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10) / 10f;

                else
                    rewardBar.fillAmount = 1;
            }

            else
                rewardBar.fillAmount = 0;


        //text
        rewardTierText.text = $"Lvl {rewardTier}";


            DialogText.text = DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry;

            if(DungeonManager._instance.currentCalcRun.currentActivity == DungeonActivity.eventHandling || DungeonManager._instance.currentCalcRun.currentActivity == DungeonActivity.eventStart)
            {
                EventInfoGroup.transform.localScale = new Vector3(1, 1, 1);//enable

                EventTitle.text = DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.eventName;
                EventType.text = DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType;
                EventValue.text = $"{DungeonManager._instance.currentCalcRun.currentNode.eventHealth} / {DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth}";

                if (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType == "magical")
                    eventBar.color = magColor;

                if (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType == "physical")
                    eventBar.color = physColor;

                if (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType == "social")
                    eventBar.color = socColor;

            eventBar.fillAmount = (float)DungeonManager._instance.currentCalcRun.currentNode.eventHealth / (float) DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth;
            }

            else
                EventInfoGroup.transform.localScale = new Vector3(1, 0, 1);

    }



    //connected funcs to delevents
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void InitVisuals()
    {
        if(gameObject.activeSelf)
        {
            //reward health
            rewardBar.fillAmount = 1;
            rewardTier = DatabaseManager._instance.dungeonData.currentRun.initialRewardTier;

            rewardTierText.text = $" {rewardTier} Lvl";

            //event info and health
            EventInfoGroup.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    private void UpdateRewardHealth()
    {
        if (DatabaseManager.CheckDatabaseValid() && DungeonManager._instance.currentCalcRun != null)//catch
        {
            //set tier
            oldRewardTier = rewardTier;

            if (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10 != 0)
                rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 + 1;//--> lowest lvl 1

            else if(DungeonManager._instance.currentCalcRun.rewardHealthBar % 10 == 0 && DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 == 0)
                rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 + 1;//--> lowest lvl 1

            else
                rewardTier = DungeonManager._instance.currentCalcRun.rewardHealthBar / 10 + 1;


            //animate if active and not lowest tier
            if (rewardTier != 1 && this.gameObject.activeSelf)
            {

                //do if same tier and different health
                if (DungeonManager._instance.currentCalcRun.rewardHealthBar != oldRewardHealth)
                {
                    oldRewardHealth = DungeonManager._instance.currentCalcRun.rewardHealthBar;

                    float start = rewardBar.fillAmount;
                    float end = (DungeonManager._instance.currentCalcRun.rewardHealthBar % 10) / 10f;

                    StartCoroutine(AnimateOne(animSpeed, start, end));
                }

            }

            if(rewardTier == 1)
            {
                rewardBar.fillAmount = 0;

                rewardTierText.text = $"Lvl {rewardTier}";
            }
        }
    }

    private void UpdateEvent()
    {
        if (formerHealth != DungeonManager._instance.currentCalcRun.currentNode.eventHealth && DungeonManager._instance.currentCalcRun.currentNode.eventHealth != DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth && gameObject.activeSelf)
        {
            //update event health
            int maxhealth = DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth;
            int health = DungeonManager._instance.currentCalcRun.currentNode.eventHealth;


            //animate text
            LeanTween.value(EventValue.gameObject, formerHealth, health, animSpeed).
                setOnUpdate(setEventHealthText);

            formerHealth = health;
            


            float newHealth = (float)health / (float)maxhealth;
            float oldHealth = eventBar.fillAmount;

            //set and animate health bar
            LeanTween.value(eventBar.gameObject, oldHealth, newHealth, animSpeed)
                .setOnUpdate(setEventFillAmount)
                .setEaseInExpo();


        }


        else if (formerHealth != DungeonManager._instance.currentCalcRun.currentNode.eventHealth && DungeonManager._instance.currentCalcRun.currentNode.eventHealth == DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth)
        {

            int maxhealth = DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth;

            EventValue.text = $"{maxhealth} / {maxhealth}";

            eventBar.fillAmount = 1;
        }
    }

    private void EventStart()
    {
        if (gameObject.activeSelf)
        {
            //show info
            

            EventInfoGroup.transform.localScale = new Vector3 (1,0,1);

            LeanTween.value(EventInfoGroup, 0, 1, animSpeed).
                setOnUpdate(setEventGroup)
                .setEaseOutElastic();

            // set title text
            EventTitle.text = DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.eventName;

            //set health init and health type
            EventType.text = DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType;

            if (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType == "magical")
                eventBar.color = magColor;

            if (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType == "physical")
                eventBar.color = physColor;

            if (DungeonManager._instance.currentCalcRun.currentNode.nodeEvent.statType == "social")
                eventBar.color = socColor;

            int health = DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth;

            EventValue.text = $"{health} / {health}";

            //reset healtbar
            eventBar.fillAmount = 1;


            //reset dialog text
            DialogText.text = "";
            StartCoroutine(AnimateTextBox(DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry,animSpeed));
        }
    }

    private void EventEnd()
    {
        if(gameObject.activeSelf)
        {
            //hide info
            EventInfoGroup.transform.localScale = new Vector3(1, 1, 1);

            LeanTween.value(EventInfoGroup, 1, 0, animSpeed).
                setOnUpdate(setEventGroup)
                .setEaseOutElastic();

        }
    }

    private void UpdateAfterStep()
    {
        if (oldText != DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry && gameObject.activeSelf) 
        {
            // text box animation
            oldText = DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry;

            StartCoroutine(AnimateTextBox(DungeonManager._instance.currentCalcRun.dungeonLogArr[DungeonManager._instance.currentCalcRun.dungeonLogArr.Length - 1].entry, animSpeed));
        }

    }

    //other animation helper
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    IEnumerator AnimateTextBox(string newEntry, float time)
    {
        LeanTween.value(EventInfoGroup,1, 0, time/2)
            .setOnUpdate(setTextGroup)
            .setEaseInOutExpo();

        yield return new WaitForSeconds(time/2);

        DialogText.text = newEntry;

        LeanTween.value(EventInfoGroup, 0, 1, time / 2)
            .setOnUpdate(setTextGroup)
            .setEaseInOutExpo();
    }


    //eventbar
    private void setEventFillAmount(float value)
    {
        eventBar.fillAmount = value;
    }

    private void setEventHealthText(float value)
    {
        if (value < 0)
            value = 0;

        EventValue.text = $"{(int)value} / {DungeonManager._instance.currentCalcRun.currentNode.maxEventHealth}";
    }

    private void setEventGroup(float val)
    {
        EventInfoGroup.transform.localScale = new Vector3 (val,val,val);
    }

    private void setTextGroup(float val)
    {
        DialogGroup.GetComponent<CanvasGroup>().alpha = val;
    }

    //reward bar animation
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
  

    IEnumerator AnimateOne(float time, float startValue, float endValue)
    {
        if(oldRewardTier < rewardTier)
        {
            startValue = 0;
            rewardBar.fillAmount = 0;
        }


        if (oldRewardTier > rewardTier)
        {
            startValue = 1;
            rewardBar.fillAmount = 1;
        }

        else
            startValue = rewardBar.fillAmount;



        LeanTween.value(rewardBar.gameObject, startValue, endValue, time)
            .setOnUpdate(setRewardFillAmount);

        yield return new WaitForSeconds(time);

        if (endValue == 0)
        {
            rewardBar.fillAmount = 1;

            rewardTierText.text = $"Lvl {rewardTier}";
        }

        if (oldRewardTier > rewardTier )
            rewardTierText.text = $"Lvl {rewardTier}";
    }

    private void setRewardFillAmount(float value)
    {
        if(value >= 0)
            rewardBar.fillAmount = value;    
    }



}
