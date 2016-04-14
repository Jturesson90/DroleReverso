using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenMenuButtons : MonoBehaviour
{

    List<Transform> children = new List<Transform>();
    public LeanTweenType tweenType = LeanTweenType.easeOutExpo;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }


        for (int i = 0; i < children.Count; i++)
        { 
            var rectT = children[i].GetComponent<RectTransform>();
            var startPosX = rectT.localPosition.x;
            rectT.anchoredPosition += Vector2.left * 750f;
            LeanTween.moveLocalX(children[i].gameObject, startPosX, 1f).setEase(tweenType).setDelay(i * 0.15f);
        }



    }

    // Update is called once per frame
    void Update()
    {

    }
}
