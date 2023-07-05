using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//FlavourTexts
#region
//TextsEnemyName
//TextsHeroTurn
//TextsPathChoosingFlavour
//TextsPathHandlingFlavour
//TextsPathHandlingFlavour
//TextsEnemyTurnFlavour

[System.Serializable]
public class TextFlavours
{

    public TextEnemyName[] textsEnemyNames;
    public string GetRandomName(string _nodeType = "")
    {
        if (textsEnemyNames == null)
        {
            return "no options";
        }
        List<string> options = new List<string>();
        foreach (var item in textsEnemyNames)
        {
            if(item.optionalNodeType == "" || item.optionalNodeType == "none" || item.optionalNodeType == _nodeType)
            {
                options.Add(item.name);
            }
        }
        if(options.Count > 0)
        {
            return options[DungeonManager._instance.currentCalcRun.RandomNum(0, options.Count)];
        }
        return "no options";
    }

    public TextPathChoosing[] textsPathChoosing;
    public string GetRandomPathChoosingText(string _pathType = "")
    {
        if (textsPathChoosing == null)
        {
            return "no options";
        }
        List<string> options = new List<string>();
        foreach (var item in textsPathChoosing)
        {
            if (item.optionalPathType == "" || item.optionalPathType == "none" || item.optionalPathType == _pathType)
            {
                options.Add(item.text);
            }
        }
        if (options.Count > 0)
        {
            return options[DungeonManager._instance.currentCalcRun.RandomNum(0, options.Count)];
        }
        return "no options";
    }

    public TextPathHandling[] textsPathHandling;
    public string GetRandomPathHandlingText(string _pathType = "")
    {
        if(textsPathHandling == null)
        {
            return "no options";
        }
        List<string> options = new List<string>();
        foreach (var item in textsPathHandling)
        {
            if (item.optionalPathType == "" || item.optionalPathType == "none" || item.optionalPathType == _pathType)
            {
                options.Add(item.text);
            }
        }
        if (options.Count > 0)
        {
            return options[DungeonManager._instance.currentCalcRun.RandomNum(0, options.Count)];
        }
        return "no options";
    }

    public TextEnemyTurnFlavour[] textsEnemyTurn;
    public string GetRandomEnemyTurnText(string _nodeType = "", string _eventType = "")
    {
        if (textsEnemyTurn == null)
        {
            return "no options";
        }
        List<string> options = new List<string>();
        foreach (var item in textsEnemyTurn)
        {
            if (item.optionalNodeType == "" || item.optionalNodeType == "none" || item.optionalNodeType == _nodeType)
            {
                if (item.optionalEventType == "" || item.optionalEventType == "none" || item.optionalEventType == _eventType)
                {
                    options.Add(item.text);
                }
            }
        }
        if (options.Count > 0)
        {
            return options[DungeonManager._instance.currentCalcRun.RandomNum(0, options.Count)];
        }
        return "no options";
    }

    public TextHeroTurn[] textsHeroTurn;
    public string GetRandomHeroTurnText(string _nodeType = "", string _eventType = "")
    {
        if (textsHeroTurn == null)
        {
            return "no options";
        }
        List<string> options = new List<string>();
        foreach (var item in textsHeroTurn)
        {
            if (item.optionalNodeType == "" || item.optionalNodeType == "none" || item.optionalNodeType == _nodeType)
            {
                if (item.optionalEventType == "" || item.optionalEventType == "none" || item.optionalEventType == _eventType)
                {
                    options.Add(item.text);
                }
            }
        }
        if (options.Count > 0)
        {
            return options[DungeonManager._instance.currentCalcRun.RandomNum(0, options.Count)];
        }
        return "no options";
    }
}

[System.Serializable]
public class TextEnemyName
{
    public string name;
    public string optionalNodeType;
}

[System.Serializable]
public class TextPathChoosing
{
    public string text;
    public string optionalPathType;
}

[System.Serializable]
public class TextPathHandling
{
    public string text;
    public string optionalPathType;
}

[System.Serializable]
public class TextEnemyTurnFlavour
{
    public string text;
    public string optionalNodeType;
    public string optionalEventType;
}

[System.Serializable]
public class TextHeroTurn
{
    public string text;
    public string optionalNodeType;
    public string optionalEventType;
}
#endregion