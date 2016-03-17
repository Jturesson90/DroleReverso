using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;


public class InvitationEvents : MonoBehaviour {

    public Text message;

    // the invitation object being processed.
    private Invitation inv;

    private bool processed = false;
    private string inviterName = null;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        
        inv = (inv != null) ? inv : InvitationManager.Instance.Invitation;
        if (inv == null && !processed)
        {
          //  Debug.Log("No Invite -- back to main");
            NavigationUtil.ShowMainMenu();
            return;
        }

        if (inviterName == null)
        {
            inviterName = (inv.Inviter == null || inv.Inviter.DisplayName == null) ? "Someone" :
            inv.Inviter.DisplayName;
            message.text = inviterName + " is challenging you to Reverso!";
        }

        if (ReversoGooglePlay.Instance != null) 
        {
            ReversoGooglePlay.Instance.OpponentName = (inv.Inviter == null || inv.Inviter.DisplayName == null) ? "" : inv.Inviter.DisplayName;
            switch (ReversoGooglePlay.Instance.State)
            {
                case ReversoGooglePlay.GameState.Aborted:
                    Debug.Log("Aborted -- back to main");
                  //  NavigationUtil.ShowMainMenu();
                    break;
                case ReversoGooglePlay.GameState.Finished:
                    Debug.Log("Finished-- back to main");
                //    NavigationUtil.ShowMainMenu();
                    break;
                case ReversoGooglePlay.GameState.Playing:
                 //   NavigationUtil.ShowPlayingPanel();
                    break;
                case ReversoGooglePlay.GameState.SettingUp:
              //      message.text = "Setting up Race...";
                    break;
                case ReversoGooglePlay.GameState.SetupFailed:
                    Debug.Log("Failed -- back to main");
               //     NavigationUtil.ShowMainMenu();
                    break;
            }
        }
    }
    public void OnAccept()
    {

        if (processed)
        {
            return;
        }

        processed = true;
        InvitationManager.Instance.Clear();

        ReversoGooglePlay.AcceptInvitation(inv.InvitationId);
        Debug.Log("Accepted! RaceManager state is now " + ReversoGooglePlay.Instance.State);

    }

    public void OnDecline()
    {
        NavigationUtil.ShowMainMenu();
        if (processed)
        {
            return;
        }

        processed = true;
        InvitationManager.Instance.DeclineInvitation();

        
    }

}
