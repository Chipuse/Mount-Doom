using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoAvailableAction : MonoBehaviour
{
    [SerializeField] GameObject InfoButton;

    private void Start()
    {
        InfoButton.GetComponent<Button>().onClick.AddListener(() => { ShowLink(); });
    }

    private void ShowLink()
    {
        UIEnablerManager.Instance.EnableElement("LinkInfo",true);
    }
}
