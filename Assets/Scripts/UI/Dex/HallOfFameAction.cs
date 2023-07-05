using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallOfFameAction : MonoBehaviour
{
    [SerializeField] GameObject CloseButton;


    private void Start()
    {
        CloseButton.GetComponent<Button>().onClick.AddListener(() => { CloseHallOfFame(); });
    }

    private void CloseHallOfFame()
    {
        UIEnablerManager.Instance.DisableElement("HallOfFame", true);
    }
}
