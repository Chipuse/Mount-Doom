using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangeCardAnim : MonoBehaviour
{
    #region vars
    [SerializeField] TMP_Text Name;
    [SerializeField] Image portrait;
    [Space]
    [SerializeField] GameObject[] Rarity;

    private DefaultHero defaultHero;
    private PlayerHero playerHero;

    #endregion

    public void UpdateHero(PlayerHero hero)
    {
        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];

        playerHero = hero;

        Name.text = hero.heroId;

        if (SpriteStruct.SpriteDictionary.ContainsKey(playerHero.heroId))
            portrait.sprite = SpriteStruct.SpriteDictionary[playerHero.heroId];

        foreach (GameObject rarityStar in Rarity)
        {
            rarityStar.SetActive(false);
        }

        for (int i = 0; i <= defaultHero.rarity - 1; i++)
        {
            Rarity[i].SetActive(true);
        }
    }
}
