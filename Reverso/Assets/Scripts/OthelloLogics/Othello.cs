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



    public GameObject cantMovePanel;

    private bool firstMove = false;
    private bool showOutOfTime = true;
    //SpeedMode 
    float speedModeTimer = 0f;
    float _maxSpeedModeTime = 5f;
    bool lastBrick = false;

    bool coolDown = false;
    float coolDownTimer = 1f;

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
    }

    void Start()
    {

        bricks = gameBoard.SetupBoard(bricks);
        if (OthelloManager.Instance.SpeedMode)
        {
            ResetSpeedMode();
        }
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
        if (coolDown)
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
            coolDown = true;
            firstMove = true;
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
    IEnumerator WaitAndMakeComputerMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ComputerMove();
    }



    private bool hasValidMoves = false;
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
        if (OthelloManager.Instance.SpeedMode && firstMove && !WaitingForSpeedModeCallback && !hasWinner)
        {
            speedModeTimer -= Time.deltaTime;
            if (speedModeTimer < 0f)
            {
                OutOfTime();
            }
            gameBoard.UpdateTime(speedModeTimer);
        }
        else if (OthelloManager.Instance.SpeedMode && hasWinner)
        {
            gameBoard.DeactiveTimers();
        }

        if (coolDown)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer < 0)
            {
                coolDownTimer = 1f;
                coolDown = false;
            }
        }
        if (ReversoGooglePlay.Instance != null) {
            if (stateText.activeSelf) {
                stateText.GetComponent<Text>().text = ""+ReversoGooglePlay.Instance.State;
            }
        }
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

    private void GameOver()
    {
        ReversoGooglePlay.Instance.OnGameOver();
        hasWinner = true;
        CURRENT_PLAYER = PlayerColor.NoOne;
        gameBoard.CheckArrows(CURRENT_PLAYER);
        cantMove.NoOneCanMove(bricks);
    }

    void ChangePlayer()
    {
        CURRENT_PLAYER = CURRENT_PLAYER == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
        gameBoard.CheckArrows(CURRENT_PLAYER);
        if (OthelloManager.Instance.SpeedMode)
        {
            ResetSpeedMode();
        }
      
    }
    public void OnOnlineRecievedData(byte[] onlineData)
    {
       
        if (onlineData == null) return;
        bricks = onlineData.ToOthelloPieceArray(bricks);
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

        if (!lastBrick)
        {
            if (CURRENT_PLAYER == PlayerColor.White)
            {
                cantMove.WhiteOutOfTime();
            }
            else
            {
                cantMove.BlackOutOfTime();
            }
            ResetSpeedMode();
        }
        MakeRandomMove();

    }
    private void ResetSpeedMode()
    {
        int bricksLeft = 0;
        foreach (OthelloPiece brick in bricks)
        {
            if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
            {
                bricksLeft++;
            }
        }
        if (bricksLeft == 1)
        {
            lastBrick = true;
        }
        else
        {
            lastBrick = false;
        }
        speedModeTimer = (float)bricksLeft;
        speedModeTimer *= 0.5f;
        speedModeTimer = Mathf.Max(_maxSpeedModeTime, speedModeTimer);
    }
}
