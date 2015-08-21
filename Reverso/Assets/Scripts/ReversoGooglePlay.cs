using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Collections.Generic;

public class ReversoGooglePlay : RealTimeMultiplayerListener
{

    const int MinOpponents = 1, MaxOpponents = 1, QuickGameOpponents = 1;
    const int GameVariant = 0;
    public enum GameState { SettingUp, Playing, Finished, SetupFailed, Aborted };
    private GameState _gameState = GameState.SettingUp;
    private static ReversoGooglePlay _instance;
    private string mMyParticipantId = "";
    private string _opponentName;
    float _roomSetupStartTime = 0.0f;

    private ReversoGooglePlay()
    {
        _roomSetupStartTime = Time.time;
    }
    public string OpponentName
    {
        get
        {
            return _opponentName;
        }
        set
        {
            _opponentName = value;
        }
    }
    public GameState State
    {
        get
        {
            return _gameState;
        }
    }
    public static ReversoGooglePlay Instance
    {
        get
        {
            return _instance;
        }
    }



    public bool IsLoggedIn()
    {
        return Social.localUser.authenticated;
    }


 

    private bool showingWaitingRoom = false;
    public void OnRoomSetupProgress(float percent)
    {
        if (!showingWaitingRoom)
        {
            showingWaitingRoom = true;
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            _gameState = GameState.Playing;
            mMyParticipantId = GetSelf().ParticipantId;
            // SetupTrack();
            //TODO Maybe startScene
            SetupOnlineGame();
        }
        else
        {
            _gameState = GameState.SetupFailed;
        }
    }

    private void SetupOnlineGame()
    {

        Participant self = GetSelf();

        List<Participant> participants = GetParticipants();

        bool thisPlayerStarts = participants.IndexOf(self) == 0 ? true : false;
        string opponentName = GetOpponent().DisplayName;
        OthelloManager.StartOnline(thisPlayerStarts, opponentName);

    }

    public static void AcceptInvitation(string invitationId)
    {
        _instance = new ReversoGooglePlay();
        PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, _instance);
    }

    public void OnLeftRoom()
    {
        if (_gameState != GameState.Finished)
        {
            _gameState = GameState.Aborted;
        }
    }

    public void OnParticipantLeft(Participant participant)
    {
        DebugOSD.Log("OnPeersDisconnected");
        if (_gameState == GameState.Playing)
        {
            _gameState = GameState.Aborted;
        }
    }

    public void OnPeersConnected(string[] participantIds)
    {
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        DebugOSD.Log("OnPeersDisconnected");
        if (_gameState == GameState.Playing)
        {
            _gameState = GameState.Aborted;
        }
    }

    public static void CreateQuickGame()
    {
        _instance = new ReversoGooglePlay();
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(QuickGameOpponents, QuickGameOpponents,
                GameVariant, _instance);
    }

    public static void CreateWithInvitationScreen()
    {
        _instance = new ReversoGooglePlay();
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents,
                GameVariant, _instance);

    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        GameObject.Find("Othello").GetComponent<Othello>().OnOnlineRecievedData(data);
    }

    public static void AcceptFromInbox()
    {
        _instance = new ReversoGooglePlay();
        PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(_instance);
    }

    internal void CleanUp()
    {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        //TODO Maybe back to Main menu here
        _gameState = GameState.Aborted;
        _instance = null;

    }

    private Participant GetSelf()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf();

    }

    private List<Participant> GetParticipants()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();

    }

    private Participant GetParticipant(string participantId)
    {
        return PlayGamesPlatform.Instance.RealTime.GetParticipant(participantId);
    }
    private Participant GetOpponent()
    {
        List<Participant> participants = GetParticipants();
        Participant opponent = null;
        foreach (Participant participant in participants)
        {
            if (participant.Equals(GetSelf()))
            {
            }
            else
            {
                opponent = participant;
            }
        }


        return opponent;
    }
    byte[] _othelloPacket;

    public void BroadcastMyTurn(OthelloPiece[,] bricks, Othello.PlayerColor currentPlayerColor)
    {
        _othelloPacket = bricks.ToByteArray();
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, _othelloPacket);
    }
    public void OnGameOver()
    {
        _gameState = GameState.Finished;
        
    }
    
}
