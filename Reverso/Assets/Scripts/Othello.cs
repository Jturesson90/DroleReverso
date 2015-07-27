using UnityEngine;
using System.Collections;

public class Othello : MonoBehaviour
{
	GameBoard gameBoard;
	OthelloRules othelloRules;
	CantMove cantMove;

	public bool canMove;
	public bool hasWinner;

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
		canMove = true;
		cantMove = GameObject.Find ("CantMovePanel").GetComponent<CantMove> ();
		CURRENT_PLAYER = PlayerColor.White;
		othelloRules = GetComponent<OthelloRules> ();
		gameBoard = GetComponent<GameBoard> ();
	}

	void Start ()
	{
		bricks = gameBoard.SetupBoard (bricks);
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Pressed (int x, int y)
	{
		HandleInput (bricks [x, y]);
	}

	private void HandleInput (OthelloPiece brick)
	{
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
					cantMove.WhiteCantMove();
				}else{
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
	}
}
