using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SwipeSlot : MonoBehaviour
{
    #region vars
    [HideInInspector] public PlayerHero playerHero;

    [HideInInspector] public int slotID;
    [HideInInspector] public int originalSlotReferenceID;

    [SerializeField] public GameObject heroCard;
    [SerializeField] private GameObject disabledCard;

    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject[] rarityGraphics;
    [SerializeField] TMP_Text heroName;
    [SerializeField] Image portrait;


    [SerializeField] GameObject matched;
    [SerializeField] GameObject highlight;
    [HideInInspector] public bool IsMatched;


    public event Action <int> OnClickEvent;
    public event Action <int> OnDoubleClickEvent;

    #endregion

    private void Start()
    {
        heroCard.GetComponent<Button>().onClick.AddListener(() => OnClick());
        heroCard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += OnDoubleClickEvent;
    }

    public void updateHero(PlayerHero hero, Sprite sprite, int rarity, int referenceID)
    {
        playerHero = hero;
        originalSlotReferenceID = referenceID;

        portrait.sprite = sprite;
        heroName.text = hero.heroId;

        int spacing = -130;

        foreach (GameObject gameObject in rarityGraphics)
        {
            gameObject.SetActive(false);
        }

        for (int i = 0; i < rarity; i++)
        {
            spacing += 20;
            rarityGraphics[i].SetActive(true);
        }

        rarityGroup.GetComponent<HorizontalLayoutGroup>().spacing = spacing;

        if (SpriteStruct.SpriteDictionary.ContainsKey(playerHero.heroId))
            portrait.sprite = SpriteStruct.SpriteDictionary[playerHero.heroId];
    }

    public void showHero()
    {
        matched.SetActive(false);
        highlight.SetActive(false);
        disabledCard.SetActive(false);
        heroCard.SetActive(true);
    }

    //public funcs
    public void hideHero()
    {
        heroCard.SetActive(false);
        disabledCard.SetActive(true);
    }

    public void matchHero()
    {
        matched.SetActive(true);
        IsMatched = true;
    }

    public void unmatchHero()
    {
        matched.SetActive(false);
        IsMatched = false;
    }

    public void enableHighlight()
    {
        highlight.SetActive(true);
    }    
    
    public void disableHighlight()
    {
        highlight.SetActive(false);
    }
    //drag





    //click
    public void OnClick()
    {
        OnClickEvent(slotID);
    }

}
