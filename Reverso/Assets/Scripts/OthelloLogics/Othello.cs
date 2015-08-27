using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class Othello : MonoBehaviour
{
    GameBoard gameBoard;

    CantMove cantMove;

    public GameObject stateText;

    public bool canMove;
    public bool hasWinner;


    private bool doneFirstMove = false;
    private bool showOutOfTime = true;
    //SpeedMode 
    public float timeLeft = 180f;

    bool clickCoolDown = false;
    float clickCoolDownTimer = 1f;

    public bool WaitingForSpeedModeCallback = false;
    public enum PlayerColor
    {
        White,
        Black,
        NoOne
    }

    public static PlayerColor CURRENT_PLAYER = PlayerColor.White;

    public OthelloPiece[,] bricks;


    public bool isDebugging = false;
    void Awake()
    {

        stateText.SetActive(isDebugging ? true : false);

        showOutOfTime = true;
        canMove = true;
        cantMove = GameObject.Find("CantMovePanel").GetComponent<CantMove>();
        CURRENT_PLAYER = PlayerColor.White;

        gameBoard = GetComponent<GameBoard>();
        if (OthelloManager.Instance != null)
        {
            if (OthelloManager.Instance.PlayingOnline)
            {
                GetComponent<OnlinePlayingEvents>().enabled = true;
            }
        }
        Timer.Instance.BlackTimer = timeLeft;
        Timer.Instance.WhiteTimer = timeLeft;
    }

    void Start()
    {

        bricks = gameBoard.SetupBoard(bricks);
        ShowHints();
    }


    public void Pressed(int x, int y)
    {
        HandleInput(bricks[x, y]);
    }

    public void OnLongPressed(int x, int y)
    {
        HandleOnLongPressed(bricks[x, y]);
    }

    public void OnLongReleased()
    {

        OthelloRules.ReleaseAllFlashes(bricks);
    }

    private void HandleOnLongPressed(OthelloPiece brick)
    {
        ArrayList validDirections;
        validDirections = OthelloRules.GetValidDirections(bricks, brick);
        if (validDirections.Count > 0)
        {
            for (int i = 0; i < validDirections.Count; i++)
            {
                OthelloRules.Direction direction = (OthelloRules.Direction)validDirections[i];
                OthelloPiece nextBrick = OthelloRules.NextBrickInDirection(brick.x, brick.y, direction, bricks);
                OthelloRules.FlashRow(nextBrick, direction, bricks);

            }
        }
    }
    private void HandleInput(OthelloPiece brick)
    {
        if (clickCoolDown)
        {
            return;
        }

        if (OthelloManager.Instance.PlayAgainstComputer)
        {
            if (CURRENT_PLAYER == PlayerColor.Black)
            {
                return;
            }
        }
        if (OthelloManager.Instance.PlayingOnline)
        {
            if (OthelloManager.Instance.PlayerColor != CURRENT_PLAYER)
            {
                return;
            }
        }
        ArrayList validDirections;
        validDirections = OthelloRules.GetValidDirections(bricks, brick);

        if (validDirections.Count > 0)
        {
            clickCoolDown = true;
            doneFirstMove = true;
            showOutOfTime = false;
            OthelloRules.PutDownBrick(brick);
            for (int i = 0; i < validDirections.Count; i++)
            {
                OthelloRules.Direction direction = (OthelloRules.Direction)validDirections[i];
                OthelloPiece nextBrick = OthelloRules.NextBrickInDirection(brick.x, brick.y, direction, bricks);
                OthelloRules.TurnRow(nextBrick, direction, bricks);

            }

            gameBoard.UpdateBoard(bricks);
            ChangePlayer();
            CheckForValidMoves();
            ShowHints();
            if (OthelloManager.Instance.PlayAgainstComputer)
            {
                StartCoroutine(WaitAndMakeComputerMove(1.0F));
            }
            if (OthelloManager.Instance.PlayingOnline)
            {
                DoneMyTurn();
            }

        }
    }

    public void OpponentLost()
    {
        AchievementsManager.Instance.OnlineWin();
        if (OthelloManager.Instance.PlayerIsWhite())
        {
            MakeWinner(PlayerColor.White);
           
        }
        else
        {
            MakeWinner(PlayerColor.Black);
        }
    }

    IEnumerator WaitAndMakeComputerMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ComputerMove();
    }



    private void MakeRandomMove()
    {
        ArrayList validMoves;
        validMoves = OthelloRules.GetAllValidMoves(bricks);
        if (validMoves.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, validMoves.Count - 1);
            OthelloPiece brick = validMoves[index] as OthelloPiece;
            ArrayList validDirections;
            validDirections = OthelloRules.GetValidDirections(bricks, brick);
            if (validDirections.Count > 0)
            {
                OthelloRules.PutDownBrick(brick);
                for (int i = 0; i < validDirections.Count; i++)
                {
                    OthelloRules.Direction direction = (OthelloRules.Direction)validDirections[i];
                    OthelloPiece nextBrick = OthelloRules.NextBrickInDirection(brick.x, brick.y, direction, bricks);
                    OthelloRules.TurnRow(nextBrick, direction, bricks);
                }

                gameBoard.UpdateBoard(bricks);
                ChangePlayer();
                CheckForValidMoves();

                ShowHints();
                if (OthelloManager.Instance.PlayingOnline)
                {
                    DoneMyTurn();
                }
            }
        }
    }
    void Update()
    {
        if (OthelloManager.Instance.ShowTimers && doneFirstMove && !WaitingForSpeedModeCallback && !hasWinner)
        {
            if (CURRENT_PLAYER == PlayerColor.White)
            {
                Timer.Instance.WhiteTimer -= Time.deltaTime;
                Timer.Instance.WhiteTimer = Mathf.Max(0, Timer.Instance.WhiteTimer);
                if (Timer.Instance.WhiteTimer <= 0f)
                {
                    HandleGameOverDueNoTimeLeft(PlayerColor.White);
                    print("WHITE LOST");
                }

            }
            else if (CURRENT_PLAYER == PlayerColor.Black)
            {
                Timer.Instance.BlackTimer -= Time.deltaTime;
                Timer.Instance.BlackTimer = Mathf.Max(0, Timer.Instance.BlackTimer);
                if (Timer.Instance.BlackTimer <= 0f)
                {
                    HandleGameOverDueNoTimeLeft(PlayerColor.Black);
                    print("BLACK LOST");
                }
            }

        }

        if (clickCoolDown)
        {
            clickCoolDownTimer -= Time.deltaTime;
            if (clickCoolDownTimer < 0)
            {
                clickCoolDownTimer = 1f;
                clickCoolDown = false;
            }
        }
        if (ReversoGooglePlay.Instance != null)
        {
            if (stateText.activeSelf)
            {
                stateText.GetComponent<Text>().text = "" + ReversoGooglePlay.Instance.State;
            }
        }
    }
    private void HandleGameOverDueNoTimeLeft(PlayerColor pc)
    {
        if (OthelloManager.Instance.PlayingOnline)
        {
            OnlineGameOverDueNoTimeLeft(pc);
        }
        else
        {
            GameOver(pc);
        }
    }
    private void OnlineGameOverDueNoTimeLeft(PlayerColor pc)
    {
        if (pc != OthelloManager.Instance.PlayerColor) return;
        if (ReversoGooglePlay.Instance != null)
        {
            ReversoGooglePlay.Instance.OnGameOver();
            ReversoGooglePlay.Instance.BroadcastILost();
        }

        hasWinner = true;
        cantMove.OutOfTimeWin(pc);
        CURRENT_PLAYER = PlayerColor.NoOne;
        gameBoard.CheckArrows(CURRENT_PLAYER);
    }
    private void ComputerMove()
    {
        print("Computer tries to make a move");

        if (CURRENT_PLAYER == PlayerColor.White)
        {
            return;
        }
        MakeRandomMove();

        print("Computer has made his turn");
    }
    public void OnPlayerLeft()
    {
        cantMove.OpponentLeft();
    }
    private void ShowHints()
    {
        if (!OthelloManager.Instance.UseHints)
        {
            return;
        }
        ArrayList validMoves;
        validMoves = OthelloRules.GetAllValidMoves(bricks);
        foreach (OthelloPiece brick in bricks)
        {
            if (brick.brickColor == BrickColor.Hint)
            {
                brick.brickColor = BrickColor.Empty;
            }
        }
        foreach (OthelloPiece brick in validMoves)
        {
            if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
            {
                brick.brickColor = BrickColor.Hint;
            }
        }

    }
    private void CheckForValidMoves()
    {
        if (OthelloRules.HasWinner(bricks))
        {
            GameOver();
            return;
        }
        // No one can move
        if (!OthelloRules.CanMakeMove(bricks))
        {
            ChangePlayer();
            if (!OthelloRules.CanMakeMove(bricks))
            {
                AchievementsManager.Instance.EarlyWin();
                GameOver();
            }
            else
            {
                if (CURRENT_PLAYER == Othello.PlayerColor.Black)
                {
                    showOutOfTime = false;
                    cantMove.WhiteCantMove();
                    if (OthelloManager.Instance.PlayAgainstComputer)
                    {
                        StartCoroutine(WaitAndMakeComputerMove(1.0F));
                    }
                }
                else
                {
                    showOutOfTime = false;
                    cantMove.BlackCantMove();
                }

            }
        }

    }
    private void MakeWinner(PlayerColor playerColor)
    {
        if (ReversoGooglePlay.Instance != null) ReversoGooglePlay.Instance.OnGameOver();
        hasWinner = true;
        if (playerColor == PlayerColor.Black)
        {
            cantMove.OutOfTimeWin(PlayerColor.White);
        }
        else
        {
            cantMove.OutOfTimeWin(PlayerColor.Black);
        }

        CURRENT_PLAYER = PlayerColor.NoOne;
        gameBoard.CheckArrows(CURRENT_PLAYER);

    }
    private void GameOver(PlayerColor pc)
    {
        if (ReversoGooglePlay.Instance != null) ReversoGooglePlay.Instance.OnGameOver();
        hasWinner = true;

        cantMove.OutOfTimeWin(pc);

        CURRENT_PLAYER = PlayerColor.NoOne;
        gameBoard.CheckArrows(CURRENT_PLAYER);



    }
    private void GameOver()
    {
        if (ReversoGooglePlay.Instance != null) ReversoGooglePlay.Instance.OnGameOver();
        hasWinner = true;
        if (OthelloManager.Instance.PlayAgainstComputer)
        {
            AchievementsManager.Instance.WonAgainstTheComputer();
        }
        else {
            AchievementsManager.Instance.LocalGameEnded();
        }
        CURRENT_PLAYER = PlayerColor.NoOne;
        gameBoard.CheckArrows(CURRENT_PLAYER);
        cantMove.NoOneCanMove(bricks);


    }

    void ChangePlayer()
    {
        CURRENT_PLAYER = CURRENT_PLAYER == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        gameBoard.CheckArrows(CURRENT_PLAYER);

    }
    public void OnOnlineRecievedData(byte[] onlineData)
    {

        if (onlineData == null) return;
        bricks = onlineData.ToOthelloPieceArray(bricks);
        doneFirstMove = true;
        gameBoard.UpdateBoard(bricks);
        ChangePlayer();
        CheckForValidMoves();
    }
    void DoneMyTurn()
    {
        ReversoGooglePlay.Instance.BroadcastMyTurn(bricks, CURRENT_PLAYER);
    }

    public void OutOfTime()
    {
        if (!showOutOfTime)
        {
            showOutOfTime = true;
            return;
        }
    }

}
