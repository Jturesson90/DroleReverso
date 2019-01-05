using UnityEngine;
using System.Collections;
using Enum = System.Enum;

public static class OthelloRules
{
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

    public static bool HasWinner(OthelloPiece[,] bricks)
    {
        int bricksLeft = 0;
        foreach (OthelloPiece brick in bricks)
        {
            if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
            {
                bricksLeft++;
            }
        }
        bool hasWinner = false;
        hasWinner = bricksLeft == 0;

        return hasWinner;
    }

    public static bool CanMakeMove(OthelloPiece[,] bricks, Othello.PlayerColor currentPlayer)
    {

        int i = 0;
        foreach (OthelloPiece brick in bricks)
        {
            i++;

            if (brick.brickColor == BrickColor.Black || brick.brickColor == BrickColor.White)
            {
                continue;
            }

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                OthelloPiece nextBrick = NextBrickInDirection(brick.x, brick.y, direction, bricks);
                if (nextBrick != null)
                {
                    if (ValidateLine(nextBrick.x, nextBrick.y, direction, 1, bricks, currentPlayer))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static ArrayList GetValidDirections(OthelloPiece[,] bricks, OthelloPiece brick, Othello.PlayerColor currentPlayer)
    {

        //	print ("Checking Valid Moves");
        ArrayList validDirections = new ArrayList();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            OthelloPiece nextBrick = NextBrickInDirection(brick.x, brick.y, direction, bricks);
            if (nextBrick != null)
            {
                //print ("valid move: X." + nextBrick.x + " Y. " + nextBrick.y + " in direction " + direction);
                if (ValidateLine(nextBrick.x, nextBrick.y, direction, 1, bricks, currentPlayer))
                {
                    validDirections.Add(direction);
                }
            }
        }
        return validDirections;
    }
    static bool ValidateLine(int X, int Y, Direction direction, int step, OthelloPiece[,] bricks, Othello.PlayerColor currentPlayer)
    {
        //Here we want to get a complete Othello Line. Where
        int max = (int)Mathf.Sqrt(bricks.Length);
        max--;

        //if outside the board return false
        if (X < 0 || X > max || Y < 0 || Y > max)
        {
            //	print (direction + " Next brick is OUTSIDE!");
            return false;
        }
        //if empty return false
        else if (IsEmpty(bricks[X, Y]))
        {
            //print (direction + " Next brick is EMPTY!");
            return false;
        }
        //if has stepped over atleast 1 of opponents bricks and now finds your own color. Returns true and validates the move as a valid move. 
        else if (step > 1 && ((currentPlayer == Othello.PlayerColor.Black && bricks[X, Y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.White && bricks[X, Y].brickColor == BrickColor.White)))
        {
            //	print (direction + " Next brick makes the line VALID!");
            return true;
        }
        //if first checked brick is the same color return false
        else if (step == 1 && ((currentPlayer == Othello.PlayerColor.Black && bricks[X, Y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.White && bricks[X, Y].brickColor == BrickColor.White)))
        {
            //	print (direction + " Next brick on the first step is the same color!");
            return false;
        }
        else
        {
            //	print (direction + " CONTINUING for next Validation");
            OthelloPiece brick = NextBrickInDirection(X, Y, direction, bricks);
            if (brick != null)
            {
                step += 1;
                return ValidateLine(brick.x, brick.y, direction, step, bricks, currentPlayer);
            }
            else
            {
                //		print (direction + " Reached the end, not a valid direction");
                return false;
            }
        }
    }
    public static OthelloPiece NextBrickInDirection(int X, int row, Direction dir, OthelloPiece[,] bricks)
    {
        int Y = row;
        int max = (int)Mathf.Sqrt(bricks.Length);
        max--;

        if ((dir == Direction.NW) && X > 0 && Y < max && !IsEmpty(bricks[X - 1, Y + 1]))
        {
            //Found something in NW
            return bricks[X - 1, Y + 1];
        }
        else if ((dir == Direction.N) && Y < max && !IsEmpty(bricks[X, Y + 1]))
        {
            //Found something in N
            return bricks[X, Y + 1];
        }
        else if ((dir == Direction.NE) && X < max && Y < max && !IsEmpty(bricks[X + 1, Y + 1]))
        {
            //Found something in NE
            return bricks[X + 1, Y + 1];
        }
        else if ((dir == Direction.W) && X > 0 && !IsEmpty(bricks[X - 1, Y]))
        {
            //Found something in W
            return bricks[X - 1, Y];
        }
        else if ((dir == Direction.E) && X < max && !IsEmpty(bricks[X + 1, Y]))
        {
            //Found something in E
            return bricks[X + 1, Y];
        }
        else if ((dir == Direction.SW) && X > 0 && Y > 0 && !IsEmpty(bricks[X - 1, Y - 1]))
        {
            //Found something in SW
            return bricks[X - 1, Y - 1];
        }
        else if ((dir == Direction.S) && Y > 0 && !IsEmpty(bricks[X, Y - 1]))
        {
            //Found something in S"
            return bricks[X, Y - 1];
        }
        else if ((dir == Direction.SE) && Y > 0 && X < max && !IsEmpty(bricks[X + 1, Y - 1]))
        {
            //Found something in SE"
            return bricks[X + 1, Y - 1];
        }
        return null;
    }
    public static ArrayList GetAllValidMoves(OthelloPiece[,] bricks, Othello.PlayerColor currentPlayer)
    {
        ArrayList validMoves = new ArrayList();
        ArrayList validDirections;
        foreach (OthelloPiece brick in bricks)
        {
            if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
            {
                validDirections = GetValidDirections(bricks, brick, currentPlayer);
                if (validDirections.Count > 0)
                {
                    validMoves.Add(brick);
                }
            }
        }
        return validMoves;
    }


    public static void FlashRow(OthelloPiece brick, Direction direction, OthelloPiece[,] bricks, Othello.PlayerColor currentPlayer)
    {
        int X = brick.x;
        int Y = brick.y;
        if ((currentPlayer == Othello.PlayerColor.White && bricks[X, Y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.Black && bricks[X, Y].brickColor == BrickColor.White))
        {
            Flash(brick);

            brick = NextBrickInDirection(X, Y, direction, bricks);
            FlashRow(brick, direction, bricks, currentPlayer);
        }
        else
        {
            return;
        }
    }
    public static void TurnRow(OthelloPiece brick, Direction direction, OthelloPiece[,] bricks, Othello.PlayerColor currentPlayer)
    {
        int X = brick.x;
        int Y = brick.y;
        if ((currentPlayer == Othello.PlayerColor.White && bricks[X, Y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.Black && bricks[X, Y].brickColor == BrickColor.White))
        {
            Turn(brick);
            brick = NextBrickInDirection(X, Y, direction, bricks);
            TurnRow(brick, direction, bricks, currentPlayer);
        }
        else
        {
            return;
        }
    }
    public static void PutDownBrick(OthelloPiece brick, Othello.PlayerColor currentPlayer)
    {
        if (currentPlayer == Othello.PlayerColor.White)
        {
            brick.brickColor = BrickColor.White;
        }
        else
        {
            brick.brickColor = BrickColor.Black;

        }
    }
    static void Flash(OthelloPiece brick)
    {
        brick.ShouldFlash = true;
    }
    static void Turn(OthelloPiece brick)
    {
        if (brick.brickColor == BrickColor.White)
        {
            brick.brickColor = BrickColor.Black;
        }
        else if (brick.brickColor == BrickColor.Black)
        {
            brick.brickColor = BrickColor.White;
        }
    }
    static bool IsEmpty(OthelloPiece brick)
    {
        if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
        {
            return true;
        }
        return false;
    }

    static public void TurnBrick(OthelloPiece brick)
    {
        if (brick.brickColor == BrickColor.Black)
        {
            brick.brickColor = BrickColor.White;
        }
        else if (brick.brickColor == BrickColor.White)
        {
            brick.brickColor = BrickColor.Black;
        }
    }

    static public void ReleaseAllFlashes(OthelloPiece[,] bricks)
    {
        foreach (OthelloPiece brick in bricks)
        {
            if (IsColored(brick))
            {
                brick.ShouldFlash = false;
            }
        }
    }

    static bool IsColored(OthelloPiece brick)
    {
        if (brick.brickColor == BrickColor.Black || brick.brickColor == BrickColor.White)
        {
            return true;
        }
        return false;
    }
}
