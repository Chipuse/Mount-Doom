using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchHero : MonoBehaviour
{
    #region vars

    [SerializeField] TextMeshProUGUI ownerName;
    [SerializeField] TextMeshProUGUI heroName;
    [SerializeField] GameObject [] rarityGraphics;
    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject matchCard;
    [SerializeField] UpdateHeroCard updateCard;
    [SerializeField] Image portrait;

    [HideInInspector] private TradeOffer offer;

    #endregion

    private void Awake()
    {
        matchCard.GetComponent<ButtonDoubleClickListener>().onDoubleClick += DoubleClick;
    }

    public void UpdateMatchHero(TradeOffer tradeOffer)
    {
        offer = tradeOffer;

        ownerName.text = offer.playerId;
        heroName.text = offer.heroId;
        int rarity = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[offer.heroId].rarity;

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

        if (SpriteStruct.SpriteDictionary.ContainsKey(offer.heroId))
            portrait.sprite = SpriteStruct.SpriteDictionary[offer.heroId];

    }


    private void DoubleClick(int i)
    {
        updateCard.UpdateHero(DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[offer.heroId], offer.origOwner, offer.lastOwner, offer.runs, offer.traded);

        UIEnablerManager.Instance.EnableElement("HeroCard", true);
    }
}
