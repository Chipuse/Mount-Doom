using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileLogger : MonoBehaviour
{
    public static MobileLogger _instance;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public TMPro.TMP_Text logText;

    public static void LogText(string _text)
    {
        if (_instance == null)
            return;
        _instance.logText.text = _text;
    }
    public static void AppendLogText(string _text)
    {
        if (_instance == null)
            return;
        _instance.logText.text = _text + "\n" + _instance.logText.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
