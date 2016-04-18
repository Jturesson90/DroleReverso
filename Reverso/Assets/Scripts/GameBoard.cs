using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameBoard : MonoBehaviour
{
    public GameObject rotateCanvas;
    public GameObject gameBoard;
    public GameObject opponentName;
    public GameObject brick;
    public GameObject blacksScoreText, whitesScoreText;

    public Color32 blackTextColor;
    public Color32 whiteTextColor;


    private Text blackTextWhiteSide, whiteTextWhiteSide, blackTextBlackSide, whiteTextBlackSide;

    public GameObject whiteArrow;
    public GameObject blackArrow;

    public Text whiteTimeLeftText;
    public Text blackTimeLeftText;

    public static int BoardWidth = 8;

    float timer = 0.5f;

    bool pressed = false;
    bool longPress = false;
    bool longPressUsed = false;

    void Awake()
    {

        opponentName.SetActive(false);
        whiteTextWhiteSide = GameObject.Find("WhitesWhiteSide").GetComponent<Text>();
        whiteTextBlackSide = GameObject.Find("WhitesBlackSide").GetComponent<Text>();
        blackTextWhiteSide = GameObject.Find("BlacksWhiteSide").GetComponent<Text>();
        blackTextBlackSide = GameObject.Find("BlacksBlackSide").GetComponent<Text>();




        whiteTimeLeftText.gameObject.SetActive(false);
        blackTimeLeftText.gameObject.SetActive(false);
    }
    void Start()
    {
        CheckPlayerColor();
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        if (OthelloManager.Instance.ShowTimers)
        {
            ActivateTimers();
            FixOnlineOpponentTimerRotation();
        }
        else
        {
            DeactiveTimers();
        }
    }

    private void CheckPlayerColor()
    {
        if (OthelloManager.Instance.PlayAgainstComputer)
        {
            SetOpponentName("Computer " + OthelloManager.Instance.ComputerLevel);
            DeactiveOpponentTimeText();

            return;
        }
        if (OthelloManager.Instance.PlayingOnline)
        {
            if (OthelloManager.Instance.PlayerColor == Othello.PlayerColor.White)
            {

            }
            else
            {
                gameBoard.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
                rotateCanvas.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }


            DeactiveOpponentTimeText();
            SetOpponentName(OthelloManager.Instance.OpponentName);
        }



    }
    public void DeactiveOpponentTimeText()
    {
        whitesScoreText.SetActive(OthelloManager.Instance.PlayerIsWhite() ? true : false);
        blacksScoreText.SetActive(OthelloManager.Instance.PlayerIsWhite() ? false : true);

    }
    private void SetOpponentName(string name)
    {
        opponentName.SetActive(true);
        opponentName.GetComponent<Text>().text = name;
        opponentName.GetComponent<Text>().color = OthelloManager.Instance.PlayerIsWhite() ? whiteTextColor : blackTextColor;
    }

    Transform selected;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            if (CastDownRay())
            {
                pressed = true;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
            longPress = false;
            timer = 0.5f;
            CastUpRay();
        }

        if (pressed)
        {
            longPress = timer < 0f ? true : false;
            timer -= Time.deltaTime;
            if (longPress && !longPressUsed)
            {
                longPressUsed = true;
                OthelloPiece op = selected.GetComponent<OthelloPiece>();
                if (op && (op.brickColor == BrickColor.Empty || op.brickColor == BrickColor.Hint))
                {
                    GetComponent<Othello>().OnLongPressed(op.x, op.y);
                }
            }
        }
        else if (longPressUsed)
        {
            GetComponent<Othello>().OnLongReleased();
            longPressUsed = false;
        }

    }
    void FixedUpdate()
    {
        whiteTimeLeftText.text = Timer.Instance.WhiteTimer.ToMinutesAndSeconds();
        blackTimeLeftText.text = Timer.Instance.BlackTimer.ToMinutesAndSeconds();
    }
    public void ActivateTimers()
    {
        blackTimeLeftText.gameObject.SetActive(true);
        whiteTimeLeftText.gameObject.SetActive(true);
    }
    public void DeactiveTimers()
    {
        whiteTimeLeftText.gameObject.SetActive(false);
        blackTimeLeftText.gameObject.SetActive(false);
    }
    void CastUpRay()
    {

#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
#else
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
#endif

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (selected == hit.transform)
            {
                OthelloPiece op = hit.transform.GetComponent<OthelloPiece>();
                if (op && (op.brickColor == BrickColor.Empty || op.brickColor == BrickColor.Hint))
                {
                    if (!longPressUsed)
                    {
                        GetComponent<Othello>().Pressed(op.x, op.y);
                    }
                }
            }
            else
            {

            }
        }
    }
    bool CastDownRay()
    {
#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
#else
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
#endif
        if (!GetComponent<Othello>().canMove)
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "GamePiece")
            {
                selected = hit.transform;
                return true;
            }
        }
        return false;
    }

    public OthelloPiece[,] SetupBoard(OthelloPiece[,] bricks)
    {

        CheckArrows(Othello.CURRENT_PLAYER);

        bricks = new OthelloPiece[8, 8];
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                GameObject piece = (GameObject)Instantiate(brick, new Vector3(x, y, -0.4f), Quaternion.identity);
                piece.transform.parent = transform;
                OthelloPiece othello = piece.GetComponentInChildren<OthelloPiece>();
                othello.brickColor = BrickColor.Empty;
                othello.x = x;
                othello.y = y;
                bricks[x, y] = othello;
            }
        }

        bricks[3, 4].brickColor = BrickColor.White;
        bricks[4, 3].brickColor = BrickColor.White;
        bricks[3, 3].brickColor = BrickColor.Black;
        bricks[4, 4].brickColor = BrickColor.Black;
        UpdateBoard(bricks);
        return bricks;
    }



    public void CheckArrows(Othello.PlayerColor playerColor)
    {

        if (playerColor == Othello.PlayerColor.White)
        {
            whiteArrow.SetActive(true);
            blackArrow.SetActive(false);
        }
        else if (playerColor == Othello.PlayerColor.Black)
        {
            whiteArrow.SetActive(false);
            blackArrow.SetActive(true);
        }
        else
        {
            whiteArrow.SetActive(false);
            blackArrow.SetActive(false);
        }
    }
    public void UpdateBoard(OthelloPiece[,] bricks)
    {
        int blacks = 0;
        int whites = 0;
        foreach (OthelloPiece i in bricks)
        {
            if (i.brickColor == BrickColor.Black)
            {
                blacks++;
            }
            else if (i.brickColor == BrickColor.White)
            {
                whites++;
            }
        }
        UpdateLabels(blacks, whites);


    }
    public void UpdateLabels(int blacks, int whites)
    {

        if (Application.loadedLevelName.Equals("GameScene"))
        {
            UpdateBlackSideLabels(blacks, whites);
            UpdateWhiteSideLabels(blacks, whites);
            return;
        }
    }
    private void UpdateBlackSideLabels(int blacks, int whites)
    {
        whiteTextBlackSide.text = "Whites - " + whites;
        blackTextBlackSide.text = blacks + " - Blacks";
    }
    private void UpdateWhiteSideLabels(int blacks, int whites)
    {
        whiteTextWhiteSide.text = "Whites - " + whites;
        blackTextWhiteSide.text = blacks + " - Blacks";
    }

    private void FixOnlineOpponentTimerRotation()
    {
        if (OthelloManager.Instance.PlayingOnline)
        {
            if (OthelloManager.Instance.PlayerColor == Othello.PlayerColor.White)
            {
                blackTimeLeftText.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                whiteTimeLeftText.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
        }
    }
}
