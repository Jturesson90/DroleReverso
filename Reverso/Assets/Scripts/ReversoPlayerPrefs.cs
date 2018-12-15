using System;
using UnityEngine;


class ReversoPlayerPrefs : MonoBehaviour
{
    const string HINTS_KEY = "hints";
    const string LOG_IN_KEY = "login";
    const string TIMER_KEY = "key";
    const string CHOSEN_BOARD = "CHOSEN_BOARD";
    const string COMPUTER_LEVEL = "COMPUTER_LEVEL";
    const string AUDIO = "AUDIO";


    public static void SetHints(bool hints)
    {
        if (hints)
        {
            PlayerPrefs.SetInt(HINTS_KEY, 1);
        }
        else
        {
            PlayerPrefs.SetInt(HINTS_KEY, 0);
        }
    }

    public static bool IsHintsOn()
    {
        return PlayerPrefs.GetInt(HINTS_KEY, 0) == 1 ? true : false;
    }
    public static bool ShouldLogIn()
    {
        return PlayerPrefs.GetInt(LOG_IN_KEY, 0) == 1 ? true : false;
    }

    public static bool IsTimerOn()
    {
        return PlayerPrefs.GetInt(TIMER_KEY, 0) == 1 ? true : false;
    }

    public static void SetShouldLogIn(bool login)
    {
        if (login)
        {
            PlayerPrefs.SetInt(LOG_IN_KEY, 1);
        }
        else
        {
            PlayerPrefs.SetInt(LOG_IN_KEY, 0);
        }
    }

    public static void SetTimer(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt(TIMER_KEY, 1);
        }
        else
        {
            PlayerPrefs.SetInt(TIMER_KEY, 0);
        }
    }

    public static void SetChosenBoard(int id)
    {

        PlayerPrefs.SetInt(CHOSEN_BOARD, id);
    }

    public static int GetChosenBoard()
    {
        int id = PlayerPrefs.GetInt(CHOSEN_BOARD, 0);

        return id;
    }
    public static void SetComputerLevel(OthelloManager.ComputerLevelEnum computerLevel)
    {

        PlayerPrefs.SetInt(COMPUTER_LEVEL, (int)computerLevel);
    }
    public static OthelloManager.ComputerLevelEnum GetComputerLevel()
    {
        OthelloManager.ComputerLevelEnum computerLevel = (OthelloManager.ComputerLevelEnum)PlayerPrefs.GetInt(COMPUTER_LEVEL, 0);

        return computerLevel;
    }

    public static void SetAudio(bool value)
    {
        PlayerPrefs.SetInt(AUDIO, Convert.ToInt32(value));
    }

    public static bool GetAudio()
    {
        return Convert.ToBoolean(PlayerPrefs.GetInt(AUDIO, Convert.ToInt32(true)));
    }
}

