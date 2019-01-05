using UnityEngine;
using System.Collections;

public class Timer
{

    private static Timer _instance;
    public float BlackTimer { get; set; }
    public float WhiteTimer { get; set; }

    public static Timer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Timer();
            }
            return _instance;
        }
    }

}
