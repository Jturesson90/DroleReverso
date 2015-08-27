using UnityEngine;
using UnityEngine.UI;

using System.Collections;

using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class OnlineMenuEvents : MonoBehaviour
{
    private const float ERROR_STATUS_TIMEOUT = 10.0f;
    private const float INFO_STATUS_TIMEOUT = 2.0f;
    private float mStatusCountdown = 0f;

    public GameObject statusText;

    private string mStatusMsg = null;

    private bool processed = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleStatusUpdate();
        if (ReversoGooglePlay.Instance == null)
        {

            return;
        }

        switch (ReversoGooglePlay.Instance.State)
        {
            case ReversoGooglePlay.GameState.SettingUp:
                if (statusText != null)
                {
                    //reset the timer, we can stay here for a long time.
                    mStatusMsg = null;
                    ShowStatus("Waiting for opponent...", false);
                }
                break;
            case ReversoGooglePlay.GameState.SetupFailed:
                ShowStatus("Game setup failed", true);
                ReversoGooglePlay.Instance.CleanUp();
                processed = false;
                break;
            case ReversoGooglePlay.GameState.Aborted:
                ShowStatus("Race Aborted.", true);
                ReversoGooglePlay.Instance.CleanUp();
                processed = false;
                break;
            case ReversoGooglePlay.GameState.Finished:
                // really should not see this on the main menu page,
                // so go to playing panel to display the final outcome of the race.
                //   NavigationUtil.ShowPlayingPanel();
                processed = false;
                break;
            case ReversoGooglePlay.GameState.Playing:
                //  NavigationUtil.ShowPlayingPanel();
                processed = false;
                break;
            default:
                Debug.Log("RaceManager.Instance.State = " + ReversoGooglePlay.Instance.State);
                break;
        }
    }
    void HandleStatusUpdate()
    {
        if (statusText.activeSelf)
        {
            mStatusCountdown -= Time.deltaTime;
            if (mStatusCountdown <= 0)
            {
                statusText.SetActive(false);
            }
        }
    }
    void ShowStatus(string msg, bool error)
    {
        if (msg != mStatusMsg)
        {
            mStatusMsg = msg;
            statusText.SetActive(true);
            Text txt = statusText.GetComponentInChildren<Text>();
            txt.text = msg;
            if (error)
            {
                //     Color c = statusText.GetComponent<Image>().color;
                //    c.a = 1.0f;
                //       statusText.GetComponent<Image>().color = c;
                mStatusCountdown = ERROR_STATUS_TIMEOUT;
            }
            else
            {
                //      Color c = statusText.GetComponent<Image>().color;
                //     c.a = 0.0f;
                //    statusText.GetComponent<Image>().color = c;
                mStatusCountdown = INFO_STATUS_TIMEOUT;
            }
        }
    }

    public void OnQuickMatch()
    {
        if (processed)
        {
            return;
        }
        processed = true;
        ReversoGooglePlay.CreateQuickGame();
    }

    //Handler for the send initation button.
    public void OnInvite()
    {
        ReversoGooglePlay.CreateWithInvitationScreen();
    }

    //Handler for the inbox button.
    public void OnInboxClicked()
    {
        if (processed)
        {
            return;
        }
        processed = true;
        ReversoGooglePlay.AcceptFromInbox();
    }

    //Handler for the signout button.
    public void OnCancelClicked()
    {
        NavigationUtil.ShowMainMenu();
    }
}
