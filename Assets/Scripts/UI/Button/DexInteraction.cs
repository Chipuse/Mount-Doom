using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DexInteraction : MonoBehaviour
{
    [SerializeField] GameObject CloseButton;


    private void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(() => { CloseDex(); });
    }

    private void CloseDex()
    {
        UIEnablerManager.Instance.DisableElement("Dex",true);
    }

}
