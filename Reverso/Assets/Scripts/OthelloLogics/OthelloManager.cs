using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class OthelloManager
{


    private static OthelloManager _instance;
    private bool _speedMode = false;
    private bool _playAgainstComputer = false;
    private bool _useHints = true;
    private Othello.PlayerColor _playerColor;
    private ComputerLevelEnum? _computerLevel;
    private bool _computerIsWaitingForRespond;
    public bool ComputerIsWaitingForRespond
    {
        get
        {
            return _computerIsWaitingForRespond;
        }
        set { _computerIsWaitingForRespond = value; }
    }

    public enum ComputerLevelEnum
    {
        One, Two, Three, Four, Five
    }

    public ComputerLevelEnum ComputerLevel
    {
        get
        {
            if (!_computerLevel.HasValue)
            {
                _computerLevel = ReversoPlayerPrefs.GetComputerLevel();
            }
            return (ComputerLevelEnum)_computerLevel.Value;
        }
        set
        {
            if (_computerLevel.HasValue)
            {
                if (value != _computerLevel)
                {
                    ReversoPlayerPrefs.SetComputerLevel((ComputerLevelEnum)value);
                }
                _computerLevel = value;
            }
            else
            {
                _computerLevel = value;
                ReversoPlayerPrefs.SetComputerLevel((ComputerLevelEnum)_computerLevel);
            }




        }
    }


    public Othello.PlayerColor PlayerColor
    {
        get
        {
            return _playerColor;
        }
    }
    public Othello.PlayerColor ComputerColor
    {
        get
        {

            return _playerColor == Othello.PlayerColor.White ? Othello.PlayerColor.Black : Othello.PlayerColor.White;
        }
    }

    public bool ShowTimers
    {
        get
        {
            if (PlayingOnline) return true;
            return PlayAgainstComputer ? false : true && ReversoPlayerPrefs.IsTimerOn();
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
    public static bool HasStartedGame
    {
        get; set;
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
    private static int _chosenBoard = -1;
    public static int ChosenBoard
    {
        get
        {
            _chosenBoard = ReversoPlayerPrefs.GetChosenBoard();
            return _chosenBoard;
        }
        set
        {
            _chosenBoard = value;

        }
    }

    private OthelloManager()
    {
    }



    private static void StartGame()
    {
        //Application.LoadLevel("GameScene")
        HasStartedGame = true;
        ReversoPlayerPrefs.SetChosenBoard(_chosenBoard);
        SceneManager.LoadSceneAsync("GameScene");
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
        _instance._playerColor = Othello.PlayerColor.White;
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
