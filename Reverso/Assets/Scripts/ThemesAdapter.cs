using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ThemesAdapter : MonoBehaviour
{
    public GameObject ThemeRowPrefab;
    public List<ThemeRowEntity> themeRows;
    private RectTransform _rectTransform;
    private ToggleGroup _toggleGroup;
    private float _prefabHeight;
    private static bool DEBUG = false;
    // Use this for initialization
    void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        _prefabHeight = 250f;
        if (DEBUG && !OthelloManager.HasStartedGame)
        {
            ResetAllToggles();
            ReversoPlayerPrefs.SetChosenBoard(0);
        }
        DeleteAllChildren();
        PopulateChildren();
        SetPanelHeight();
    }

    private void SetPanelHeight()
    {
        _rectTransform.SetHeight(_prefabHeight * themeRows.Count);
    }
    private void PopulateChildren()
    {
        int chosenBoard = OthelloManager.ChosenBoard;
        var tempThemeRow = new List<ThemeRow>();
        for (int i = 0; i < themeRows.Count; i++)
        {
            GameObject themeRowGo = GameObject.Instantiate(ThemeRowPrefab) as GameObject;
            RectTransform themeRectT = themeRowGo.GetComponent<RectTransform>();
            themeRectT.SetParent(_rectTransform, false);
            themeRectT.SetLeftTopPosition(new Vector2(themeRectT.rect.x, -i * _prefabHeight));

            ThemeRow themeRow = themeRowGo.GetComponent<ThemeRow>();
            themeRow._description.text = themeRows[i].Description;
            themeRow._image.sprite = themeRows[i].ImageSprite;
            themeRow._image.color = themeRows[i].color;
            themeRow._radioButton.group = _toggleGroup;
            _toggleGroup.RegisterToggle(themeRow._radioButton);

            themeRow.ID = i;
            tempThemeRow.Add(themeRow);
            themeRow.OnStart();


        }
        foreach (var item in tempThemeRow)
        {
            if (chosenBoard == item.ID)
            {
                item._radioButton.isOn = true;

            }
            else
            {
                item._radioButton.isOn = false;
            }
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
    // Update is called once per frame
    void ResetAllToggles()
    {

        for (int ID = 1; ID < 4; ID++)
        {
            PlayerPrefs.SetInt("RADIO_BUTTON" + ID, 1);
        }
    }
}
