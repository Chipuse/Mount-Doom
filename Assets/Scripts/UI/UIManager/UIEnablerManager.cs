using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnablerManager : MonoBehaviour
{
    public enum ListReference
    {
        Footer,
        Header,
        Centered,
        PopUp
    }


    #region vars
    public static UIEnablerManager Instance;

    [SerializeField] Canvas canvas;          
    [SerializeField] GameObject blur;          
    [SerializeField] float canvasAnimTime;          

    //dictionaries
    Dictionary<string,GameObject> FooterElements = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> HeaderElements = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> CenterElements = new Dictionary<string, GameObject>();
    Dictionary<string, GameObject> PopUpElements = new Dictionary<string, GameObject>();

    float animSpeed = 10;
    #endregion


    //Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }


    //public funcs to be used 
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //init
    public void AssignElementToList(ListReference reference, GameObject UIelement)
    {
        string name = UIelement.name;
        int index = name.LastIndexOf("_"); // Character to remove "_"


        if (index > 0)
            name = name.Substring(0, index);


        switch (reference)
        {
            case ListReference.Header:
                if(!HeaderElements.ContainsKey(name))
                    HeaderElements.Add(name,UIelement);
                break;
            case ListReference.Footer:
                if(!FooterElements.ContainsKey(name))
                    FooterElements.Add(name,UIelement);
                break;
            case ListReference.Centered:
                if(!CenterElements.ContainsKey(name))
                    CenterElements.Add(name,UIelement);
                break;
            case ListReference.PopUp:
                if(!PopUpElements.ContainsKey(name))
                    PopUpElements.Add(name,UIelement);
                break;
            default:
                Debug.Log("trying to assign none existant element");
                break;
        }
    }


    //during runtime to be called
    public void EnableElement(string element, bool DoAnimation)
    {
        bool IsSomewhere = false;//catch bool


        //debug check
        if (FooterElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = FooterElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                anim.ShowObject();
            }

            else
            {
                dictObject.SetActive(true);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }

        }

        if (HeaderElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = HeaderElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                anim.ShowObject();
            }

            else
            {
                dictObject.SetActive(true);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }
        }

        if (CenterElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = CenterElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                anim.ShowObject();
            }

            else
            {
                dictObject.SetActive(true);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }
        }

        if (PopUpElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = PopUpElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                anim.ShowObject();
            }

            else
            {
                dictObject.SetActive(true);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }
        }



        //not found
        if (!IsSomewhere)
            Debug.Log("trying to enable: " + element + ", but this does not exist in any dict as key");
    }

    public float DisableElement(string element, bool DoAnimation)
    {

        float time = 0;
        bool IsSomewhere = false;//catch bool
        

        //differemt enable logics for dictionary type
        //find if in Dictionaries there is something to be enabled
        //only do if not already disabled

        //check for each dictionary
        if (FooterElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = FooterElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                time = anim.HideObject();
            }

            else
            {
                dictObject.SetActive(false);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }

        }

        if (HeaderElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = HeaderElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                time = anim.HideObject();
            }

            else
            {
                dictObject.SetActive(false);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }
        }

        if (CenterElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = CenterElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                time = anim.HideObject();
            }

            else
            {
                dictObject.SetActive(false);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }
        }

        if (PopUpElements.ContainsKey(element))
        {
            //debug check
            IsSomewhere = true;

            GameObject dictObject = PopUpElements[element];

            AbstractElementAnimation anim;
            dictObject.TryGetComponent<AbstractElementAnimation>(out anim);

            if (anim != null && DoAnimation)
            {
                time = anim.HideObject();
            }

            else
            {
                dictObject.SetActive(false);

                if (DoAnimation)
                    Debug.Log("No animation set for center element " + element);
            }
        }
        
        //not found
        if (!IsSomewhere)
            Debug.Log("trying to disable: " + element + ", but this does not exist in any dict as key");

        return time;

    }

    public void SwitchElements(string oldElement,string newElement, bool DoAnimation)
    {
        //mix of enable and disable
        //wait for disable before do enable (at least for certain objects) or do smooth transition
        float waitTime = DisableElement(oldElement, DoAnimation);
        StartCoroutine(Pause(waitTime, DoAnimation, newElement));
    }

    public void EnableCanvas()
    {
        LeanTween.value(canvas.gameObject, 0f, 1f, canvasAnimTime)
            .setOnUpdate((value) =>
            {
                canvas.GetComponent<Image>().color = new Color(canvas.GetComponent<Image>().color.r, canvas.GetComponent<Image>().color.g, canvas.GetComponent<Image>().color.b, value);
            });
    }    
    
    public void DisableCanvas()
    {
        LeanTween.value(canvas.gameObject, 1f, 0f, canvasAnimTime)
            .setOnUpdate((value) =>
            {
                canvas.GetComponent<Image>().color = new Color(canvas.GetComponent<Image>().color.r, canvas.GetComponent<Image>().color.g, canvas.GetComponent<Image>().color.b, value);
            });
    }


    public void EnableBlur()
    {
        blur.SetActive(true);
    }

    public void DisableBlur()
    {
        blur.SetActive(false);
    }


    IEnumerator Pause(float time, bool DoAnimation, string newElement)
    {
        yield return new WaitForSeconds(time);
        EnableElement(newElement, DoAnimation);
    }


}
