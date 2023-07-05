using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComVisSound : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.PlayEffect("startLoad");

    }

    private void OnDisable()
    {
        AudioManager.PlayEffect("endLoad");
    }
}
