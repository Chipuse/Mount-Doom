using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAssignment : MonoBehaviour
{
    [SerializeField] UIEnablerManager.ListReference UIreference;

    //private GameObject[] children;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform element in transform)
        {
            UIEnablerManager.Instance.AssignElementToList(UIreference, element.gameObject);
        }
    }

}
