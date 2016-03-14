using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ThemesAdapter : MonoBehaviour
{
    public GameObject ThemeRowPrefab;
    public List<ThemeRow> themeRows;

    // Use this for initialization
    void Start()
    {
       // ResetAllToggles();
        //PlayerPrefs.SetInt("RADIO_BUTTON0", 0);
        //DeleteAllChildren();
    }
    void DeleteAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject, 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void ResetAllToggles()
    {

        for (int ID = 1; ID < 4; ID++)
        {
            PlayerPrefs.SetInt("RADIO_BUTTON" + ID, 1);
        }
    }
}
