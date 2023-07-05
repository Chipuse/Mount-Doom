using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangeHeroes : MonoBehaviour
{
    #region vars
    [HideInInspector] public int currentMatch;
    [HideInInspector] public int totalMatch;
    [HideInInspector] public bool allMatch;
    [HideInInspector] public List<Match> allMatchList;
    [HideInInspector] public List<int[]> allMatchListPotential;


    [SerializeField] GameObject givenCard;
    [SerializeField] GameObject newCard;
    [SerializeField] GameObject stats;
    [SerializeField] TextMeshProUGUI statusText;
    [SerializeField] TextMeshProUGUI potentialText;
    [SerializeField] TextMeshProUGUI userText;
    [SerializeField] TextMeshProUGUI magPotText;
    [SerializeField] TextMeshProUGUI physPotText;
    [SerializeField] TextMeshProUGUI socPotText;    
    [SerializeField] TextMeshProUGUI magPotText2;
    [SerializeField] TextMeshProUGUI physPotText2;
    [SerializeField] TextMeshProUGUI socPotText2;
    [Space]
    [SerializeField] GameObject nextExchange;
    [SerializeField] GameObject FinishExchange;
    [SerializeField] GameObject FinishTrade;
    [SerializeField] GameObject ShowBuff;
    [SerializeField] Image physicalPotStatBar;
    [SerializeField] Image magicalPotStatBar;
    [SerializeField] Image socialPotStatBar;
    [SerializeField] float animSpeed;
    #endregion

    private void OnEnable()
    {
        physPotText.text = "";
        magPotText.text = "";
        socPotText.text = "";

        statusText.text = "";
        potentialText.text = "";

        FinishExchange.SetActive(false);
        FinishTrade.SetActive(false);
        nextExchange.SetActive(false);
        ShowBuff.SetActive(false);
        stats.SetActive(false);

        AudioManager.PlayEffect("tradeAway");

        ResetAndAnimate();
    }

    private void EnableButton()
    {
        //always
        ShowBuff.SetActive(true);

        if(allMatch && totalMatch == currentMatch + 1 && totalMatch != 0)
        {
            FinishExchange.SetActive(false);
            FinishTrade.SetActive(true);
            nextExchange.SetActive(false);
        }

        else if(totalMatch == currentMatch + 1 && totalMatch != 0)
        {
            FinishExchange.SetActive(true);
            FinishTrade.SetActive(false);
            nextExchange.SetActive(false);
        }

        else
        {
            FinishExchange.SetActive(false);
            FinishTrade.SetActive(false);
            nextExchange.SetActive(true);

            //currentMatch++;
        }

        
    }

    private void ResetAndAnimate()
    {
        //init
        givenCard.transform.localScale = new Vector3(1, 1, 1);
        newCard.transform.localScale = new Vector3(0,0,0);

        givenCard.SetActive(true);
        newCard.SetActive(false);


        //update cards according to match
        givenCard.GetComponent<ExchangeCardAnim>().UpdateHero(allMatchList[currentMatch].ownHero);
        newCard.GetComponent<ExchangeCardNew>().UpdateHero(allMatchList[currentMatch].matchedOffer.heroId);

        //update match text
        userText.text = $"Found a match with {allMatchList[currentMatch].matchedOffer.available}.";
        statusText.text = $"You say goodbye to {allMatchList[currentMatch].ownHero.heroId}.";

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(animSpeed);

        //remove old card
        LeanTween.scale(givenCard, new Vector3(0, 0, 0), animSpeed);

        yield return new WaitForSeconds(animSpeed*1.5f);

        AudioManager.PlayEffect("tradeReceive");


        //get new card
        statusText.text = $"You welcome {allMatchList[currentMatch].matchedOffer.heroId}.";

        givenCard.SetActive(false);
        newCard.SetActive(true);

        newCard.transform.localScale = new Vector3(0, 0, 0);
        givenCard.transform.localScale = new Vector3(1, 1, 1);

        LeanTween.scale(newCard, new Vector3(1,1,1), animSpeed);

        yield return new WaitForSeconds(animSpeed * 1.5f);


        //check if potential growth

        bool pot = false;

        foreach (int buff in allMatchListPotential[currentMatch])
        { 
            if(buff > 0)
            {
                pot = true;
                break;
            }
        }

        if (pot)
        {
            statusText.text = "";

            potentialText.text = "unlocked higher potential.";


            stats.SetActive(true);

            LeanTween.value(gameObject, 0f, 1f, animSpeed)
            .setEaseInOutExpo()
            .setOnUpdate(setGrowth);

            LeanTween.value(gameObject, 1f, 0f, animSpeed)
                .setEaseInOutExpo()
                .setOnUpdate(setBarGrowth);

            yield return new WaitForSeconds(1f);
        }

        else
        {
            statusText.text = "";
        }

        EnableButton();
    }


    private void setGrowth(float i)
    {
        physPotText.text = $"+ {(int)(allMatchList[currentMatch].GetBuffDiff()[0] * i)}";
        magPotText.text = $"+ {(int)(allMatchList[currentMatch].GetBuffDiff()[1] * i)}";
        socPotText.text = $"+ {(int)(allMatchList[currentMatch].GetBuffDiff()[2] * i)}";

    }

    private void setBarGrowth(float i)
    {
        DefaultHero hero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[allMatchList[currentMatch].matchedOffer.heroId];

        physPotText2.text = $"{hero.pDef}/{(int)(allMatchList[currentMatch].GetCalcPotentials()[0] - ((allMatchList[currentMatch].GetBuffDiff()[0] * i)))}";
        magPotText2.text = $"{hero.pDef}/{(int)(allMatchList[currentMatch].GetCalcPotentials()[1] - ((allMatchList[currentMatch].GetBuffDiff()[0] * i)))}";
        socPotText2.text = $"{hero.pDef}/ {(int)(allMatchList[currentMatch].GetCalcPotentials()[2] - ((allMatchList[currentMatch].GetBuffDiff()[0] * i)))}";

        physicalPotStatBar.fillAmount = 1 - (((allMatchList[currentMatch].GetCalcPotentials()[0] -(allMatchList[currentMatch].GetBuffDiff()[0] * i)) / 999));
        magicalPotStatBar.fillAmount = 1 - (((allMatchList[currentMatch].GetCalcPotentials()[1] - (allMatchList[currentMatch].GetBuffDiff()[1] * i)) / 999));
        socialPotStatBar.fillAmount = 1 - (((allMatchList[currentMatch].GetCalcPotentials()[2] - (allMatchList[currentMatch].GetBuffDiff()[2] * i)) / 999));

    }



}
