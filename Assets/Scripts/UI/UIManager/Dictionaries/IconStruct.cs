using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconStruct : MonoBehaviour
{
    public static IconStruct Instance;


    [System.Serializable]
    [HideInInspector]public struct IconDict
    {
        public string name;

        public Sprite sprite;

        public Color color;
    }

    [SerializeField] List<IconDict> WorkAroundIconList = new List<IconDict>();

    public static Dictionary<string, IconDict > IconDictionary = new Dictionary<string, IconDict>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);



        //init dict
        foreach (IconDict icon in WorkAroundIconList)
        {
            if (IconDictionary.ContainsKey(icon.name))
                IconDictionary[icon.name] = icon;

            else
                IconDictionary.Add(icon.name, icon);
        }
    }
}
