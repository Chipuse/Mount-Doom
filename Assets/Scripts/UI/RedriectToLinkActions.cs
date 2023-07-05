using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RedriectToLinkActions : MonoBehaviour
{
    [SerializeField] GameObject LinkButton;
    //[SerializeField] GameObject InfoPopUp;
    [SerializeField] GameObject LaterButton;
    [SerializeField] GameObject ThanksButton;
    [SerializeField] TextMeshProUGUI DisplayTextObject;


    [SerializeField] public static string URL;
    [SerializeField] public static string displayText = "inactive";
    [SerializeField] public static string formName;

    private void Start()
    {
        LinkButton.GetComponent<Button>().onClick.AddListener(() => { OpenLink(); });
        LaterButton.GetComponent<Button>().onClick.AddListener(() => { Close(); });
        ThanksButton.GetComponent<Button>().onClick.AddListener(() => { Close(); });
        displayText = "inactive";
    }

    private void OnEnable()
    {
        DisplayTextObject.text = displayText;


        ThanksButton.SetActive(false);
        LaterButton.SetActive(true);
    }

    private void OpenLink()
    {
        Application.OpenURL(URL);
        DatabaseManager._instance.activePlayerData.answeredForms.Add(formName);
        UIEnablerManager.Instance.DisableElement("InfoAvailable",false);
        LaterButton.SetActive(false);
        ThanksButton.SetActive(true);
    }

    private void Close()
    {
        UIEnablerManager.Instance.DisableElement("LinkInfo", true);
        displayText = "inactive";
        FormsManager._instance.TryDisplayForm();
    }

}
