using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class OthelloRulesTestGround
{






    public struct Brick
    {
        public BrickColor brickColor;
        public Position position;
        public Brick(Position pos, BrickColor bc)
        {
            position = pos;
            brickColor = bc;
        }
    }
    public struct Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static OthelloPiece GetMoveWithLeastOpponentMoves(OthelloPiece[,] bricks, List<OthelloPiece> possibleMoves, Othello.PlayerColor currentColor)
    {
        var chosenBrick = new Brick(new Position(0, 0), BrickColor.Empty);

        var opponentColor = currentColor == Othello.PlayerColor.Black ? Othello.PlayerColor.White : Othello.PlayerColor.Black;


        var reservIndexex = new List<int>();
        var bestNumberOfOpponentReserveMoves = int.MaxValue;
        var chosenIndexex = new List<int>();
        var index = 0;
        int bestNumberOfOppenentMoves = int.MaxValue;
        foreach (var brick in possibleMoves)
        {
            var board = MakeCopyOfBricks(bricks);
            Brick tempBrick = board[brick.x, brick.y];
            MakeMove(ref board, tempBrick, currentColor);
            List<Brick> allValidMoves = GetAllValidMoves(board, opponentColor);
            bool opponentCanTakeCorner = false;
            opponentCanTakeCorner = CheckIfOpponentCanTakeACorner(allValidMoves);

            if (opponentCanTakeCorner)
            {
                int numberOfReservMoves = allValidMoves.Count;

                if (numberOfReservMoves == bestNumberOfOpponentReserveMoves)
                {
                    reservIndexex.Add(index);
                }
                else if (numberOfReservMoves < bestNumberOfOpponentReserveMoves)
                {
                    bestNumberOfOpponentReserveMoves = numberOfReservMoves;
                    reservIndexex.Clear();
                    reservIndexex.Add(index);
                }
                index++;
                Debug.Log("If I take " + brick.x + " , " + brick.y + " you will have " + numberOfReservMoves + " number of possible moves. But you can take the corner");
                continue;
            }

            int numberOfOpponentMoves = allValidMoves.Count;

            Debug.Log("If I take " + brick.x + " , " + brick.y + " you will have " + numberOfOpponentMoves + " number of possible moves.");

            if (numberOfOpponentMoves == bestNumberOfOppenentMoves)
            {
                chosenIndexex.Add(index);
            }
            else if (numberOfOpponentMoves < bestNumberOfOppenentMoves)
            {
                bestNumberOfOppenentMoves = numberOfOpponentMoves;
                chosenIndexex.Clear();
                chosenIndexex.Add(index);
            }


            index++;
        }

        Debug.Log("I got " + chosenIndexex.Count + " best moves to choose from.");
        int chosenIndex = 0;
        if (chosenIndexex.Count > 0)
        {
            chosenIndex = chosenIndexex[UnityEngine.Random.Range(0, chosenIndexex.Count - 1)];
            Debug.Log("Sorry, but I chose " + possibleMoves[chosenIndex].x + " , " + possibleMoves[chosenIndex].y + " for you to only have " + bestNumberOfOppenentMoves + " moves next turn");
        }
        else
        {
            chosenIndex = reservIndexex[UnityEngine.Random.Range(0, reservIndexex.Count - 1)];
            Debug.Log("Sorry, but I chose " + possibleMoves[chosenIndex].x + " , " + possibleMoves[chosenIndex].y + " for you to only have " + bestNumberOfOppenentMoves + " moves next turn. But you can take the corner....");

        }
        return possibleMoves[chosenIndex];
    }

    private static bool CheckIfOpponentCanTakeACorner(List<Brick> allValidMoves)
    {
        foreach (Brick item in allValidMoves)
        {
            if (IsCorner(item))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsCorner(Brick item)
    {
        var x = item.position.x;
        var y = item.position.y;

        if (x == 0 && y == 0) return true;
        if (x == GameBoard.BoardWidth - 1 && y == 0) return true;
        if (x == 0 && y == GameBoard.BoardWidth - 1) return true;
        if (x == GameBoard.BoardWidth - 1 && y == GameBoard.BoardWidth - 1) return true;

        return false;

    }

    private static void MakeMove(ref Brick[,] board, Brick brick, Othello.PlayerColor currentColor)
    {
        List<Direction> validDirections;
        validDirections = GetValidDirections(board, brick, currentColor);
        if (validDirections.Count > 0)
        {
            PutDownBrick(ref board, brick, currentColor);
            for (int i = 0; i < validDirections.Count; i++)
            {
                Direction direction = validDirections[i];
                Brick? tempNextBrick = NextBrickInDirection(brick.position.x, brick.position.y, direction, board);
                if (tempNextBrick.HasValue)
                {
                    var nextBrick = tempNextBrick.Value;
                    TurnRow(nextBrick, direction, ref board, currentColor);
                }
            }

        }
    }
    public static void PutDownBrick(ref Brick[,] board, Brick brick, Othello.PlayerColor currentPlayer)
    {
        board[brick.position.x, brick.position.y].brickColor = currentPlayer ==
            Othello.PlayerColor.White ? board[brick.position.x, brick.position.y].brickColor = BrickColor.White : brick.brickColor = BrickColor.Black;

    }

    public static List<Direction> GetValidDirections(Brick[,] bricks, Brick brick, Othello.PlayerColor currentPlayer)
    {
        var pos = brick.position;

        var validDirections = new List<Direction>();
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Brick? tempNextBrick = NextBrickInDirection(pos.x, pos.y, direction, bricks);

            if (tempNextBrick.HasValue)
            {
                var nextBrick = (Brick)tempNextBrick.Value;
                var nextPos = nextBrick.position;
                if (ValidateLine(nextPos.x, nextPos.y, direction, 1, bricks, currentPlayer))
                {
                    validDirections.Add(direction);
                }
            }
        }
        return validDirections;
    }

    public static List<Brick> GetAllValidMoves(Brick[,] board, Othello.PlayerColor currentPlayer)
    {
        var validMoves = new List<Brick>();
        List<Direction> validDirections;
        foreach (Brick brick in board)
        {
            if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
            {

                validDirections = GetValidDirections(board, brick, currentPlayer);
                if (validDirections.Count > 0)
                {
                    validMoves.Add(brick);
                }
            }
        }
        return validMoves;
    }
    static bool ValidateLine(int x, int y, Direction direction, int step, Brick[,] board, Othello.PlayerColor currentPlayer)
    {
        //Here we want to get a complete Othello Line. Where
        int max = board.GetLength(0) - 1;
        //if outside the board return false
        if (x < 0 || x > max || y < 0 || y > max)
        {
            //	print (direction + " Next brick is OUTSIDE!");
            return false;
        }
        //if empty return false
        else if (IsEmpty(board[x, y]))
        {
            //print (direction + " Next brick is EMPTY!");
            return false;
        }
        //if has stepped over atleast 1 of opponents bricks and now finds your own color. Returns true and validates the move as a valid move. 
        else if (step > 1 && ((currentPlayer == Othello.PlayerColor.Black && board[x, y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.White && board[x, y].brickColor == BrickColor.White)))
        {
            //	print (direction + " Next brick makes the line VALID!");
            return true;
        }
        //if first checked brick is the same color return false
        else if (step == 1 && ((currentPlayer == Othello.PlayerColor.Black && board[x, y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.White && board[x, y].brickColor == BrickColor.White)))
        {
            //	print (direction + " Next brick on the first step is the same color!");
            return false;
        }
        else
        {
            //	print (direction + " CONTINUING for next Validation");
            Brick? tempBrick = NextBrickInDirection(x, y, direction, board);
            if (tempBrick.HasValue)
            {
                Brick brick = tempBrick.Value;
                var brickPos = brick.position;
                step += 1;
                return ValidateLine(brickPos.x, brickPos.y, direction, step, board, currentPlayer);
            }
            else
            {
                return false;
            }
        }
    }
    public static Brick? NextBrickInDirection(int x, int y, Direction dir, Brick[,] board)
    {

        int max = board.GetLength(0) - 1;


        if ((dir == Direction.NW) && x > 0 && y < max && !IsEmpty(board[x - 1, y + 1]))
        {
            //Found something in NW
            return board[x - 1, y + 1];
        }
        else if ((dir == Direction.N) && y < max && !IsEmpty(board[x, y + 1]))
        {
            //Found something in N
            return board[x, y + 1];
        }
        else if ((dir == Direction.NE) && x < max && y < max && !IsEmpty(board[x + 1, y + 1]))
        {
            //Found something in NE
            return board[x + 1, y + 1];
        }
        else if ((dir == Direction.W) && x > 0 && !IsEmpty(board[x - 1, y]))
        {
            //Found something in W
            return board[x - 1, y];
        }
        else if ((dir == Direction.E) && x < max && !IsEmpty(board[x + 1, y]))
        {
            //Found something in E
            return board[x + 1, y];
        }
        else if ((dir == Direction.SW) && x > 0 && y > 0 && !IsEmpty(board[x - 1, y - 1]))
        {
            //Found something in SW
            return board[x - 1, y - 1];
        }
        else if ((dir == Direction.S) && y > 0 && !IsEmpty(board[x, y - 1]))
        {
            //Found something in S"
            return board[x, y - 1];
        }
        else if ((dir == Direction.SE) && y > 0 && x < max && !IsEmpty(board[x + 1, y - 1]))
        {
            //Found something in SE"
            return board[x + 1, y - 1];
        }
        return null;
    }
    static bool IsEmpty(Brick brick)
    {
        if (brick.brickColor == BrickColor.Empty || brick.brickColor == BrickColor.Hint)
        {
            return true;
        }
        return false;
    }
    public static void TurnRow(Brick brick, Direction direction, ref Brick[,] board, Othello.PlayerColor currentPlayer)
    {
        int x = brick.position.x;
        int y = brick.position.y;
        if ((currentPlayer == Othello.PlayerColor.White && board[x, y].brickColor == BrickColor.Black) || (currentPlayer == Othello.PlayerColor.Black && board[x, y].brickColor == BrickColor.White))
        {
            Turn(ref board, brick);
            var tempBrick = NextBrickInDirection(x, y, direction, board);
            if (tempBrick.HasValue)
            {
                brick = tempBrick.Value;
            }
            TurnRow(brick, direction, ref board, currentPlayer);
        }
        else
        {
            return;
        }
    }
    static void Turn(ref Brick[,] board, Brick brick)
    {
        var x = brick.position.x;
        var y = brick.position.y;
        if (brick.brickColor == BrickColor.White)
        {
            board[x, y].brickColor = BrickColor.Black;
        }
        else if (brick.brickColor == BrickColor.Black)
        {
            board[x, y].brickColor = BrickColor.White;
        }
    }
    private static Brick[,] MakeCopyOfBricks(OthelloPiece[,] bricks)
    {
        var bricksWidth = bricks.GetLength(0);
        var bricksHeight = bricks.GetLength(1);

        Brick[,] board = new Brick[bricksWidth, bricksHeight];

        for (int y = 0; y < bricksHeight; y++)
        {
            for (int x = 0; x < bricksWidth; x++)
            {
                var brick = bricks[x, y];
                board[x, y] = new Brick(new Position(brick.x, brick.y), brick.brickColor);
            }
        }

        return board;
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
}
