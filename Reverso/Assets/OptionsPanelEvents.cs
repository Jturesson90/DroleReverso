using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class OptionsPanelEvents : MonoBehaviour
{
    public Toggle HintToggle;
    public Toggle TimerToggle;
    public Text SignInText;
    string signIn = "Sign in";
    string signOut = "Sign out";
    public Button AchievementButton;


    void Start()
    {
        HintToggle.isOn = ReversoPlayerPrefs.IsHintsOn();
        TimerToggle.isOn = ReversoPlayerPrefs.IsTimerOn();
    }
    public void OnAchievementsButton()
    {
        if (AchievementsManager.Instance != null)
        {
            AchievementsManager.Instance.ShowAchievements();
        }
    }
    void FixedUpdate() {
        CheckAuthentication();
      
    }

    

    public void OnGooglePlayPressed()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            PlayGamesPlatform.Instance.SignOut();
        }
        else
        {
            GameObject.Find("MenuCanvas").GetComponent<MainMenuEvents>().ConfigPlayGames();
        }
    }
    public void OnTimerPressed()
    {
        ReversoPlayerPrefs.SetTimer(TimerToggle.isOn);
    }
    public void OnTogglePressed()
    {
        ReversoPlayerPrefs.SetHints(HintToggle.isOn);
    }
    void CheckAuthentication()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            SignInText.text = signOut;
            AchievementButton.interactable = true;
        }
        else {
            SignInText.text = signIn;
            AchievementButton.interactable = false;
        }
    }
}
