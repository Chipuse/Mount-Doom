using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonHeroActions : MonoBehaviour
{
    #region vars
    [SerializeField] ExploreInventoryUI exploreInventory;

    [SerializeField] GameObject cancelButton;
    [SerializeField] GameObject confirmButton;

    [SerializeField] HubButtonActions HubFooter;

    #endregion

    private void Start()
    {
        cancelButton.GetComponent<Button>().onClick.AddListener(() => { ClickedCancel(); });
        confirmButton.GetComponent<Button>().onClick.AddListener(() => { ClickedConfirm(); });
    }

    private void ClickedCancel()
    {
        //go to hero selection
        //save the selected map in data
        UIEnablerManager.Instance.SwitchElements( "DungeonHeroSelect", "DungeonDetailSelect", true);

        UIEnablerManager.Instance.DisableElement("HeroHub", true);
        UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);

        //disable background
        UIEnablerManager.Instance.DisableCanvas();

        //do actual logic
        exploreInventory.RemoveAllHeroesFromExplore();
    }

    private void ClickedConfirm()
    {
        //do actual logic in inventory
        bool confirmed = exploreInventory.ConfirmAllHeroesForExplore();

        if(confirmed)
        {
            //change buttons of general
            //
            HubFooter.UpdateHubButton(ButtonState.Unfocused);
            HubFooter.UpdateTradeButton(ButtonState.Unfocused);
            HubFooter.UpdateDungeonButton(ButtonState.Focused);

            //disable background

            //go to observation
            UIEnablerManager.Instance.SwitchElements("DungeonHeroSelect", "General", true);
            UIEnablerManager.Instance.DisableElement("ShardAndBuff", true);
            UIEnablerManager.Instance.DisableElement("HeroHub", true);
            UIEnablerManager.Instance.EnableElement("DungeonObserve", true);

            UIEnablerManager.Instance.DisableCanvas();


            HubFooter.UpdateHubState(HubState.DungeonHub);

            AudioManager.PlayEffect("click");

        }

        else
            AudioManager.PlayEffect("blocked");


    }
}
