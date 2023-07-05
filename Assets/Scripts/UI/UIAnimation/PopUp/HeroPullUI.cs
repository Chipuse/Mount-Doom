using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroPullUI : MonoBehaviour
{
    #region vars

    [SerializeField] GameObject ReleaseText;

    [SerializeField] GameObject ReleaseButton;
    [SerializeField] GameObject ContinueButton;

    [SerializeField] GameObject doublecard;


    [SerializeField] GameObject card;
    [SerializeField] GameObject blackCard;
    [SerializeField] GameObject blink;

    [SerializeField] float pullAnimSpeed;

    [SerializeField] InventoryUI inventory;
    [SerializeField] float discardAnimSpeed;
    [Space]
    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] GameObject [] rarity;

    DefaultHero defaultHero;


    [SerializeField]UpdateHeroCard heroCard;
    //for update


    #endregion
    private void Start()
    {
        doublecard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
    }

    private void OnEnable()
    {
        AudioManager.PlayEffect("pull");

        blackCard.SetActive(true);

        blink.SetActive(false);
        ReleaseText.SetActive(false);
        ContinueButton.SetActive(false);
        ReleaseButton.SetActive(false);

        UpdatePulledHero(DungeonManager._instance.rewardHero);


        if (DatabaseManager._instance.activePlayerData.inventory.Count >= inventory.heroSlots.Length)
        {
            StartCoroutine(DiscardPull());
        }

        else
        {
            StartCoroutine(PullHero());
        }
    }

    private void UpdatePulledHero(PlayerHero hero)
    {
        //pop up
        heroCard.UpdateHero(hero);

        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];


        //set rarity
        foreach (GameObject rarityStar in rarity)
        {
            rarityStar.SetActive(false);
        }

        for (int i = 0; i <= defaultHero.rarity - 1; i++)
        {
            rarity[i].SetActive(true);
        }

        //texts
        name.text = hero.heroId;


        //image
        if (SpriteStruct.SpriteDictionary.ContainsKey(hero.heroId))
            Image.sprite = SpriteStruct.SpriteDictionary[hero.heroId];
    }

    private void DoubleClick(int i)
    {
        UIEnablerManager.Instance.EnableElement("HeroCard", true);
    }


    IEnumerator PullHero()
    {
        yield return new WaitForSeconds(pullAnimSpeed);


        blackCard.SetActive(true);
        blackCard.transform.localScale = new Vector3(1, 1, 1);

        //
        //pull animation shake
        LeanTween.rotateZ(card, 20, pullAnimSpeed).setEaseInOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed);


        LeanTween.rotateZ(card, -20, pullAnimSpeed).setEaseInOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed);

        LeanTween.rotateZ(card, 0 , pullAnimSpeed).setEaseInOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed *2);


        //
        //reveal
        blink.SetActive(true);
        LeanTween.value(blink, 0, 1, pullAnimSpeed/2)
            .setOnUpdate(setColor)
            .setEaseInExpo();
        yield return new WaitForSeconds(pullAnimSpeed/1.5f);

        //show hero
        blackCard.SetActive(false);
        //set buttons
        ContinueButton.SetActive(true);
        ReleaseButton.SetActive(false);

        LeanTween.value(blink, 1, 0, pullAnimSpeed/2)
            .setOnUpdate(setColor)
            .setEaseOutExpo();

        AudioManager.PlayEffect("reward");
        yield return new WaitForSeconds(pullAnimSpeed/2);



        //wrap up
        blink.SetActive(false);

        yield return null;
    }

    IEnumerator DiscardPull()
    {
        yield return new WaitForSeconds(pullAnimSpeed);

        blackCard.SetActive(true);
        blackCard.transform.localScale = new Vector3(1, 1, 1);


        //
        //pull animation shake
        LeanTween.rotateZ(card, 20, pullAnimSpeed).setEaseInOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed);

        LeanTween.rotateZ(card, -20, pullAnimSpeed).setEaseInOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed);

        LeanTween.rotateZ(card, 0, pullAnimSpeed).setEaseInOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed *2);

        //
        //reveal
        blink.GetComponent<Image>().color = new Color(blink.GetComponent<Image>().color.r, blink.GetComponent<Image>().color.g, blink.GetComponent<Image>().color.b, 0);
        blink.SetActive(true);
        LeanTween.value(blink, 0, 1, pullAnimSpeed/2)
            .setOnUpdate(setColor)
            .setEaseInExpo();
        yield return new WaitForSeconds(pullAnimSpeed/1.5f);


        //set buttons
        ContinueButton.SetActive(false);
        ReleaseButton.SetActive(true);
        ReleaseText.SetActive(true);
        //show hero
        blackCard.SetActive(false);

        LeanTween.value(blink,1,0,pullAnimSpeed/2)
            .setOnUpdate(setColor)
            .setEaseOutExpo();
        yield return new WaitForSeconds(pullAnimSpeed/2);

        AudioManager.PlayEffect("reward");


        //wrap up
        blink.SetActive(false);

        //ReleaseText.transform.localScale = new Vector3(1, 0, 1);

        //LeanTween.scaleY(ReleaseText, 1, discardAnimSpeed).setEaseOutBounce();
        yield return null;
    }

    private void setColor(float value)
    {
        blink.GetComponent<Image>().color = new Color(blink.GetComponent<Image>().color.r, blink.GetComponent<Image>().color.g, blink.GetComponent<Image>().color.b, value);
    }
}
