using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeTester : MonoBehaviour
{
    public int[] invIndexToAdd = new int[] { 0, 1, 2 };
    public int[] ownOfferId = new int[] {0,1,2,3 };
    public int interestId;
    public int[] idToDelete = new int[] { 0, 1 }; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //List<TradeOffer> temp = TradeManager._instance.GetSwipeBatch();
            //var matches = TradeManager._instance.GetTradingResults();
        }

        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            List<PlayerHero> list = new List<PlayerHero>();
            foreach (var item in invIndexToAdd)
            {
                if(item < DatabaseManager._instance.activePlayerData.inventory.Count)
                {
                    list.Add(DatabaseManager._instance.activePlayerData.inventory[item]);
                }
            }
            TradeManager._instance.UploadOffer(list.ToArray());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TradeManager._instance.UpdateOffer(interestId, ownOfferId);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            TradeManager._instance.PullTradeOffers();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            List<TradeOffer> list = new List<TradeOffer>();
            foreach (var item in idToDelete)
            {
                foreach (var offer in DatabaseManager._instance.tradeData.tradeOffers)
                {
                    if(offer.offerId == item)
                    {
                        list.Add(offer);
                        break;
                    }
                }
            }
            if(list.Count > 0)
            {
                TradeManager._instance.DeleteOffers(list.ToArray());
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TradeManager._instance.ApplySuccessfulTrades();
        }
        */
    }
}
