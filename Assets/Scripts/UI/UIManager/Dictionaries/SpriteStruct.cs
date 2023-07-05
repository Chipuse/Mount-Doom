using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteStruct : MonoBehaviour
{
    [System.Serializable]
    struct SpriteDict 
    {
        public string name;

        public Sprite sprite;
    }

    [SerializeField] List<SpriteDict> WorkAroundList = new List<SpriteDict>();

    public static Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();

    private void Awake()
    {
        //init dict
        foreach (SpriteDict sprite in WorkAroundList)
        {
            if (SpriteDictionary.ContainsKey(sprite.name))
                SpriteDictionary[sprite.name] = sprite.sprite;

            else
                SpriteDictionary.Add(sprite.name, sprite.sprite);
        }
    }
}
    
