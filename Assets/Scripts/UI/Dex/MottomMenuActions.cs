using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MottomMenuActions : MonoBehaviour
{
    [SerializeField] GameObject OpenButton;
    [SerializeField] GameObject CreditsButton;
    [SerializeField] GameObject HelpButton;
    [SerializeField] GameObject HallOfFameButton;
    [SerializeField] TextMeshProUGUI hallOfFameText;

    [SerializeField] string URL;

    private void Start()
    {
        OpenButton.GetComponent<Button>().onClick.AddListener(() => { OpenDex(); });
        CreditsButton.GetComponent<Button>().onClick.AddListener(() => { OpenCredit(); });
        HelpButton.GetComponent<Button>().onClick.AddListener(() => { HelpLink(); });
        HallOfFameButton.GetComponent<Button>().onClick.AddListener(() => { OpenHallOfFame(); });
    }

    private void OpenDex()
    {
        UIEnablerManager.Instance.EnableElement("Dex", true);
    }

    private void OpenCredit()
    {
        UIEnablerManager.Instance.EnableElement("Credits", true);
    }
    private void OpenHallOfFame()
    {
        UIEnablerManager.Instance.EnableElement("HallOfFame", true);
        //UpdateHallOfFame from global data
        if(hallOfFameText != null)
        {
            hallOfFameText.text = "<size=180>Hall of Fame\n</size> ";
            hallOfFameText.text += "<size=80>the valiant people who assembled the heroes of Mount Doom\n</size> ";
            if(DatabaseManager._instance.globalData.fameData != null) 
            {
                foreach (var item in DatabaseManager._instance.globalData.fameData)
                {
                    hallOfFameText.text += "<size=50>___________________________________\n</size> ";
                    hallOfFameText.text += "<size=140>"+ item.name + "\n</size> ";
                    hallOfFameText.text += "<size=40>"+ item.date + "\n</size> ";
                }
            }
        }
    }

    private void HelpLink()
    {
        Application.OpenURL(URL.Replace("SomeUserName", DatabaseManager._instance.activePlayerData.playerId));
    }
}
