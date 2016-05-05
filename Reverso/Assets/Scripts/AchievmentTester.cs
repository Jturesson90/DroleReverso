using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

public class AchievmentTester : MonoBehaviour
{

    private System.Action<bool> _authCallback;
    private bool _signingIn = false;


    void Awake()
    {




    }
    void Start()
    {

        _authCallback = (bool success) =>
        {

            Debug.Log("In Auth callback, success = " + success);

            _signingIn = false;
            if (success)
            {
                Debug.Log("Auth SUCCESS!!");
                Camera.main.backgroundColor = Color.blue;
            }
            else
            {
                Debug.Log("Auth failed!!");
                Camera.main.backgroundColor = Color.red;
            }
        };
        ConfigPlayGames();

    }
    void Update()
    {




    }

    public void OnTestInvitationPanel()
    {
        NavigationUtil.ShowInvitationPanel();
    }
    void ConfigPlayGames()
    {
        var config = new PlayGamesClientConfiguration.Builder()
           .WithInvitationDelegate(InvitationManager.Instance.OnInvitationReceived)
           .Build();
        PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.DebugLogEnabled = true;
        Authorize(false);

    }
    void Authorize(bool silent)
    {
        if (!_signingIn && !PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Debug.Log("Starting sign-in...");
            _signingIn = true;
            PlayGamesPlatform.Instance.Authenticate(_authCallback, silent);

        }

        else
        {
            Debug.Log("Already started signing in");
        }

    }

    public void OnOnlineWinPressed()
    {
        print("WTF");
        AchievementsManager.Instance.OnlineWin();
    }
    public void OnEarlyWinPressed()
    {
        print("WTF");
        AchievementsManager.Instance.EarlyWin();
    }
    public void OnComputerWinPressed()
    {
        print("WTF");
        AchievementsManager.Instance.WonAgainstTheComputer();
    }
    public void OnLocalWinPressed()
    {
        print("WTF");
        AchievementsManager.Instance.LocalGameEnded();
    }
    public void OnOpenAchievementPressed()
    {
        print("WTF");
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }













}
