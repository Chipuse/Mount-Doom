using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenteredAnimation : AbstractElementAnimation
{
    [SerializeField] float animSpeed = 2;

    public override float HideObject()
    {
        //StopCoroutine(Pause());


        if (gameObject.activeSelf == false)
            gameObject.SetActive(true);

        gameObject.transform.localScale = new Vector3(1, 1, 1);
        LeanTween.scaleX(gameObject, 0, animSpeed);

        StartCoroutine(Pause());

        return animSpeed;
    }

    public override void ShowObject()
    {
        StopCoroutine(Pause());


        gameObject.SetActive(true);

        gameObject.transform.localScale = new Vector3(0, 1, 1);

        LeanTween.scaleX(gameObject, 1, animSpeed);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(animSpeed);


        gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
