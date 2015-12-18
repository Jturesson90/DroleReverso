using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSwitcher : MonoBehaviour
{



    public float defaultFadeToClearTime = 2f;
    public float defaultFadeToBlackTime = 2f;
    public bool shouldFadeOnStart = true;
    public static LevelSwitcher levelSwitcher;
    RawImage image;
    void Awake()
    {
        levelSwitcher = this;
        image = GetComponent<RawImage>();
    }

    void Start()
    {
        if (shouldFadeOnStart)
        {

            FadeToClear();
        }
        else
        {
            FadeToClear(0f);
        }

    }
    public void SwitchLevel(string level)
    {
        StartCoroutine(_SwitchLevel(level, defaultFadeToBlackTime));
    }
    public void SwitchLevel(string level, float seconds)
    {
        print("Switching level");
        StartCoroutine(_SwitchLevel(level, seconds));
    }
    public void SwitchLevel(int level)
    {
        StartCoroutine(_SwitchLevel(level, defaultFadeToBlackTime));
    }
    public void SwitchLevel(int level, float seconds)
    {
        StartCoroutine(_SwitchLevel(level, seconds));
    }
    private IEnumerator _SwitchLevel(int level, float seconds)
    {

        image.CrossFadeAlpha(1f, seconds, true);
        yield return StartCoroutine(WaitForRealSeconds(seconds));
        Application.LoadLevel(level);

    }
    private IEnumerator _SwitchLevel(string level, float seconds)
    {

        image.CrossFadeAlpha(1f, seconds, true);
        yield return StartCoroutine(WaitForRealSeconds(seconds));
        Application.LoadLevel(level);

    }
    public void FadeToClear()
    {
        SetAlphaOne();
        image.CrossFadeAlpha(0f, defaultFadeToClearTime, true);
    }
    public void FadeToClear(float seconds)
    {
        SetAlphaOne();
        image.CrossFadeAlpha(0f, seconds, true);

    }
    private void SetAlphaOne()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }


    private IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }

}
