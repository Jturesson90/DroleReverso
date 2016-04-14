using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OtherGamesShower : MonoBehaviour
{

    public LeanTweenType tweenTypeIn;
    public LeanTweenType tweenTypeOut;

    public OtherGame[] otherGames;
    public Image imageHolder;
    public GameObject parentPanel;
    private OtherGame _chosenGame;
    private int _chosenIndex;
    public float timeBetweenChanges = 15f;
    private float changedAtTime = 0f;
    private float _startPosX;
    private float _endPosX;
    private RectTransform _rectT;



    Vector2 _toPos;
    Vector2 _fromPos;
    bool tweenedOut = false;
    // Use this for initialization

    void Awake()
    {
        _rectT = parentPanel.GetComponent<RectTransform>();
        _startPosX = _rectT.anchoredPosition.x;
        _toPos = _rectT.anchoredPosition;
        _fromPos = _toPos + Vector2.right * 400f;
        _rectT.anchoredPosition = _fromPos;
        _endPosX = _rectT.anchoredPosition.x;        // _rectT.anchoredPosition = new Vector2(_endPosX, _rectT.anchoredPosition.y);
        _chosenIndex = Random.Range(0, otherGames.Length);


    }
    void Start()
    {
        TweenIn(2f);
    }


    void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > changedAtTime + timeBetweenChanges)
        {
            ChangeGame();
        }
    }
    void ChangeGame()
    {
        if (tweenedOut) return;
        TweenOut(0f);
        StartCoroutine(Wait(1f));


    }
    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TweenIn(0f);
    }
    void TweenOut(float delay)
    {
        LeanTween.move(_rectT, _fromPos, 0.6f).setEase(tweenTypeOut).setDelay(delay);
        tweenedOut = true;
    }
    void TweenIn(float delay)
    {
        if (!imageHolder) return;
        _chosenIndex++;
        if (_chosenIndex > otherGames.Length - 1) _chosenIndex = 0; ;
        imageHolder.sprite = otherGames[_chosenIndex].Image;
        changedAtTime = Time.timeSinceLevelLoad;

        LeanTween.move(_rectT, _toPos, 0.6f).setEase(tweenTypeIn).setDelay(delay);
      
        tweenedOut = false;
    }

    public void Pressed()
    {
        OpenGameAtStore();
    }
    private void OpenGameAtStore()
    {
        _chosenGame = otherGames[_chosenIndex];
        if (_chosenGame.Equals(null)) return;

        string url;
#if UNITY_ANDROID
        url = _chosenGame.AndroidUrl;

#elif UNITY_IOS
       url = _chosenGame.IOSUrl;
#endif
        if (!url.Equals(string.Empty))
        {
            Application.OpenURL(url);
        }
    }
}
[System.Serializable]
public struct OtherGame
{
    public Sprite Image;
    public string AndroidUrl;
    public string IOSUrl;
}