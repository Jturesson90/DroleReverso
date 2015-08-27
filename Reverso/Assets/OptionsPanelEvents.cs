using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class OptionsPanelEvents : MonoBehaviour
{
    public Toggle hintToggle;
    public Text signInText;
    string signIn = "Sign in";
    string signOut = "Sign out";
    public Button AchievementButton;

    void Start()
    {
        hintToggle.isOn = ReversoPlayerPrefs.IsHintsOn();
      
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

    public void OnTogglePressed()
    {
        ReversoPlayerPrefs.SetHints(hintToggle.isOn);
    }
    void CheckAuthentication()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            signInText.text = signOut;
            AchievementButton.interactable = true;
        }
        else {
            signInText.text = signIn;
            AchievementButton.interactable = false;
        }
    }
}
