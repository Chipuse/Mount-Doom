using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DexCard : MonoBehaviour
{
    [SerializeField] GameObject LockedCard;
    [SerializeField] GameObject UnlockedCard;

    [SerializeField] TextMeshProUGUI heroName;
    [SerializeField] GameObject[] rarityGraphics;
    [SerializeField] GameObject rarityGroup;
    [SerializeField] Image portrait;


    public void UpdateState(int state)
    {
        switch (state)
        {
            case 0:
                LockedCard.SetActive(true);
                UnlockedCard.SetActive(false);
                break;
            default:
                LockedCard.SetActive(false);
                UnlockedCard.SetActive(true);

                break;
        }
    }

    public void UpdateDexCard(DefaultHero defaultHero)
    
    {
        heroName.text = defaultHero.heroId;
        int rarity = DatabaseManager._instance.defaultHeroData.defaultHeroDictionary[defaultHero.heroId].rarity;

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

        if (SpriteStruct.SpriteDictionary.ContainsKey(defaultHero.heroId))
            portrait.sprite = SpriteStruct.SpriteDictionary[defaultHero.heroId];

    }
    
}
