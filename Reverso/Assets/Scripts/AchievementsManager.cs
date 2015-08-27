using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class AchievementsManager
{

    const string ACHIEVEMENT_COMPUTER_WIN_1 = "CgkI89rkoNgdEAIQAw";
    const string ACHIEVEMENT_COMPUTER_WIN_10 = "CgkI89rkoNgdEAIQAg";
    const string ACHIEVEMENT_2PLAYER_WIN_10 = "CgkI89rkoNgdEAIQBQ";
    const string ACHIEVEMENT_EARLY_WIN = "CgkI89rkoNgdEAIQBA";
    const string ACHIEVEMENT_ONLINE_WIN = "CgkI89rkoNgdEAIQAQ";

    private static AchievementsManager _instance;
    public static AchievementsManager Instance
    {
        get
        {
            Debug.Log("instance");
            if (_instance == null)
            {
                Debug.Log("instance is null");
                _instance = new AchievementsManager();
            }
            return _instance;
        }
    }

    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    public void WonAgainstTheComputer()
    {
        Debug.Log("WON");
        UnlockAchievement(ACHIEVEMENT_COMPUTER_WIN_1);
        IncrementAchievement(ACHIEVEMENT_COMPUTER_WIN_10);
    }

    public void LocalGameEnded()
    {
        IncrementAchievement(ACHIEVEMENT_2PLAYER_WIN_10);
    }

    public void OnlineWin()
    {
        UnlockAchievement(ACHIEVEMENT_ONLINE_WIN);
    }
    public void EarlyWin()
    {
        UnlockAchievement(ACHIEVEMENT_EARLY_WIN);
    }
    private void UnlockAchievement(string id)
    {
        Debug.Log("UNLOCK ACHIEVMENT");
        PlayGamesPlatform.Instance.ReportProgress(id, 100.0f, (bool success) =>
        {
            // handle success or failure
        });
    }
    private void IncrementAchievement(string id)
    {
        Debug.Log("UNLOCK ACHIEVMENT");
        PlayGamesPlatform.Instance.IncrementAchievement(
        id, 1, (bool success) =>
        {
            // handle success or failure
        });
    }


}
