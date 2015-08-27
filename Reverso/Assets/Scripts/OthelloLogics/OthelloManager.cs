using UnityEngine;
using System.Collections;
using System;

public class OthelloManager
{


    private static OthelloManager _instance;
    private bool _speedMode = false;
    private bool _playAgainstComputer = false;
    private bool _useHints = true;
    private Othello.PlayerColor _playerColor;


    
    public enum ComputerLevel
    {
        Easy, Normal, Hard
    }
    public Othello.PlayerColor PlayerColor
    {
        get
        {
            return _playerColor;
        }
    }

    public bool ShowTimers
    {
        get {
            
            return PlayAgainstComputer ? false : true;
        }
    }
    public bool SpeedMode
    {
        get
        {
            return _speedMode;
        }
        set
        {
            _speedMode = value;
        }
    }
    public bool UseHints
    {
        get
        {
            return ReversoPlayerPrefs.IsHintsOn();
        }
        set
        {
            _useHints = value;
        }

    }

    public bool PlayAgainstComputer
    {
        get
        {
            return _playAgainstComputer;
        }

    }

 
    public static OthelloManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new OthelloManager();
            }
            return _instance;
        }
    }
    private bool _playingOnline = false;
    public bool PlayingOnline
    {
        get
        {
            return _playingOnline;
        }
        
    }

    private string _opponentName = String.Empty;
    public string OpponentName
    {
        get
        {
            return _opponentName;
        }
    }

    private OthelloManager()
    {
    }



    private static void StartGame()
    {
        Application.LoadLevel("GameScene");
    }

    public static void StartVersus()
    {
        _instance = new OthelloManager();
        _instance._playingOnline = false;
        _instance._playAgainstComputer = false;
        StartGame();
    }

    public static void StartComputer()
    {
        _instance = new OthelloManager();
        _instance._playingOnline = false;
        _instance._playAgainstComputer = true;
        StartGame();
    }
    public static void StartOnline(bool thisPlayerStarts, string opponentName)
    {

        _instance = new OthelloManager();
        _instance._playingOnline = true;
        _instance._playAgainstComputer = false;
        _instance._opponentName = opponentName;

        if (thisPlayerStarts)
        {
            _instance._playerColor = Othello.PlayerColor.White;
        }
        else
        {
            _instance._playerColor = Othello.PlayerColor.Black;
        }
        StartGame();
    }

    public bool PlayerIsWhite()
    {
        return _instance._playerColor == Othello.PlayerColor.White ? true : false;
    }
    internal void CleanUp()
    {
        _instance = null;
    }

}
