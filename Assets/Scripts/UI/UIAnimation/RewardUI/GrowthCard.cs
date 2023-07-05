using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrowthCard : MonoBehaviour
{
    #region vars

    [SerializeField] TMP_Text Name;
    [Space]
    [SerializeField] TMP_Text physicalStatText;
    [SerializeField] TMP_Text magicalStatText;
    [SerializeField] TMP_Text socialStatText;
    [Space]
    [SerializeField] TMP_Text physicalGrowthText;
    [SerializeField] TMP_Text magicalGrowthText;
    [SerializeField] TMP_Text socialGrowthText;
    [Space]
    [SerializeField] Image physicalStatBar;
    [SerializeField] Image magicalStatBar;
    [SerializeField] Image socialStatBar;
    [Space]
    [SerializeField] Image physicalPotentialStatBar;
    [SerializeField] Image magicalPotentialStatBar;
    [SerializeField] Image socialPotentialStatBar;
    [Space]
    [SerializeField] GameObject[] Rarity;
    [SerializeField] Image pic;

    private DefaultHero defaultHero;
    public PlayerHero playerHero;

    private string physicalPotential;
    private string magicalPotential;
    private string socialPotential;


    private float max = 999;

    #endregion


    // init
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateHero(PlayerHero hero)
    {
        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];

        playerHero = hero;

        Name.text = hero.heroId;

        physicalGrowthText.text = "+0";
        magicalGrowthText.text = "+0";
        socialGrowthText.text = "+0";


        //stats
        //check if potential is maxed

        CheckPotential(hero);

        //text
        physicalStatText.text = $"{hero.pVal} / {hero.pPot} ({physicalPotential})";
        magicalStatText.text = $"{hero.mVal} / {hero.mPot} ({magicalPotential})";
        socialStatText.text = $"{hero.sVal} / {hero.sPot} ({socialPotential})";

        //set actual stat
        physicalStatBar.fillAmount = hero.pVal / max;
        magicalStatBar.fillAmount = hero.mVal / max;
        socialStatBar.fillAmount = hero.sVal / max;



        //set rarity
        foreach(GameObject rarityStar in Rarity)
        {
            rarityStar.SetActive(false);
        }

        for (int i = 0; i <= defaultHero.rarity - 1; i++)
        {
            Rarity[i].SetActive(true);
        }

        if (SpriteStruct.SpriteDictionary.ContainsKey(playerHero.heroId))
            pic.sprite = SpriteStruct.SpriteDictionary[playerHero.heroId];
    }

    private void CheckPotential(PlayerHero hero)
    {
        //physical
        if (hero.pPot >= defaultHero.pMaxPot)
        {
            hero.pPot = defaultHero.pMaxPot;
            physicalPotential = "max";

            //bar
            physicalPotentialStatBar.fillAmount = 1 - defaultHero.pMaxPot / max;
        }

        else
        {
            physicalPotential = hero.pPot.ToString();

            //bar
            physicalPotentialStatBar.fillAmount = 1 - hero.pPot / max;

        }


        //magical
        if (hero.mPot >= defaultHero.mMaxPot)
        {
            hero.mPot = defaultHero.mMaxPot;
            magicalPotential = "max";

            //bar
            magicalPotentialStatBar.fillAmount = 1 - defaultHero.mMaxPot / max;
        }

        else
        {
            magicalPotential = hero.mPot.ToString();

            //bar
            magicalPotentialStatBar.fillAmount = 1 - hero.mPot / max;
        }


        //social
        if (hero.sPot >= defaultHero.sMaxPot)
        {
            hero.sPot = defaultHero.sMaxPot;
            socialPotential = "max";

            //bar
            socialPotentialStatBar.fillAmount = 1 - defaultHero.sMaxPot / max;
        }

        else
        {
            socialPotential = hero.sPot.ToString();

            //bar
            socialPotentialStatBar.fillAmount = 1 - hero.sPot / max;
        }
    }


    //animate
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public void UpdateSocialGrowth(int social)
    {
        if (social < 0)
        {
            if (playerHero.sVal != defaultHero.sMin)
            {
                if (playerHero.sVal - social >= defaultHero.sMin)
                    socialGrowthText.text = $"{social}";

            }

        }

        else
        {

            if (playerHero.sPot != playerHero.sVal)
            {
                if (playerHero.sVal + social <= playerHero.sPot)
                    socialGrowthText.text = $"+{social}";

            }
        }
    }

    public void UpdateMagicalGrowth(int magical)
    {
        if (magical < 0)
        { 
            if (playerHero.mVal != defaultHero.mMin)
            {
                if(playerHero.mVal - magical >= defaultHero.mMin)
                    magicalGrowthText.text = $"{magical}";
               
            }
        
        }

        else
        {

            if (playerHero.mPot != playerHero.mVal)
            {
                if (playerHero.mVal + magical <= playerHero.mPot) 
                    magicalGrowthText.text = $"+{magical}";

            }



        }
    }

    public void UpdatePhysicalGrowth(int physical)
    {
        if (physical < 0)
        {
            if (playerHero.pVal != defaultHero.pMin)
            {
                if (playerHero.pVal - physical >= defaultHero.pMin)
                    physicalGrowthText.text = $"{physical}";

            }

        }

        else
        {

            if (playerHero.pPot != playerHero.pVal)
            {
                if (playerHero.pVal + physical <= playerHero.pPot)
                    physicalGrowthText.text = $"+{physical}";

            }
        }
    }    
    


    public void UpdateSocialBarGrowth(int social)
    {
        if (social < 0)
        {
            if (playerHero.sVal != defaultHero.sMin)
            {
                if (playerHero.sVal - social >= defaultHero.sMin)
                    socialStatBar.fillAmount = (playerHero.sVal - social) / max;

            }

        }

        else
        {

            if (playerHero.sPot != playerHero.sVal)
            {
                if (playerHero.sVal + social <= playerHero.sPot)
                    socialStatBar.fillAmount = (playerHero.sVal + social) / max;


            }
        }
    }

    public void UpdateMagicalBarGrowth(int magical)
    {
        if (magical < 0)
        { 
            if (playerHero.mVal != defaultHero.mMin)
            {
                if (playerHero.mVal - magical >= defaultHero.mMin)
                    magicalStatBar.fillAmount = (playerHero.mVal - magical) / max;
            }
        
        }

        else
        {

            if (playerHero.mPot != playerHero.mVal)
            {
                if (playerHero.mVal + magical <= playerHero.mPot)
                    magicalStatBar.fillAmount = (playerHero.mVal + magical) / max;
            }
        }
    }

    public void UpdatePhysicalBarGrowth(int physical)
    {
        if (physical < 0)
        {
            if (playerHero.pVal != defaultHero.pMin)
            {
                if (playerHero.pVal - physical >= defaultHero.pMin)
                    physicalStatBar.fillAmount = (playerHero.pVal - physical) / max;
            }

        }

        else
        {

            if (playerHero.pPot != playerHero.pVal)
            {
                if (playerHero.pVal + physical <= playerHero.pPot)
                    physicalStatBar.fillAmount = (playerHero.pVal + physical) / max;
            }
        }
    }

}
