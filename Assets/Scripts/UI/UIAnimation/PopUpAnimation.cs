using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpAnimation : AbstractElementAnimation
{
    #region vars
    [SerializeField] Image blur;
    [SerializeField] GameObject Buttons;
    [SerializeField] GameObject PopUp;
    [SerializeField] float animSpeed;
    [SerializeField] LeanTweenType animationType;

    float original;
    #endregion

    public override float HideObject()
    {
        //StopCoroutine(Pause());

        //bg
        gameObject.SetActive(true);

        AudioManager.PlayEffect("closePopUp");


        LeanTween.value(blur.gameObject, 1f, 0f, animSpeed)
         .setOnUpdate((value) =>
         {
             blur.color = new Color(blur.color.r, blur.color.g, blur.color.b, value);
         });



        LeanTween.value(PopUp, 1f, 0f, animSpeed)
        .setEaseOutElastic()
        .setOnUpdate((value) =>
        {
            PopUp.transform.localScale = new Vector3 (value,value,value);
        });


        original = Buttons.GetComponent<RectTransform>().rect.y;
        float removed = -Buttons.GetComponent<RectTransform>().rect.height;


        LeanTween.moveY(Buttons, removed, animSpeed).setEase(animationType);




        StartCoroutine(Pause());

        return animSpeed;
    }

    public override void ShowObject()
    {

        StopCoroutine(Pause());

        AudioManager.PlayEffect("openPopUp");

        //bg
        gameObject.SetActive(true);

        LeanTween.value(blur.gameObject, 0f, 1f, animSpeed)
             .setOnUpdate((value) =>
             {
                 blur.color = new Color(blur.color.r, blur.color.g, blur.color.b, value);
             });

        LeanTween.value(PopUp, 0f, 1f, animSpeed)
        .setEaseOutElastic()
        .setOnUpdate((value) =>
        {
            PopUp.transform.localScale = new Vector3(value, value, value);
        });


        original = Buttons.GetComponent<RectTransform>().rect.y;

        Buttons.transform.position = new Vector3(gameObject.transform.position.x, -gameObject.GetComponent<RectTransform>().rect.height, gameObject.transform.position.z);
        LeanTween.moveY(Buttons, original, animSpeed).setEase(animationType);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(animSpeed);

        gameObject.SetActive(false);
        Buttons.transform.position = new Vector3(Buttons.transform.position.x, original, Buttons.transform.position.z);

    }
}
