using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateHeroCard : MonoBehaviour
{
    #region vars
    //vars for front
    [Space]
    [Header("Card Front")]
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Race;
    [SerializeField] TMP_Text Job;
    [SerializeField] TMP_Text BackName;
    [SerializeField] TMP_Text BackRace;
    [SerializeField] TMP_Text BackJob;
    [Space]
    [SerializeField] TMP_Text physicalStatText;
    [SerializeField] TMP_Text magicalStatText;
    [SerializeField] TMP_Text socialStatText;
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
    [Space]
    [SerializeField] TMP_Text BuffText;
    [SerializeField] TMP_Text DebuffText;
    [SerializeField] TMP_Text PathText;
    [Space]
    [SerializeField] Image BuffImage;
    [SerializeField] Image DebuffImage;
    [SerializeField] Image PathImage;


    //vars for detail
    [Space]
    [Header("Card Detail")]
    [SerializeField] TMP_Text descriptionContent;
    [Space]
    [SerializeField] TMP_Text originalOwner;
    [SerializeField] TMP_Text formerOwner;
    [Space]
    [SerializeField] TMP_Text dungeonAmount;
    [SerializeField] TMP_Text tradeAmount;

    public PlayerHero publicHero;

    private DefaultHero defaultHero;

    private string physicalPotential;
    private string magicalPotential;
    private string socialPotential;

    private string physicalStat;
    private string magicalStat;
    private string socialStat;

    private float max = 999;

    [SerializeField] ScrollSnapButton scroll;
    #endregion



    public void UpdateHero(PlayerHero hero)
    {
        publicHero = hero;
        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];

        Name.text = hero.heroId;
        Race.text = defaultHero.race;
        Job.text = defaultHero.job;
        BackName.text = hero.heroId;
        BackRace.text = defaultHero.race;
        BackJob.text = defaultHero.job;


        //stats
        //check if potential is maxed
        
        CheckPotential(hero);
        CheckStats(hero);

        //adjust buttons
        UpdateScrollSnap();


        //text
        physicalStatText.text = $"{physicalStat} / {physicalPotential}";
        magicalStatText.text = $"{magicalStat} / {magicalPotential}";
        socialStatText.text = $"{socialStat} / {socialPotential}";




        //set rarity
        foreach(GameObject rarityStar in Rarity)
        {
            rarityStar.SetActive(false);
        }

        for(int i = 0; i <= defaultHero.rarity - 1; i++ )
        {
            Rarity[i].SetActive(true);
        }


        //Buff Debuff Path
        //images
        if (IconStruct.IconDictionary.ContainsKey(defaultHero.nodeBuff))
        {
            BuffImage.sprite = IconStruct.IconDictionary[defaultHero.nodeBuff].sprite;
            BuffImage.color = IconStruct.IconDictionary[defaultHero.nodeBuff].color;
        }

        if (IconStruct.IconDictionary.ContainsKey(defaultHero.nodeDebuff))
        {
            DebuffImage.sprite = IconStruct.IconDictionary[defaultHero.nodeDebuff].sprite;
            DebuffImage.color = IconStruct.IconDictionary[defaultHero.nodeDebuff].color;
        }

        if (IconStruct.IconDictionary.ContainsKey(defaultHero.pathAff))
        {
            PathImage.sprite = IconStruct.IconDictionary[defaultHero.pathAff].sprite;
            PathImage.color = IconStruct.IconDictionary[defaultHero.pathAff].color;
        }

        //text
        switch (defaultHero.nodeBuff)
        {
            case "seaside":
                BuffText.text = "I enjoy swimming.";
                break;
            case "ruins":
                BuffText.text = "I want to explore ruins.";
                break;
            case "mountains":
                BuffText.text = "I love to climb.";
                break;
            case "plain":
                BuffText.text = "I want to travel plains.";
                break;
            case "settlement":
                BuffText.text = "I like visiting settlements.";
                break;
            case "forest":
                BuffText.text = "I long for nature.";
                break;
            default:
                Debug.Log("node buff not found" + defaultHero.nodeBuff);
                break;
        }

        switch (defaultHero.nodeDebuff)
        {
            case "seaside":
                DebuffText.text = "I hate fluids.";
                break;
            case "ruins":
                DebuffText.text = "I'm affraid of ghosts.";
                break;
            case "mountains":
                DebuffText.text = "I'm scared of heights.";
                break;
            case "plain":
                DebuffText.text = "I don't like open plains.";
                break;
            case "settlement":
                DebuffText.text = "I despise urbanization.";
                break;
            case "forest":
                DebuffText.text = "I have pollen allergies.";
                break;
            default:
                Debug.Log("node debuff not found" + defaultHero.nodeDebuff);
                break;
        }

        switch(defaultHero.pathAff)
        {
            case "sand":
                PathText.text = "Soft ground is good for my knees.";
                break;
            case "cobblestone":
                PathText.text = "I want to walk on paved trails.";
                break;
            case "swamp":
                PathText.text = "I like the smell of swamps.";
                break;
            case "logging":
                PathText.text = "Logged paths make nice noises.";
                break;
            default:
                Debug.Log("path type not found " + defaultHero.pathAff);
                break;
        }




        //card detail
        descriptionContent.text = defaultHero.description;


        if (hero.origOwner != "")
            originalOwner.text = hero.origOwner;

        else
            originalOwner.text = DatabaseManager._instance.activePlayerData.playerId;


        if (hero.traded > 0)
        {
            tradeAmount.text = hero.traded.ToString();
            formerOwner.text = hero.lastOwner;
        }

        else
        {
            tradeAmount.text = "0";
            formerOwner.text = "-";
        }

        if (hero.runs > 0)
            dungeonAmount.text = hero.runs.ToString();

        else
            dungeonAmount.text = "0";


    }

    private void UpdateScrollSnap()
    {
        if (scroll.prevButton.activeSelf)
            scroll.prevButton.GetComponent<Button>().onClick.Invoke();
    }

    private void CheckPotential(PlayerHero hero)
    {
        //physical
        if (hero.pPot >= defaultHero.pMaxPot)
        {
            hero.pPot = defaultHero.pMaxPot;
            physicalPotential = $"{hero.pPot} (max)";

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
            magicalPotential = $"{hero.mPot} (max)";

            //bar
            magicalPotentialStatBar.fillAmount = 1 - defaultHero.mMaxPot/ max;
        }

        else
        {
            magicalPotential = hero.mPot.ToString();

            //bar
            magicalPotentialStatBar.fillAmount = 1 - hero.mPot/max;
        }


        //social
        if (hero.sPot >= defaultHero.sMaxPot)
        {
            hero.sPot = defaultHero.sMaxPot;
            socialPotential = $"{hero.sPot} (max)";

            //bar
            socialPotentialStatBar.fillAmount = 1 - defaultHero.sMaxPot/max;
        }

        else
        {
            socialPotential = hero.sPot.ToString();
            
            //bar
            socialPotentialStatBar.fillAmount = 1 - hero.sPot/ max;
        }
    }

    private void CheckStats(PlayerHero hero)
    {
        //physical
        if (hero.pPot == hero.pVal)
            physicalStat = $"{hero.pVal} (max)";

        else
            physicalStat = $"{hero.pVal}";


        if (hero.pVal != hero.pPot)
        {
            physicalStatBar.fillAmount = hero.pVal / max;
            targetFillP = hero.pVal / max;

        }

        else
        {
            physicalStatBar.fillAmount = 1;
            targetFillP = 1;
        }

        //magical
        if (hero.mPot == hero.mVal)
            magicalStat = $"{hero.mVal} (max)";

        else
            magicalStat = $"{hero.mVal}";


        if (hero.mPot != hero.mVal)
        {
            magicalStatBar.fillAmount = hero.mVal / max;
            targetFillM = hero.mVal / max;
        }

        else
        {
            magicalStatBar.fillAmount = 1;
            targetFillM = 1;
        }

        //physical
        if (hero.sPot == hero.sVal)
            socialStat = $"{hero.sVal} (max)";

        else
            socialStat = $"{hero.sVal}";

        if(hero.sPot != hero.sVal)
        {
            socialStatBar.fillAmount = hero.sVal / max;
            targetFillS = hero.sVal / max;
        }

        else
        {
            socialStatBar.fillAmount = 1;
            targetFillS = 1;
        }
    }

    private void OnEnable()
    {
        magicalStatBar.fillAmount = targetFillM;
        physicalStatBar.fillAmount = targetFillP;
        socialStatBar.fillAmount = targetFillS;
    }

    float targetFillM;
    float targetFillP;
    float targetFillS;

    private void Update()
    {
       if(magicalStatBar.fillAmount != targetFillM)
            magicalStatBar.fillAmount = targetFillM;

       if(physicalStatBar.fillAmount != targetFillP)
           physicalStatBar.fillAmount = targetFillP;

       if(socialStatBar.fillAmount != targetFillS)
           socialStatBar.fillAmount = targetFillS;
    }


    public void UpdateHero(DefaultHero Hero, string origOwner, string lastOwner ,int runs, int traded)
    {

        defaultHero = Hero;

        Name.text = defaultHero.heroId;
        Race.text = defaultHero.race;
        Job.text = defaultHero.job;
        BackName.text = defaultHero.heroId;
        BackRace.text = defaultHero.race;
        BackJob.text = defaultHero.job;


        //stats
        //check if potential is maxed

        CheckPotential(defaultHero);
        CheckStats(defaultHero);

        //adjust buttons
        UpdateScrollSnap();


        //text
        physicalStatText.text = $"{physicalStat} / {physicalPotential}";
        magicalStatText.text = $"{magicalStat} / {magicalPotential}";
        socialStatText.text = $"{socialStat} / {socialPotential}";




        //set rarity
        foreach (GameObject rarityStar in Rarity)
        {
            rarityStar.SetActive(false);
        }

        for (int i = 0; i <= defaultHero.rarity - 1; i++)
        {
            Rarity[i].SetActive(true);
        }


        //Buff Debuff Path
        //images
        if (IconStruct.IconDictionary.ContainsKey(defaultHero.nodeBuff))
        {
            BuffImage.sprite = IconStruct.IconDictionary[defaultHero.nodeBuff].sprite;
            BuffImage.color = IconStruct.IconDictionary[defaultHero.nodeBuff].color;
        }

        if (IconStruct.IconDictionary.ContainsKey(defaultHero.nodeDebuff))
        {
            DebuffImage.sprite = IconStruct.IconDictionary[defaultHero.nodeDebuff].sprite;
            DebuffImage.color = IconStruct.IconDictionary[defaultHero.nodeDebuff].color;
        }

        if (IconStruct.IconDictionary.ContainsKey(defaultHero.pathAff))
        {
            PathImage.sprite = IconStruct.IconDictionary[defaultHero.pathAff].sprite;
            PathImage.color = IconStruct.IconDictionary[defaultHero.pathAff].color;
        }

        //text
        switch (defaultHero.nodeBuff)
        {
            case "seaside":
                BuffText.text = "I enjoy swimming.";
                break;
            case "ruins":
                BuffText.text = "I want to explore ruins.";
                break;
            case "mountains":
                BuffText.text = "I love to climb.";
                break;
            case "plain":
                BuffText.text = "I want to travel plains.";
                break;
            case "settlement":
                BuffText.text = "I like visiting settlements.";
                break;
            case "forest":
                BuffText.text = "I long for nature.";
                break;
            default:
                Debug.Log("node buff not found" + defaultHero.nodeBuff);
                break;
        }

        switch (defaultHero.nodeDebuff)
        {
            case "seaside":
                DebuffText.text = "I hate fluids.";
                break;
            case "ruins":
                DebuffText.text = "I'm affraid of ghosts.";
                break;
            case "mountains":
                DebuffText.text = "I'm scared of heights.";
                break;
            case "plain":
                DebuffText.text = "I don't like open plains.";
                break;
            case "settlement":
                DebuffText.text = "I despise urbanization.";
                break;
            case "forest":
                DebuffText.text = "I have pollen allergies.";
                break;
            default:
                Debug.Log("node debuff not found" + defaultHero.nodeDebuff);
                break;
        }

        switch (defaultHero.pathAff)
        {
            case "sand":
                PathText.text = "Soft ground is good for my knees.";
                break;
            case "cobblestone":
                PathText.text = "I want to walk on paved trails.";
                break;
            case "swamp":
                PathText.text = "I like the smell of swamps.";
                break;
            case "logging":
                PathText.text = "Logged paths make nice noises.";
                break;
            default:
                Debug.Log("path type not found " + defaultHero.pathAff);
                break;
        }




        //card detail
        descriptionContent.text = defaultHero.description;


        if (origOwner != "")
            originalOwner.text = origOwner;

        else
            originalOwner.text = DatabaseManager._instance.activePlayerData.playerId;


        if (traded > 0)
        {
            tradeAmount.text = traded.ToString();
            formerOwner.text = lastOwner;
        }

        else
        {
            tradeAmount.text = "0";
            formerOwner.text = "-";
        }

        if (runs > 0)
            dungeonAmount.text = traded.ToString();

        else
            dungeonAmount.text = "0";


    }


    private void CheckPotential(DefaultHero hero)
    {
        //physical

            physicalPotential = hero.pDefPot.ToString();
            
            physicalPotentialStatBar.fillAmount = 1 - defaultHero.pDefPot / max;





        //magical

            magicalPotential = hero.mDefPot.ToString();

            magicalPotentialStatBar.fillAmount = 1 - hero.mDefPot / max;
        
        //social
            socialPotential = hero.sDefPot.ToString();

            socialPotentialStatBar.fillAmount = 1 - hero.sDefPot / max;
       
    }

    private void CheckStats(DefaultHero hero)
    {
        //physical

        physicalStat = $"{hero.pDef}";

        physicalStatBar.fillAmount = hero.pDef / max;
        targetFillP = hero.pDef / max;


        //magical
        magicalStat = $"{hero.mDef}";

        magicalStatBar.fillAmount = hero.mDef / max;
        targetFillM = hero.mDef / max;


        //social

        socialStat = $"{hero.sDef}";

        socialStatBar.fillAmount = hero.sDef / max;
        targetFillS = hero.sDef / max;
        
    }

}
