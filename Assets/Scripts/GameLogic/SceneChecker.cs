using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(DungeonManager._instance == null || DatabaseManager._instance == null || !DatabaseManager.CheckDatabaseValid())
        {
            SceneManager.LoadScene(0);
        }
    }
}
