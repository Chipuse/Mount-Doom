using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangeCardNew : MonoBehaviour
{

    #region vars
    [SerializeField] TMP_Text Name;
    [SerializeField] Image portrait;
    [Space]
    [SerializeField] GameObject[] Rarity;

    private DefaultHero defaultHero;
    #endregion

    public void UpdateHero(string heroId)
    {
        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[heroId];

        Name.text = heroId;

        if (SpriteStruct.SpriteDictionary.ContainsKey(heroId))
            portrait.sprite = SpriteStruct.SpriteDictionary[heroId];

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
