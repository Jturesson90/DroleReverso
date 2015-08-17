using UnityEngine;
using System.Collections;

public class Othello : MonoBehaviour
{
	GameBoard gameBoard;
	OthelloRules othelloRules;
	CantMove cantMove;
	OthelloManager manager;


	private bool showOutOfTime = true;
	public bool canMove;
	public bool hasWinner;
	private	bool firstMove = false;


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



	void Awake ()
	{
        
		showOutOfTime = true;
		canMove = true;
		cantMove = GameObject.Find ("CantMovePanel").GetComponent<CantMove> ();
		CURRENT_PLAYER = PlayerColor.White;
		othelloRules = GetComponent<OthelloRules> ();
		gameBoard = GetComponent<GameBoard> ();
		manager = GameObject.Find ("OthelloManager").GetComponent<OthelloManager> ();
	}

	void Start ()
	{
            
		bricks = gameBoard.SetupBoard (bricks);
		if(manager.SpeedMode){
			ResetSpeedMode ();
		}
		ShowHints ();
	}


	public void Pressed (int x, int y)
	{
		HandleInput (bricks [x, y]);
	}

	public void OnLongPressed(int x, int y)
	{
		HandleOnLongPressed (bricks[x,y]);
	}
	
	public void OnLongReleased()
	{
		othelloRules.ReleaseAllFlashes ();
	}

	private void HandleOnLongPressed(OthelloPiece brick){
		ArrayList validDirections;
		validDirections = othelloRules.ValidMoves (brick);
		if (validDirections.Count > 0) {
			for (int i = 0; i < validDirections.Count; i++) {
				OthelloRules.Direction direction = (OthelloRules.Direction)validDirections [i];
				OthelloPiece nextBrick = othelloRules.NextBrickInDirection (brick.x, brick.y, direction);
				othelloRules.FlashRow (nextBrick, direction);
				
			}
		}
	}
	private void HandleInput (OthelloPiece brick)
	{
		if (coolDown) {
			return;
		}
		ArrayList validDirections;
		validDirections = othelloRules.ValidMoves (brick);
		if (manager.PlayAgainstComputer) {
			if(CURRENT_PLAYER == PlayerColor.Black){
				return;
			}
		}
		if (validDirections.Count > 0) {
			coolDown = true;
			firstMove = true;
			showOutOfTime = false;
			othelloRules.ChosenBrick (brick);
			for (int i = 0; i < validDirections.Count; i++) {
				OthelloRules.Direction direction = (OthelloRules.Direction)validDirections [i];
				OthelloPiece nextBrick = othelloRules.NextBrickInDirection (brick.x, brick.y, direction);
				othelloRules.TurnRow (nextBrick, direction);

			}

			gameBoard.UpdateBoard (bricks);
			ChangePlayer ();
			CheckForValidMoves ();
			ShowHints();
			if(manager.PlayAgainstComputer){
				StartCoroutine(WaitAndMakeComputerMove(1.0F));
			}
		}
	}
	IEnumerator WaitAndMakeComputerMove(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		ComputerMove ();
	}
	private void MakeMove(OthelloPiece brick){
	}

	private void MakeRandomMove(){
		ArrayList validMoves;
		validMoves = othelloRules.GetAllValidMoves ();
		if (validMoves.Count > 0) {
			int index = Random.Range(0,validMoves.Count-1);
			OthelloPiece brick = validMoves[index] as OthelloPiece;
			ArrayList validDirections;
			validDirections = othelloRules.ValidMoves (brick);
			if (validDirections.Count > 0) {
				othelloRules.ChosenBrick (brick);
				for (int i = 0; i < validDirections.Count; i++) {
					OthelloRules.Direction direction = (OthelloRules.Direction)validDirections [i];
					OthelloPiece nextBrick = othelloRules.NextBrickInDirection (brick.x, brick.y, direction);
					othelloRules.TurnRow (nextBrick, direction);
				}
				
				gameBoard.UpdateBoard (bricks);
				ChangePlayer ();
				CheckForValidMoves ();
				ShowHints();
			}
		}
	}
	void Update(){
		if (manager.SpeedMode && firstMove && !WaitingForSpeedModeCallback && !hasWinner) {
			speedModeTimer -= Time.deltaTime;
			if (speedModeTimer < 0f) {
				OutOfTime ();
			}
			gameBoard.UpdateTime (speedModeTimer);
		} else if (manager.SpeedMode && hasWinner) {
			gameBoard.DeactiveTimers();
		}

		if (coolDown) {
			coolDownTimer -= Time.deltaTime;
			if(coolDownTimer < 0){
				coolDownTimer = 1f;
				coolDown = false;
			}
		}
	
	}
	//TODO 
	private void ComputerMove(){
		print ("Computer tries to make a move");

		if(CURRENT_PLAYER == PlayerColor.White){
			return;
		}
		MakeRandomMove ();

		print ("Computer has made his turn");
	}

	private void ShowHints(){
		if (!manager.UseHints) 
		{
			return;
		}
		ArrayList validMoves;
		validMoves = othelloRules.GetAllValidMoves ();
		foreach (OthelloPiece brick in bricks) {
			if(brick.brickColor == BrickColor.Hint ){
				brick.brickColor = BrickColor.Empty;
			}
		}
		foreach (OthelloPiece brick in validMoves) {
			if(brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint ){
				brick.brickColor = BrickColor.Hint;
			}
		}

	}
	private void CheckForValidMoves(){
		if (othelloRules.HasWinner()) {
			GameOver();
			return;
		}
		// No one can move
		if (!othelloRules.CanMakeMove()) {
			ChangePlayer();
			if(!othelloRules.CanMakeMove()){
				GameOver();
			}else{
				if(CURRENT_PLAYER == Othello.PlayerColor.Black ){
					showOutOfTime = false;
					cantMove.WhiteCantMove();
					if(manager.PlayAgainstComputer){
						StartCoroutine(WaitAndMakeComputerMove(1.0F));
					}
				}else{
					showOutOfTime = false;
					cantMove.BlackCantMove();
				}

			}
		}

	}

	private void GameOver(){
		hasWinner = true;
		CURRENT_PLAYER = PlayerColor.NoOne;
		gameBoard.CheckArrows (CURRENT_PLAYER);
		cantMove.NoOneCanMove(bricks);
	}

	private void ChangePlayer ()
	{
		CURRENT_PLAYER = CURRENT_PLAYER == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
		gameBoard.CheckArrows (CURRENT_PLAYER);
		if (manager.SpeedMode) {

			ResetSpeedMode();
		}
	}

	public void OutOfTime(){
		if (!showOutOfTime) {
			showOutOfTime = true;
			return;
		}

		if (!lastBrick) {
			if (CURRENT_PLAYER == PlayerColor.White) {
				cantMove.WhiteOutOfTime ();
			} else {
				cantMove.BlackOutOfTime ();
			}
			ResetSpeedMode ();
		}
		MakeRandomMove();

	}
	private void ResetSpeedMode(){
		int bricksLeft = 0;
		foreach (OthelloPiece brick in bricks) {
			if(brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint){
				bricksLeft++;
			}
		}
		if (bricksLeft == 1) {
			lastBrick = true;
		} else {
			lastBrick = false;
		}
		speedModeTimer = (float) bricksLeft;
		speedModeTimer *= 0.5f;
		speedModeTimer = Mathf.Max (_maxSpeedModeTime, speedModeTimer);
	}
}
