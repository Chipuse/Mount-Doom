using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradeObserveUpdate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI [] texts;
    [SerializeField] Image [] bars;
    [SerializeField] GameObject [] done;

    private List<string> textFilled = new List<string>();

    private void Start()
    {
        InitObserve();
        DeleventSystem.TradeStart += InitObserve;
    }

    private void InitObserve()
    {
        if(textFilled.Count ==0)
         textFilled.Clear();
            { 

                foreach(TextMeshProUGUI uGUI in texts)
                {
                    textFilled.Add(uGUI.text);
                    uGUI.text = "???";
                }

                foreach(Image image in bars)
                {
                    image.fillAmount = 0;
                }

                foreach (GameObject gameObject in done)
                {
                    gameObject.SetActive(false);
                }
            }
        }

    public void UpdateObserve(float value)
    {
        if (textFilled.Count < texts.Length)
            return;

        float calcHelper = texts.Length* value;

        float leftOver = calcHelper % 1f;

        int wholeNumb = (int)(calcHelper);


        for (int i = 0; i < wholeNumb; i++)
        {
            texts[i].text = textFilled[i];
            bars[i].fillAmount = 1;
            done[i].SetActive(true);
        }

        texts[wholeNumb].text = textFilled[wholeNumb];
        bars[wholeNumb].fillAmount = leftOver;
    }


    public void EndTrade()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if(textFilled.Count == texts.Length)
                texts[i].text = textFilled[i];
        }

        foreach (Image image in bars)
        {
            image.fillAmount = 1;
        }

        foreach (GameObject gameObject in done)
        {
            gameObject.SetActive(true);
        }
    }
}

