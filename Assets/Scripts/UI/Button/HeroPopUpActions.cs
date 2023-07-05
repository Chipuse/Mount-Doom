using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroPopUpActions : MonoBehaviour
{
    [SerializeField] GameObject ReleaseButton;
    [SerializeField] GameObject CloseButton;
    [SerializeField] GameObject ReleaseWarningContinue;
    [SerializeField] GameObject ReleaseWarningLastContinue;
    [SerializeField] GameObject ReleaseSubmit;
    [SerializeField] GameObject ReleaseCancel;
    [SerializeField] GameObject Detail;
    [SerializeField] GameObject Hero;

    [SerializeField] HubButtonActions hub;

    [SerializeField] UpdateHeroCard card;

    [SerializeField] InventoryUI InventoryUI;
    [SerializeField] SwipeInventory Swipe;

    private void OnEnable()
    {
        if (hub.currentHubFocus == HubState.HeroHub && !hub.isRewarding && Swipe.isActiveAndEnabled == false)
            ReleaseButton.SetActive(true);

        else
            ReleaseButton.SetActive(false);   
    }

    private void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(() => { CloseHeroPopUp(); });
        ReleaseButton.GetComponent<Button>().onClick.AddListener(() => { ReleaseHero(); });
        ReleaseSubmit.GetComponent<Button>().onClick.AddListener(() => { OnReleaseSubmit(); });
        ReleaseCancel.GetComponent<Button>().onClick.AddListener(() => { OnReleaseCancel(); });
        ReleaseWarningContinue.GetComponent<Button>().onClick.AddListener(() => { OnReleaseWarningContinue(); });
        ReleaseWarningLastContinue.GetComponent<Button>().onClick.AddListener(() => { OnReleaseLastContinue(); });
        Detail.GetComponent<Button>().onClick.AddListener(() => { PageSound(); });
        Hero.GetComponent<Button>().onClick.AddListener(() => { PageSound(); });
    }

    private void PageSound()
    {
        AudioManager.PlayEffect("pageTurn");
    }

    private void CloseHeroPopUp()
    {
        UIEnablerManager.Instance.DisableElement("HeroCard", true);
    }

    //UI flow release
    private void ReleaseHero()
    {
        if (card.publicHero.status != HeroStatus.Idle)
            UIEnablerManager.Instance.EnableElement("ReleaseWarning", true);

        else
        {
            if(DatabaseManager._instance.activePlayerData.inventory.Count > 1)
            {
                UIEnablerManager.Instance.EnableElement("ReleaseHero", true);
            }

            else
                UIEnablerManager.Instance.EnableElement("ReleaseWarningLast", true);
        }
    }

    private void OnReleaseCancel()
    {
        UIEnablerManager.Instance.DisableElement("ReleaseHero", true);
    }

    private void OnReleaseSubmit()
    {
        DatabaseManager._instance.activePlayerData.ReleaseHero(card.publicHero.uniqueId, false);
        UIEnablerManager.Instance.DisableElement("ReleaseHero", true);
        UIEnablerManager.Instance.DisableElement("HeroCard", false);

        //send the data actually and update UI
        DatabaseManager._instance.SaveGameDataLocally();
        ServerCommunicationManager._instance.DoServerRequest(Request.PushPlayerData);

        InventoryUI.UpdateInventory();
    }
    private void OnReleaseWarningContinue()
    {
        UIEnablerManager.Instance.DisableElement("ReleaseWarning", true);
    }

    private void OnReleaseLastContinue()
    {
        UIEnablerManager.Instance.DisableElement("ReleaseWarningLast", true);
    }
}
