using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FooterAnimation : AbstractElementAnimation
{
    [SerializeField] float animSpeed = 2;
    [SerializeField] LeanTweenType animationType;

    private float original;

    public override float HideObject()
    {
        //StopCoroutine(Pause());

        gameObject.SetActive(true);


        original = gameObject.GetComponent<RectTransform>().rect.y;
        float removed = -gameObject.GetComponent<RectTransform>().rect.height;


        LeanTween.moveY(gameObject, removed, animSpeed).setEase(animationType);

        StartCoroutine(Pause());

        return animSpeed;
    }

    public override void ShowObject()
    {
        StopCoroutine(Pause());


        float original = gameObject.GetComponent<RectTransform>().rect.y;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, - gameObject.GetComponent<RectTransform>().rect.height, gameObject.transform.position.z);
        gameObject.SetActive(true);

        LeanTween.moveY(gameObject, original, animSpeed).setEase(animationType);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(animSpeed);


        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, original, gameObject.transform.position.z);
    }
}
