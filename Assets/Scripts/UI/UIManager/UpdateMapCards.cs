using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMapCards : MonoBehaviour
{
    #region vars
    [SerializeField] GameObject mapContainer;

    private Button[] mapButtons;
    #endregion


    private void Start()
    {
        mapButtons = mapContainer.GetComponentsInChildren<Button>();
    }

    //public funcs
    //---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateQuests()//do onenable?
    {
        //get quest name
        //get quest content or description
        //get quest reward
        //assign to each map
    }
}
