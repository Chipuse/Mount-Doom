using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DexUI : MonoBehaviour
{
    [SerializeField] GameObject dexCardPrefab;

    private List<DexCard> dexCards;

    private void Start()
    {
        
        if(DatabaseManager.CheckDatabaseValid())
        {
            dexCards = new List<DexCard>();

            for (int i = 0; i < DatabaseManager._instance.defaultHeroData.defaultHeroList.Length; i++)
            {
                if (DatabaseManager._instance.activePlayerData.dex.Count > i)
                {
                    dexCards.Add(Instantiate(dexCardPrefab).GetComponent<DexCard>());

                    if (dexCards.Count > i)
                    { 
                        dexCards[i].transform.SetParent(this.transform);
                        dexCards[i].transform.localScale = new Vector3(1, 1, 1);
                        dexCards[i].UpdateDexCard(DatabaseManager._instance.defaultHeroData.defaultHeroList[i]);
                        dexCards[i].UpdateState(DatabaseManager._instance.activePlayerData.dex[i]);
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < dexCards.Count; i++)
        {
            if (DatabaseManager._instance.activePlayerData.dex.Count > i)
                dexCards[i].UpdateState(DatabaseManager._instance.activePlayerData.dex[i]);
        }
    }
}
