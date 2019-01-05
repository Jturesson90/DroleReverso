using UnityEngine;
using System.Collections;

public class SplashScript : MonoBehaviour
{

    public LevelSwitcher levelSwitcher;
    // Use this for initialization
    void Start()
    {
        ReversoPlayerPrefs.SetShouldLogIn(true);
        StartCoroutine(WaitForRealSeconds(1.5f));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
        levelSwitcher.SwitchLevel("GameMenu", 1f);
    }
}
