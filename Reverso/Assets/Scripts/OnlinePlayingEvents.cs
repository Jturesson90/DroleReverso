using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class OnlinePlayingEvents : MonoBehaviour
{
    void Awake()
    {
    }
    void Start()
    {
    }
    void Update()
    {
        if (ReversoGooglePlay.Instance == null)
        {
            return;
        }
        switch (ReversoGooglePlay.Instance.State)
        {

            case ReversoGooglePlay.GameState.SetupFailed:
                Debug.Log("RaceManager.Instance.State = " + ReversoGooglePlay.Instance.State);
                // call the local version of the method to handle the gamepad state.
                ShowMainMenu();
                break;
            case ReversoGooglePlay.GameState.Aborted:
                Debug.Log("RaceManager.Instance.State = " + ReversoGooglePlay.Instance.State);
                // call the local version of the method to handle the gamepad state.
                GameObject.Find("Othello").GetComponent<Othello>().OnPlayerLeft();
                if (ReversoGooglePlay.Instance != null)
                {
                    ReversoGooglePlay.Instance.CleanUp();
                }
                break;
            case ReversoGooglePlay.GameState.Finished:
                // ShowResults();
                Debug.Log("RaceManager.Instance.State = " + ReversoGooglePlay.Instance.State);
                break;
            case ReversoGooglePlay.GameState.Playing:
                //if (done)
                //{
                //    ResetUI();
                //}
                HandleGamePlay();
                break;
            default:
                Debug.Log("RaceManager.Instance.State = " + ReversoGooglePlay.Instance.State);
                break;
        }
    }

    public static void OnQuit()
    {
        if (ReversoGooglePlay.Instance != null)
        {
            ReversoGooglePlay.Instance.CleanUp();
        }
        ShowMainMenu();
    }

    private static void ShowMainMenu()
    {
        NavigationUtil.ShowMainMenu();
    }
    private void HandleGamePlay()
    {
    }
}

