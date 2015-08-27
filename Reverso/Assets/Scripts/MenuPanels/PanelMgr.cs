using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class PanelMgr : MonoBehaviour
{

    private const string TRIGGER_SWIPE_OUT = "SwipeOut";
    private const string TRIGGER_SWIPE_IN = "SwipeIn";


    public GameObject optionsPanel;
    public GameObject mainMenu;
    public GameObject onlineMenu;
    public GameObject playingPanel;
    public GameObject invitationPanel;

    private GameObject currentPanel;
    private GameObject prevSelected;


    void Start()
    {
        if (Application.loadedLevelName.Equals("GameMenu"))
        {

            OpenMainMenuPanel();
        }
    }
    public void OpenMainMenuPanel()
    {

        if (!Application.loadedLevelName.Equals("GameScene"))
        {
            OpenPanel(mainMenu);

        }
        else
        {
            Application.LoadLevel("GameMenu");
        }

    }


    public void OpenPlayingPanel()
    {
        OpenPanel(playingPanel);

    }

    internal void OnBackbuttonPressenInMenu()
    {
        if (currentPanel == mainMenu)
        {
            if (ReversoGooglePlay.Instance != null)
            {
                ReversoGooglePlay.Instance.CleanUp();
            }
            Application.Quit();
            return;
        }
        else
        {
            OpenPanel(mainMenu);
        }


    }
    public void OpenOptionsPanel()
    {
        if (currentPanel == optionsPanel)
        {
            OpenPanel(mainMenu);
        }
        else
        {
            OpenPanel(optionsPanel);
        }
    }
    public void OpenOnlineMenu()
    {
        OpenPanel(onlineMenu);
    }

    public void OpenInvitationPanel()
    {
        OpenPanel(invitationPanel);
    }

    void OpenPanel(GameObject panel)
    {
        if (currentPanel == panel)
        {
            return;
        }

        if (currentPanel != null && currentPanel != mainMenu)
        {
            ClosePanel(currentPanel);
        }
        panel.gameObject.SetActive(true);
        currentPanel = panel;


        if (panel != mainMenu)
        {
            panel.GetComponent<Animator>().SetTrigger(TRIGGER_SWIPE_IN);
        }
    }


    void ClosePanel(GameObject panel)
    {
        print("Closing " + panel.name);
        if (currentPanel == mainMenu)
        {
            return;
        }

        panel.GetComponent<Animator>().SetTrigger(TRIGGER_SWIPE_OUT);
        prevSelected = panel;
    }

}