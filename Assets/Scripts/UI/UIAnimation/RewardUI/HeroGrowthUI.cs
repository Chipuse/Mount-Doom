using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroGrowthUI : MonoBehaviour
{
    #region vars
    [SerializeField] Transform HeroAmount;
    [SerializeField] GameObject ContinueButton;

    [SerializeField] float animSpeed;

    List <GrowthCard> growthCards = new List<GrowthCard> ();

    //calc helper
    int child_1;
    int child_2;
    #endregion



    private void OnEnable()
    {
        //test
        UpdateHeroGrowthPopUp();
    }

    //update card
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void UpdateHeroGrowthPopUp()
    {
        AudioManager.PlayEffect("finished");

        ContinueButton.SetActive(false);


        //reset
        growthCards.Clear();



        //catch
        if (DatabaseManager._instance.dungeonData.currentRun.party.Count < 0)
            Debug.LogWarning("Party is empty");

        //hide all
        for(int i = 0; i < HeroAmount.childCount; i++)
        {
            HeroAmount.GetChild(i).gameObject.SetActive(false);
        }


        //set active due to party size
        for (int i = 0; i < DatabaseManager._instance.dungeonData.currentRun.party.Count; i++)
        {
            if(i >= 4)
            {
                Debug.LogWarning("Party too big");
                break;
            }

            child_1 = i * 2;

            HeroAmount.GetChild(child_1).gameObject.SetActive(true);
            HeroAmount.GetChild(child_1).GetComponent<GrowthCard>().UpdateHero(DatabaseManager._instance.dungeonData.currentRun.party[i]);

            growthCards.Add(HeroAmount.GetChild(child_1).GetComponent<GrowthCard>());

            if(child_1 - 1 >= 0)
            {
                child_2 = child_1 - 1;
                HeroAmount.GetChild(child_2).gameObject.SetActive(true);
            }
        }

        StartCoroutine(Anim());
    }


    //buttons
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void DoAnim()
    {
        LeanTween.value(gameObject, 0f, 1f, animSpeed)
            .setEaseInOutExpo()
            .setOnUpdate(setGrowth);

        LeanTween.value(gameObject, 0f, 1f, animSpeed)
            .setEaseInOutExpo()
            .setOnUpdate(setBarGrowth);
    }    




    //do growth animations
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void setGrowth(float i)
    {
        //number text
        foreach (GrowthCard card in growthCards)
        {
            card.UpdatePhysicalGrowth((int)(card.playerHero.CalcGrowth(DungeonManager._instance.currentCalcRun.pStatGrowth, StatType.physical, DatabaseManager._instance.dungeonData.currentRun.dungeon.type) * i));
            card.UpdateMagicalGrowth((int)(card.playerHero.CalcGrowth(DungeonManager._instance.currentCalcRun.mStatGrowth, StatType.magical, DatabaseManager._instance.dungeonData.currentRun.dungeon.type)  * i ));
            card.UpdateSocialGrowth((int)(card.playerHero.CalcGrowth(DungeonManager._instance.currentCalcRun.sStatGrowth, StatType.social, DatabaseManager._instance.dungeonData.currentRun.dungeon.type) * i));
        }
    }

    private void setBarGrowth(float i)
    {
        foreach (GrowthCard card in growthCards)
        {
            card.UpdateMagicalBarGrowth((int)(card.playerHero.CalcGrowth(DungeonManager._instance.currentCalcRun.pStatGrowth, StatType.physical, DatabaseManager._instance.dungeonData.currentRun.dungeon.type) * i));
            card.UpdatePhysicalBarGrowth((int)(card.playerHero.CalcGrowth(DungeonManager._instance.currentCalcRun.mStatGrowth, StatType.magical, DatabaseManager._instance.dungeonData.currentRun.dungeon.type) * i));
            card.UpdateSocialBarGrowth((int)(card.playerHero.CalcGrowth(DungeonManager._instance.currentCalcRun.sStatGrowth, StatType.social, DatabaseManager._instance.dungeonData.currentRun.dungeon.type) * i));
        }
    }


    IEnumerator  Anim()
    {
        yield return new WaitForSeconds(0.5f);
        DoAnim();
        yield return new WaitForSeconds(animSpeed);


        ContinueButton.SetActive(true);

    }
}