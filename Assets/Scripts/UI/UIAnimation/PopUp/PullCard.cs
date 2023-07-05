using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PullCard : MonoBehaviour
{
    #region vars
    [SerializeField] TMP_Text Name;
    [Space]
    [SerializeField] GameObject[] Rarity;

    private DefaultHero defaultHero;
    private PlayerHero playerHero;

    #endregion

    private void OnEnable()
    {
        //give me the hero
        //UpdateHero()
    }

    public void UpdateHero(PlayerHero hero)
    {
        defaultHero = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[hero.heroId];

        playerHero = hero;

        Name.text = hero.heroId;

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
