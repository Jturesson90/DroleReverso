using UnityEngine;
using System.Collections;

public class Othello : MonoBehaviour
{
	GameBoard gameBoard;
	OthelloRules othelloRules;

	public enum PlayerColor
	{
		White,
		Black
	}

	public static PlayerColor CURRENT_PLAYER = PlayerColor.White;

	public OthelloPiece[,] bricks;



	void Awake ()
	{
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
		}
	}
	private void ChangePlayer ()
	{
		CURRENT_PLAYER = CURRENT_PLAYER == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
		gameBoard.CheckArrows (CURRENT_PLAYER);
	}
}
