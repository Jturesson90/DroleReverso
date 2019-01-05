using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReversoSceneManager : MonoBehaviour
{
    private const string GAME_SCENE = "GameScene";
    private const string MENU_SCENE = "GameMenu";

    public static ReversoSceneManager _instance;

    private void Awake()
    {
        if (_instance == this) return;

        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this);
    }

    public static void RestartScene(string sceneName)
    {
        _instance.StartScene(SceneManager.GetActiveScene().name);
    }

    public static void StartMenuScene()
    {
        _instance.StartScene(MENU_SCENE);
    }

    public static void StartGameScene()
    {
        _instance.StartScene(GAME_SCENE);
    }

    private void StartScene(string sceneName)
    {
        StartCoroutine(StartSceneCourutine(sceneName));

    }

    private IEnumerator StartSceneCourutine(string sceneName)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
