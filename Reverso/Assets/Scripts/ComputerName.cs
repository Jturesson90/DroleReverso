using UnityEngine;
using System.Collections;

public static class ComputerName
{


    public static string GetComputerName(OthelloManager.ComputerLevelEnum computerLevel)
    {
        var name = string.Empty;
        var computerNames = new string[]
        {
            "Easy Computer",
            "Normal Computer",
            "Hard Computer",
            "Very Hard Computer",
            "Computer Impossible",
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
