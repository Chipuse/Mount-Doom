using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FormsManager : MonoBehaviour
{
    public static FormsManager _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        StartCoroutine(AutoplayRoutine());
    }

    IEnumerator AutoplayRoutine()
    {
        while (DatabaseManager._instance.activePlayerData.answeredForms.Count < DatabaseManager._instance.globalData.formData.Count)
        {
            TryDisplayForm();
            yield return new WaitForSeconds(4);
        }
    }

    public void TryDisplayForm()
    {
        if(RedriectToLinkActions.displayText != "inactive")
        {
            return;
        }
        foreach (var item in DatabaseManager._instance.globalData.formData)
        {
            if (!DatabaseManager._instance.activePlayerData.answeredForms.Contains(item.title) && CheckFormCondition(item)) //+condition is met! ToDo
            {
                //Do the Routine for the ui
                RedriectToLinkActions.displayText = item.message;
                RedriectToLinkActions.formName = item.title;
                RedriectToLinkActions.URL = item.link.Replace("SomeUserName", DatabaseManager._instance.activePlayerData.playerId);
                UIEnablerManager.Instance.EnableElement("InfoAvailable", true);
                break;
            }
        }
    }

    public bool CheckFormCondition(FormEntry _entry)
    {
        if (_entry.conVal == "")
            return true;
        int conVal = int.Parse(_entry.conVal);
        if (conVal <= 0)
            return true;
        switch (_entry.condition)
        {
            case "days":
                if (DateTime.Now.ToUniversalTime().Subtract(DateTime.Parse(DatabaseManager._instance.activePlayerData.joinDate).ToUniversalTime()).TotalDays >= conVal)
                    return true;
                break;
            case "runs":
                if (DatabaseManager._instance.activePlayerData.mtdCounter >= conVal)
                    return true;
                break;
            case "trades":
                if (DatabaseManager._instance.activePlayerData.tradeCounter >= conVal)
                    return true;
                break;
            case "heroCount":
                if (DatabaseManager._instance.activePlayerData.DexCount() >= conVal)
                    return true;
                break;
            default:
                return true;
        }
        return false;
    }
}
