using UnityEngine;
using System.Collections;

public static class ComputerName
{


    public static string GetComputerName(OthelloManager.ComputerLevelEnum computerLevel)
    {
        var name = string.Empty;
        var computerNames = new string[]
        {
            "Computer one",
            "Computer two",
            "Computer three",
            "Computer four",
            "Computer five",
        };
        if (computerNames.Length > (int)computerLevel)
        {
            name = computerNames[(int)computerLevel];
        }
        else
        {
            name = "Computer";
        }

        return name;
    }



}
