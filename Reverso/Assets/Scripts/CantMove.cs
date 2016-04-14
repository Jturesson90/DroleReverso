using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CantMove : MonoBehaviour
{

    Animator anim;
    Othello othello;


    private const string TRIGGER_SWIPE_OUT = "SwipeOut";
    private const string TRIGGER_SWIPE_IN = "SwipeIn";
    Text cantMoveText;
    RectTransform parentRectTransform;
    bool returnToMenu = false;
    Vector3 forWhite = new Vector3(0f, 0f, 0f);
    Vector3 forBlack = new Vector3(0f, 0f, 180f);

    void Awake()
    {
        anim = GetComponent<Animator>();
        othello = GameObject.Find("Othello").GetComponent<Othello>();
        cantMoveText = GameObject.Find("CantMoveText").GetComponent<Text>();
        parentRectTransform = GameObject.Find("CantMovePanelParent").GetComponent<RectTransform>();


    }

    void Start()
    {

    }

    public void WhiteCantMove()
    {
        print("WhiteCantMove");
        othello.canMove = false;
        othello.WaitingForSpeedModeCallback = true;
        cantMoveText.text = "White cant move!\nBlack's turn";
        RotateToWhite();
        anim.SetTrigger(TRIGGER_SWIPE_IN);

    }
    public void BlackCantMove()
    {
        print("BlackCantMove");
        othello.canMove = false;
        RotateToBlack();
        othello.WaitingForSpeedModeCallback = true;
        cantMoveText.text = "Black cant move!\nWhite's turn";
        anim.SetTrigger(TRIGGER_SWIPE_IN);
    }

    public void BlackOutOfTime()
    {
        othello.canMove = false;
        RotateToBlack();
        othello.WaitingForSpeedModeCallback = true;
        cantMoveText.text = "Out of time!\nWhite's turn";
        anim.SetTrigger(TRIGGER_SWIPE_IN);
    }

    public void WhiteOutOfTime()
    {
        othello.canMove = false;
        cantMoveText.text = "Out of time!\nBlack's turn";
        RotateToWhite();
        othello.WaitingForSpeedModeCallback = true;
        anim.SetTrigger(TRIGGER_SWIPE_IN);
    }
    public void OpponentLeft()
    {
        othello.canMove = false;
        returnToMenu = true;
        string endText = "Opponent left the game";
        cantMoveText.text = endText;
        anim.SetTrigger(TRIGGER_SWIPE_IN);
    }

    public void OutOfTimeWin(Othello.PlayerColor looser)
    {
        string winText = "Congratulations, ";
        print("Looser! " + looser);
        if (OthelloManager.Instance.PlayingOnline)
        {
            bool isWinner = looser == OthelloManager.Instance.PlayerColor ? false : true;

            winText = isWinner ? "Congratulations, you won!!" : "You lost!";
        }
        else
        {
            if (looser == Othello.PlayerColor.White)
            {
                winText += BlackWon();
            }
            else
            {
                winText += WhiteWon();
            }
        }
        cantMoveText.text = winText;
        anim.SetTrigger(TRIGGER_SWIPE_IN);
    }
    public void NoOneCanMove(OthelloPiece[,] bricks)
    {
        print("NoOneCanMove");
        othello.canMove = false;
        int whites = 0;
        int blacks = 0;
        string winText = "Congratulations, ";
        bool whiteWon = false;
        foreach (OthelloPiece brick in bricks)
        {
            if (brick.brickColor == BrickColor.White)
            {
                whites++;
            }
            else if (brick.brickColor == BrickColor.Black)
            {
                blacks++;
            }
        }
        whiteWon = whites > blacks ? true : false;

        if (OthelloManager.Instance.PlayingOnline)
        {
            if (OthelloManager.Instance.PlayerIsWhite() && whiteWon)
            {
                winText += " you won!!";
            }
            else if (OthelloManager.Instance.PlayerIsWhite() && !whiteWon)
            {
                winText = "You lost!";
            }
            else if (!OthelloManager.Instance.PlayerIsWhite() && !whiteWon)
            {
                winText += " you won!!";
            }
            else if (!OthelloManager.Instance.PlayerIsWhite() && whiteWon)
            {
                winText = "You lost!";
            }
            else
            {
                winText = "It's a draw!";
            }


        }
        else
        {
            if (whiteWon)
            {
                winText += WhiteWon();
            }
            else if (!whiteWon)
            {
                winText += BlackWon();
            }
            else
            {
                winText = Draw();
            }
        }

        if (OthelloManager.Instance.PlayAgainstComputer)
        {
            if (OthelloManager.Instance.PlayerIsWhite() && !whiteWon)
            {
                winText = "You lost!";
            }

            else if (!OthelloManager.Instance.PlayerIsWhite() && whiteWon)
            {
                winText = "You lost!";
            }
        }
        cantMoveText.text = winText;
        anim.SetTrigger(TRIGGER_SWIPE_IN);
    }
    private string WhiteWon()
    {
        RotateToWhite();
        return "White!";
    }
    private string BlackWon()
    {

        RotateToBlack();
        return "Black!";
    }
    private string Draw()
    {
        RotateToWhite();
        return "DRAW!";
    }
    public void OKPressed()
    {
        anim.SetTrigger(TRIGGER_SWIPE_OUT);
    }
    public void AnimationSwipeOutCallback()
    {
        othello.canMove = true;
        if (returnToMenu)
        {
            NavigationUtil.ShowMainMenu();
            return;
        }
        if (othello.hasWinner)
        {
            NavigationUtil.ShowMainMenu();
        }
        else if (othello.WaitingForSpeedModeCallback)
        {
            othello.WaitingForSpeedModeCallback = false;
        }
    }

    private void RotateToWhite()
    {
        if (OthelloManager.Instance.PlayAgainstComputer || OthelloManager.Instance.PlayingOnline) return;
        parentRectTransform.eulerAngles = forWhite;
    }
    private void RotateToBlack()
    {
        if (OthelloManager.Instance.PlayAgainstComputer || OthelloManager.Instance.PlayingOnline) return;
        parentRectTransform.eulerAngles = forBlack;
    }
}
