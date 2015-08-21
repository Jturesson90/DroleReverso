using System;
using UnityEngine;
using UnityEngine.UI;
/*
 * Prints debug messages to console and on screen.
 * Samuel Johansson, www.phaaxgames.com
 */

public class DebugOSD : MonoBehaviour
{

    private const double MessageTimeoutMS = 3000;
    private const string DebugOSDTitle = "DebugOSD";

    private DateTime Timer;
    private string _text = string.Empty;
    private string Text
    {
        get
        {
            if (DateTime.Now < Timer.AddMilliseconds(MessageTimeoutMS))
            {
                return _text;
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            _text = value;
            Timer = DateTime.Now;
        }
    }

    private static DebugOSD _instance;
    private static DebugOSD Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject(DebugOSDTitle);
                _instance = go.AddComponent<DebugOSD>();
                return _instance;
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }
    void FixedUpdate()
    {

        GetComponentInChildren<Text>().text = Text;

    }
    public static void Log(string message)
    {
        Instance.Text = message;
        Debug.Log(message);
    }

    public static void LogWarning(string message)
    {
        Instance.Text = message;
        Debug.LogWarning(message);
    }

    public static void LogError(string message)
    {
        Instance.Text = message;
        Debug.LogError(message);
    }



}