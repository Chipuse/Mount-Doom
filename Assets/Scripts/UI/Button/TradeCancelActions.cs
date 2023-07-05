using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeCancelActions : MonoBehaviour
{
    [SerializeField] Button yes;
    [SerializeField] Button no;

    [SerializeField] TradeInventoryUI tradeInventoryUI;
    [SerializeField] InventoryUI inventory;
    [SerializeField] HubButtonActions hub;


    private void Start()
    {
        yes.GetComponent<Button>().onClick.AddListener(() => { ClickedYes(); });
        no.GetComponent<Button>().onClick.AddListener(() => { ClickedNo(); });
    }


    private void ClickedYes()
    {
        //do cancel

        tradeInventoryUI.RemoveAllHeroesFromTrade();
        inventory.UpdateInventory();

        UIEnablerManager.Instance.EnableElement("WaitingForTrade", true);
        TradeManager._instance.CancelOwnTrades(OnClickedYesServerCallback);
    }

    private void OnClickedYesServerCallback()
    {
        //update everything in hub
        DeleventSystem.TradeCancel?.Invoke();

        UIEnablerManager.Instance.DisableElement("TradeCancel", true);
        UIEnablerManager.Instance.DisableElement("TradeObserve", false);
        UIEnablerManager.Instance.DisableElement("TradeSwipe", true);
        UIEnablerManager.Instance.EnableElement("HeroHub", true);
        UIEnablerManager.Instance.EnableElement("ShardAndBuff", true);
        UIEnablerManager.Instance.DisableElement("WaitingForTrade", true);

        hub.currentHubFocus = HubState.HeroHub;
    }


    private void ClickedNo()
    {
        UIEnablerManager.Instance.DisableElement("TradeCancel", true);
    }
}
