using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;

// Utility class to navigate between the various panels in the main scene.
public static class NavigationUtil
{

    private static PanelMgr theMgr;

    public static PanelMgr PanelMgr
    {
        get
        {
            if (theMgr == null)
            {
                theMgr = EventSystem.current.GetComponent<PanelMgr>();
            }
            return theMgr;
        }
    }
    public static void ShowThemesPanel()
    {
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            Debug.Log("Showing ThemesPanel!");
            mgr.OpenThemesPanel();

        }
        else
        {
            Debug.LogWarning("PanelMgr script missing!");
        }
    }
    public static void ShowMainMenu()
    {
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            Debug.Log("Showing MainMenu!");
            mgr.OpenMainMenuPanel();

        }
        else
        {
            Debug.LogWarning("PanelMgr script missing!");
        }
    }
    public static void OnBackbuttonPressedInMenu()
    {
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            mgr.OnBackbuttonPressenInMenu();
        }
    }
    public static void ShowPlayingPanel()
    {
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            Debug.Log("Showing Playing Panel!");
            mgr.OpenThemesPanel();
        }
        else
        {
            Debug.Log("PanelMgr script missing!");
        }
    }

    public static void ShowOptionsPanel()
    {
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            Debug.Log("Showing Options Panel!");
            mgr.OpenOptionsPanel();
        }
        else
        {
            Debug.Log("PanelMgr script Missing");
        }
    }
    public static void ShowInvitationPanel()
    {
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            Debug.Log("Showing Invitation Panel!");
            mgr.OpenInvitationPanel();
        }
        else
        {
            Debug.Log("PanelMgr script Missing");
        }
    }
    public static void ShowOnlineMenuPanel()
    {
        Debug.Log("ShowOnlineMenuPanel");
        PanelMgr mgr = NavigationUtil.PanelMgr;
        if (mgr != null)
        {
            Debug.Log("Showing OnlineMenu Panel!");
            mgr.OpenOnlineMenu();
        }
        else
        {
            Debug.Log("PanelMgr script Missing");
        }
    }
}
