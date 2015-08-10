using UnityEngine;
using System.Collections;
using Enum = System.Enum;

public class OthelloRules : MonoBehaviour
{
	Othello othello;


	void Awake ()
	{
		othello = GetComponent<Othello> ();

	}

	public enum Direction
	{
		NW,
		N,
		NE,
		E,
		SE,
		S,
		SW,
		W
	}
	public bool HasWinner(){
		int bricksLeft = 0;
		foreach (OthelloPiece brick in othello.bricks) {
			if(brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint){
				bricksLeft++;
			}
		}
		bool hasWinner = false;
		hasWinner = bricksLeft == 0 ? true : false;
		return hasWinner;
	}
	public bool CanMakeMove(){
		
		int i = 0;
		foreach (OthelloPiece brick in othello.bricks ) {
			i++;

			if(brick.brickColor == BrickColor.Black || brick.brickColor == BrickColor.White){
				continue;
			}

			foreach (Direction direction in  Enum.GetValues(typeof(Direction))) {
				OthelloPiece nextBrick = NextBrickInDirection (brick.x, brick.y, direction);
				if (nextBrick != null) {
					if (ValidateLine (nextBrick.x, nextBrick.y, direction, 1)) {
						return true;
					}
				}
			}
		}
		return false;
	}

	public ArrayList ValidMoves (OthelloPiece brick)
	{
		//	print ("Checking Valid Moves");
		ArrayList validMoves = new ArrayList ();
		foreach (Direction direction in  Enum.GetValues(typeof(Direction))) {
			OthelloPiece nextBrick = NextBrickInDirection (brick.x, brick.y, direction);
			if (nextBrick != null) {
				//print ("valid move: X." + nextBrick.x + " Y. " + nextBrick.y + " in direction " + direction);
				if (ValidateLine (nextBrick.x, nextBrick.y, direction, 1)) {
					validMoves.Add (direction);
				}
			}
		}
		return validMoves;
	}
	bool ValidateLine (int X, int Y, Direction direction, int step)
	{
		//Here we want to get a complete Othello Line. Where
		int max = (int)Mathf.Sqrt (othello.bricks.Length);
		max--;
	
		//if outside the board return false
		if (X < 0 || X > max || Y < 0 || Y > max) {
		//	print (direction + " Next brick is OUTSIDE!");
			return false;
		}
		//if empty return false
		else if (IsEmpty (othello.bricks [X, Y])) {
			//print (direction + " Next brick is EMPTY!");
			return false;
		}
		//if has stepped over atleast 1 of opponents bricks and now finds your own color. Returns true and validates the move as a valid move. 
		else if (step > 1 && ((Othello.CURRENT_PLAYER == Othello.PlayerColor.Black && othello.bricks [X, Y].brickColor == BrickColor.Black) || (Othello.CURRENT_PLAYER == Othello.PlayerColor.White && othello.bricks [X, Y].brickColor == BrickColor.White))) {
		//	print (direction + " Next brick makes the line VALID!");
			return true;
		}
		//if first checked brick is the same color return false
		else if (step == 1 && ((Othello.CURRENT_PLAYER == Othello.PlayerColor.Black && othello.bricks [X, Y].brickColor == BrickColor.Black) || (Othello.CURRENT_PLAYER == Othello.PlayerColor.White && othello.bricks [X, Y].brickColor == BrickColor.White))) {
		//	print (direction + " Next brick on the first step is the same color!");
			return false;
		} else {
		//	print (direction + " CONTINUING for next Validation");
			OthelloPiece brick = NextBrickInDirection (X, Y, direction);
			if (brick != null) {
				step += 1;
				return ValidateLine (brick.x, brick.y, direction, step);
			} else {
		//		print (direction + " Reached the end, not a valid direction");
				return false;
			}
		}
	}
	public OthelloPiece NextBrickInDirection (int X, int row, Direction dir)
	{
		int Y = row;
		int max = (int)Mathf.Sqrt (othello.bricks.Length);
		max--;

		if ((dir == Direction.NW) && X > 0 && Y < max && !IsEmpty (othello.bricks [X - 1, Y + 1])) {
			//Found something in NW
			return othello.bricks [X - 1, Y + 1];
		} else if ((dir == Direction.N) && Y < max && !IsEmpty (othello.bricks [X, Y + 1])) {
			//Found something in N
			return othello.bricks [X, Y + 1];
		} else if ((dir == Direction.NE) && X < max && Y < max && !IsEmpty (othello.bricks [X + 1, Y + 1])) {
			//Found something in NE
			return othello.bricks [X + 1, Y + 1];
		} else if ((dir == Direction.W) && X > 0 && !IsEmpty (othello.bricks [X - 1, Y])) {
			//Found something in W
			return othello.bricks [X - 1, Y];
		} else if ((dir == Direction.E) && X < max && !IsEmpty (othello.bricks [X + 1, Y])) {
			//Found something in E
			return othello.bricks [X + 1, Y];
		} else if ((dir == Direction.SW) && X > 0 && Y > 0 && !IsEmpty (othello.bricks [X - 1, Y - 1])) {
			//Found something in SW
			return othello.bricks [X - 1, Y - 1];
		} else if ((dir == Direction.S) && Y > 0 && !IsEmpty (othello.bricks [X, Y - 1])) {
			//Found something in S"
			return othello.bricks [X, Y - 1];
		} else if ((dir == Direction.SE) && Y > 0 && X < max && !IsEmpty (othello.bricks [X + 1, Y - 1])) {
			//Found something in SE"
			return othello.bricks [X + 1, Y - 1];
		}
		return null;
	}
	public ArrayList GetAllValidMoves(){
		ArrayList validMoves = new ArrayList ();
		ArrayList validDirections;
		foreach (OthelloPiece brick in othello.bricks) {
			if(brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint){
				validDirections = ValidMoves(brick);
				if(validDirections.Count > 0){
					validMoves.Add(brick);
				}
			}
		}
		return validMoves;
	}
	public void FlashRow(OthelloPiece brick, Direction direction)
	{
		int X = brick.x;
		int Y = brick.y;
		if ((Othello.CURRENT_PLAYER == Othello.PlayerColor.White && othello.bricks [X, Y].brickColor == BrickColor.Black) || (Othello.CURRENT_PLAYER == Othello.PlayerColor.Black && othello.bricks [X, Y].brickColor == BrickColor.White)) {
			Flash (brick);
			
			brick = NextBrickInDirection (X, Y, direction);
			FlashRow (brick, direction);
		} else {
			return;
		}
	}
	public void TurnRow (OthelloPiece brick, Direction direction)
	{
		int X = brick.x;
		int Y = brick.y;
		if ((Othello.CURRENT_PLAYER == Othello.PlayerColor.White && othello.bricks [X, Y].brickColor == BrickColor.Black) || (Othello.CURRENT_PLAYER == Othello.PlayerColor.Black && othello.bricks [X, Y].brickColor == BrickColor.White)) {
			Turn (brick);
		
			brick = NextBrickInDirection (X, Y, direction);
			TurnRow (brick, direction);
		} else {
			return;
		}
	}
	public void ChosenBrick (OthelloPiece brick)
	{
		if (Othello.CURRENT_PLAYER == Othello.PlayerColor.White) {
			brick.brickColor = BrickColor.White;
		} else {
			brick.brickColor = BrickColor.Black;
		}
	}
	void Flash(OthelloPiece brick)
	{
		brick.ShouldFlash = true;	
	}
	void Turn (OthelloPiece brick)
	{
		if (brick.brickColor == BrickColor.White) {
			brick.brickColor = BrickColor.Black;
		} else if (brick.brickColor == BrickColor.Black) {
			brick.brickColor = BrickColor.White;
		}
	}
	bool IsEmpty (OthelloPiece brick)
	{
		if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint) {
			return true;
		}
		return false;
	}

	public void TurnBrick (OthelloPiece brick)
	{
		if (brick.brickColor == BrickColor.Black) {
			brick.brickColor = BrickColor.White;
		} else if (brick.brickColor == BrickColor.White) {
			brick.brickColor = BrickColor.Black;
		}
	}

	public void ReleaseAllFlashes(){
		foreach (OthelloPiece brick in othello.bricks) {
			if(IsColored(brick)){
				brick.ShouldFlash = false;
			}
		}
	}

	private bool IsColored(OthelloPiece brick){
		if (brick.brickColor == BrickColor.Black || brick.brickColor == BrickColor.White) {
			return true;
		}
		return false;
	}
}
