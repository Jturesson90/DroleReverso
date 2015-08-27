using UnityEngine;
using System.Collections;

public class ExitApplication : MonoBehaviour
{
    private const string TRIGGER_SWIPE_OUT = "SwipeOut";
    private const string TRIGGER_SWIPE_IN = "SwipeIn";
    Othello othello;

    Animator anim;
    private bool canSwipeIn;

    void Awake()
    {
        try
        {
            othello = GameObject.Find("Othello").GetComponent<Othello>();
        }
        catch
        {
        }
        anim = GetComponent<Animator>();
    }



    // Use this for initialization
    void Start()
    {
        canSwipeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.loadedLevelName.Equals("GameScene"))
            {
                ShowExitDialog();
            }
            else
            {
                NavigationUtil.OnBackbuttonPressedInMenu();
            }

        }
    }

    public void ShowExitDialog()
    {

        if (canSwipeIn)
        {
            anim.SetTrigger(TRIGGER_SWIPE_IN);
            canSwipeIn = false;
        }

        if (!othello) return;
        othello.canMove = false;



    }
    public void YesButton()
    {
        if (Application.loadedLevelName.Equals("GameScene"))
        {
            if (OthelloManager.Instance != null)
            {
                if (OthelloManager.Instance.PlayingOnline)
                {
                    OnlinePlayingEvents.OnQuit();
                    return;
                }
            }
            Application.LoadLevel("GameMenu");
        }
        else
        {
            Application.Quit();
        }
    }
    public void NoButton()
    {
        anim.SetTrigger(TRIGGER_SWIPE_OUT);
    }
    public void AnimationSwipeOutCallback()
    {
        canSwipeIn = true;
        if (!othello) return;
        othello.canMove = true;

    }
}
