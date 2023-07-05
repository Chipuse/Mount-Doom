using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LayoutList", menuName = "ScriptableObjects/LayoutList", order = 1)]
public class LayoutList : ScriptableObject
{
    public Layout[] layouts;
    public Dictionary<string, GameObject> layoutPrefabs;

    public void CreateDictionary()
    {
        layoutPrefabs = new Dictionary<string, GameObject>();
        foreach (Layout layout in layouts)
        {
            if (!layoutPrefabs.ContainsKey(layout.name))
            {
                layoutPrefabs.Add(layout.name, layout.layoutPrefab);
            }
        }
    }
}

[System.Serializable]
public struct Layout
{
    public string name;
    public GameObject layoutPrefab;
}
