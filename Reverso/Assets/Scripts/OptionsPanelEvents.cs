using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Linq;

public class OptionsPanelEvents : MonoBehaviour
{
    public SoundManager soundManager;

    public Toggle HintToggle;
    public Toggle TimerToggle;
    public Toggle AudioToggle;
    public Text SignInText;
    string signIn = "Sign in";
    string signOut = "Sign out";
    public Button AchievementButton;

    void Start()
    {
        AudioToggle.isOn = soundManager.IsOn;
        HintToggle.isOn = ReversoPlayerPrefs.IsHintsOn();
        TimerToggle.isOn = ReversoPlayerPrefs.IsTimerOn();

        //TODO Hold a state and save only when quitting the app

    }

    public void OnAchievementsButton()
    {
        if (AchievementsManager.Instance != null)
        {
            AchievementsManager.Instance.ShowAchievements();
        }
    }

    void LateUpdate()
    {
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

    public void OnAudioTogglePressed()
    {
        soundManager.IsOn = AudioToggle.isOn;
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
        else
        {
            SignInText.text = signIn;
            AchievementButton.interactable = false;
        }
    }


}
