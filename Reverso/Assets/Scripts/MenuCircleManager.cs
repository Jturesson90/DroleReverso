using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuCircleManager : MonoBehaviour
{

    public GameObject circlePrefab;
    public int numberOfCircles = 10;
    public List<MenuCircle> circles = new List<MenuCircle>();

    public float startXPos = 7f;
    public float startYPos = 8.5f;


    // Use this for initialization
    void Start()
    {
        DeleteAllChildren();
        SpawnCircles();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    void SpawnCircles()
    {
        for (int i = 0; i < numberOfCircles; i++)
        {
            GameObject circleGo = Instantiate(circlePrefab) as GameObject;
            circleGo.transform.SetParent(transform, false);

            MenuCircle circle = circleGo.GetComponent<MenuCircle>();
            circles.Add(circle);

            if (i % 2 == 0)
            {
                circle.GiveBlackColor();
            }
            else
            {
                circle.GiveWhiteColor();
            }
        }

        for (int i = 0; i < numberOfCircles; i++)
        {
            bool useX = i > numberOfCircles / 2;
            float randomV = Random.value;

            if (useX)
            {
                //         circles[i].transform.position = new Vector2(randomV > 0.5f ? startXPos : -startXPos, -startYPos + Random.value * startYPos * 2);
            }
            else
            {
                //      circles[i].transform.position = new Vector2(-startXPos + Random.value * startXPos * 2, randomV > 0.5f ? startYPos : -startYPos);
            }

            circles[i].transform.position = new Vector2(-startXPos + Random.value * startXPos * 2, -startYPos + Random.value * startYPos * 2);
        }
    }
    void DeleteAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject, 0f);
        }
    }


}
