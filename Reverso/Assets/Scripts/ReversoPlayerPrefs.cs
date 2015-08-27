using UnityEngine;


class ReversoPlayerPrefs : MonoBehaviour
{
    const string HINTS_KEY = "hints";
    const string LOG_IN_KEY = "login";

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


}

