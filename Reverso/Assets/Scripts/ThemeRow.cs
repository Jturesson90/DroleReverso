using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[System.Serializable]
public class ThemeRow : MonoBehaviour
{
    public const string RADIO_BUTTON = "RADIO_BUTTON";
    const string REWARD_AD = "rewardedVideo";
    public Image _image;
    public Text _description;
    public Button _adButton;
    public Toggle _radioButton;

    public int ID;

    public Color NormalColor;

    [Header("Radio Button Back Circle")]
    public Image backCircle;
    private Color _backCircleColorOn;
    private Color _backCircleColorOff;
    [Range(0, 1)]
    public float backCircleGradient;
    public Image radioBackgroundImage;
    bool _started = false;

    public void SetAdButtonActive(bool active)
    {
        if (active)
        {
            _adButton.gameObject.SetActive(true);
            _radioButton.interactable = false;
            // _radioButton.gameObject.SetActive(false);
        }
        else
        {
            _adButton.gameObject.SetActive(false);
            _radioButton.interactable = true;
            //_radioButton.gameObject.SetActive(true);

        }
    }

    public void OnAdButtonPressed()
    {
        _adButton.interactable = false;
        ShowRewardedAd();
    }
    // Use this for initialization
    public void OnStart()
    {
        _backCircleColorOn = new Color(NormalColor.r, NormalColor.g, NormalColor.b, backCircleGradient);
        _backCircleColorOff = new Color(255f, 255f, 255f, backCircleGradient);
        if (!_image && !_description && !_adButton && !_radioButton) return;
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
        _started = true;
        SetColorToCircle();
    }


    public void OnRadioButtonValueChanged(bool val)
    {
        if (!_started) return;
        if (_radioButton.isOn)
        {
            OthelloManager.ChosenBoard = ID;
        }
        else
        {
            var selectable = _radioButton.colors;
        }
        SetColorToCircle();
        SetRadioButtonNormalColor();
    }
    private void SetColorToCircle()
    {
        if (_radioButton.isOn)
        {
            backCircle.color = _backCircleColorOn;

        }
        else
        {

            backCircle.color = _backCircleColorOff;
        }

    }
    private void SetRadioButtonNormalColor()
    {
        if (_radioButton.isOn)
        {
            radioBackgroundImage.color = NormalColor;
        }
        else
        {
            Color whiteColorWithAlpha = Color.white;
            whiteColorWithAlpha.a *= 0.7f;
            radioBackgroundImage.color = whiteColorWithAlpha;
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
        else
        {
            _adButton.interactable = true;
        }

    }

    private void HandleShowResult(ShowResult result)
    {
        _adButton.interactable = true;
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
