using UnityEngine;


class ReversoPlayerPrefs : MonoBehaviour
{
    const string HINTS_KEY = "hints";

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

}

