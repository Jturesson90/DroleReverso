using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class ThemeRow : MonoBehaviour
{
    public const string RADIO_BUTTON = "RADIO_BUTTON";
    const string REWARD_AD = "REWARD_AD";
    public Image _image;
    public Text _description;
    public GameObject _adButton;
    public Toggle _radioButton;
    public ToggleGroup MyToggleGroup;

    public string Description = string.Empty;
    public int ID;
    public Sprite ImageSprite;
    public int chosenRadioButton = 0;
    

    public void SetAdButtonActive(bool active)
    {
        if (active)
        {
            _adButton.SetActive(true);

            _radioButton.gameObject.SetActive(false);
        }
        else
        {
            _adButton.SetActive(false);
            _radioButton.gameObject.SetActive(true);
            _radioButton.isOn = false;
            if (!_radioButton.group.AnyTogglesOn() && ID == 0)
            {
                _radioButton.isOn = true;
            }
        }
    }

    public void OnAdButtonPressed()
    {
        ShowRewardedAd();
        //  SetAdButtonActive(false);
        // SetShowAdButtonPlayerPref(false);
    }
    // Use this for initialization
    void Start()
    {

        if (!_image && !_description && !_adButton && !_radioButton && !MyToggleGroup) return;
        _radioButton.group = MyToggleGroup;
        if (ID == 0)
            SetAdButtonActive(false);
        else
        {
            if (GetShowAdButtonPlayerPref())
            {
                SetAdButtonActive(true);
            }
            else
            {
                SetAdButtonActive(false);
            }
        }
        //   _image.sprite = ImageSprite;
        _description.text = Description;
        


        if (ID == OthelloManager.ChosenBoard)
        {
            _radioButton.isOn = true;
        }
        else
        {
            _radioButton.isOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnRadioButtonValueChanged(bool val)
    {
        if (_radioButton.isOn)
        {
            print(ID + " is On!");
            OthelloManager.ChosenBoard = ID;
        }

    }

    public void SetShowAdButtonPlayerPref(bool val)
    {
        //Visa Annonser sätt till 1
        if (val)
        {
            PlayerPrefs.SetInt("RADIO_BUTTON" + ID, 1);
        }
        //Visa INTE Annonser sätt till 0
        else
        {
            PlayerPrefs.SetInt("RADIO_BUTTON" + ID, 0);
        }

    }
    public bool GetShowAdButtonPlayerPref()
    {
        //Är Visa Annons 1 så visa annons
        return PlayerPrefs.GetInt("RADIO_BUTTON" + ID, 1) == 1 ? true : false;
    }
    public void ShowRewardedAd()
    {
        print("Show ads " + Advertisement.IsReady());
        if (Advertisement.IsReady(REWARD_AD))
        {

            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(REWARD_AD, options);
        }
#if UNITY_EDITOR
        HandleShowResult(ShowResult.Finished);
#endif
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                SetShowAdButtonPlayerPref(false);
                SetAdButtonActive(false);
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
