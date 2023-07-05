using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheapProgressBar : MonoBehaviour
{
    public RectTransform bar;
    float maxVal;
    float currval;

    float percVal;

    bool percMode;

    public void SetVal(float _newVal, float _maxVal, bool _interpolate = false)
    {
        maxVal = _maxVal;
        currval = _newVal;
        if(maxVal != 0)
        {
            bar.localScale = new Vector3(currval / maxVal, 1, 1);
        }
    }

    public void SetVal(float _newVal, bool _interpolate = false)
    {
        SetVal(_newVal, maxVal);
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
