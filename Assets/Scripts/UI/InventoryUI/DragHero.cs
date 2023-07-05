using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DragHero : MonoBehaviour
{
    [SerializeField] GameObject rarityGroup;
    [SerializeField] GameObject[] rarityGraphics;
    [SerializeField] TMP_Text heroName;
    [SerializeField] Image portrait;

    [SerializeField] GameObject TradeStatusUI;
    [SerializeField] GameObject DungeonStatusUI;

    private void OnEnable()
    {
        AudioManager.PlayEffect("drag");
    }

    private void OnDisable()
    {
        AudioManager.PlayEffect("drop");
    }

    public void updateDragHero(PlayerHero hero, Sprite sprite, int rarity)
    {
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


        switch (hero.status)
        {
            case HeroStatus.Trading:
                TradeStatusUI.SetActive(true);
                DungeonStatusUI.SetActive(false);
                break;
            case HeroStatus.Exploring:
                TradeStatusUI.SetActive(false);
                DungeonStatusUI.SetActive(true);
                break;
            default:
                TradeStatusUI.SetActive(false);
                DungeonStatusUI.SetActive(false);
                break;
        }
    }
}

