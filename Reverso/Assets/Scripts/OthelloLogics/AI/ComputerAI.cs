using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class ComputerAI
{

    public static OthelloPiece GetMove(OthelloPiece[,] bricks, OthelloManager.ComputerLevelEnum computerLevel)
    {
        ArrayList possibleMovesTemp = OthelloRules.GetAllValidMoves(bricks, Othello.CURRENT_PLAYER);
        var possibleMoves = new List<OthelloPiece>();
        foreach (OthelloPiece temp in possibleMovesTemp)
        {
            possibleMoves.Add(temp);
        }
        if (possibleMoves.Count < 1) return null;
        switch (computerLevel)
        {
            case OthelloManager.ComputerLevelEnum.One:
                return LevelOneMove(bricks, possibleMoves);
            case OthelloManager.ComputerLevelEnum.Two:
                return LevelTwoMove(bricks, possibleMoves);
            case OthelloManager.ComputerLevelEnum.Three:
                return LevelThreeMove(bricks, possibleMoves);
            case OthelloManager.ComputerLevelEnum.Four:
                return LevelFourMove(bricks, possibleMoves);
            case OthelloManager.ComputerLevelEnum.Five:
                return LevelFiveMove(bricks, possibleMoves);
        }
        return null;
    }

    private static OthelloPiece LevelOneMove(OthelloPiece[,] bricks, List<OthelloPiece> possibleMoves)
    {
        Debug.Log("Computer Level 1 Move");
        return GetRandomMove(bricks);
    }


    private static OthelloPiece LevelTwoMove(OthelloPiece[,] bricks, List<OthelloPiece> possibleMoves)
    {
        Debug.Log("Computer Level 2 Move");
        var corner = GetRandomValidCorner(bricks);
        if (corner != null) return corner;
        return GetRandomMove(bricks);
    }

    private static OthelloPiece LevelThreeMove(OthelloPiece[,] bricks, List<OthelloPiece> possibleMoves)
    {
        Debug.Log("Computer Level 3 Move");

        var corner = GetRandomValidCorner(bricks);
        if (corner != null) return corner;
        //Highly penalize taking the fields next to the corners
        possibleMoves = RemoveFieldsNextToCorners(possibleMoves);

        return possibleMoves.Count > 0 ? GetRandomMove(possibleMoves) : GetRandomMove(bricks);
    }
    private static OthelloPiece LevelFourMove(OthelloPiece[,] bricks, List<OthelloPiece> possibleMoves)
    {
        Debug.Log("Computer Level 4 Move");
        var corner = GetRandomValidCorner(bricks);
        if (corner != null) return corner;
        //possibleMoves = RemoveFieldsNextToCorners(possibleMoves);
        //OthelloPiece bestChoice = GetMoveWithLeastOpponentMoves(bricks, possibleMoves);

        var computerColor = OthelloManager.Instance.PlayerColor == Othello.PlayerColor.White ? Othello.PlayerColor.Black : Othello.PlayerColor.White;

        return OthelloRulesTestGround.GetMoveWithLeastOpponentMoves(bricks, possibleMoves, computerColor);

    }


    private static OthelloPiece LevelFiveMove(OthelloPiece[,] bricks, List<OthelloPiece> possibleMoves)
    {
        throw new NotImplementedException();
    }



    private static List<OthelloPiece> RemoveFieldsNextToCorners(List<OthelloPiece> possibleMoves)
    {
        int firstSize = possibleMoves.Count;

        possibleMoves.RemoveAll(elem => isNearLeftDownCorner(elem));
        possibleMoves.RemoveAll(elem => isNearRightDownCorner(elem));
        possibleMoves.RemoveAll(elem => isNearRightUpCorner(elem));
        possibleMoves.RemoveAll(elem => isNearLeftUpCorner(elem));

        Debug.Log("Removed " + (firstSize - possibleMoves.Count) + "pieces near corners from validMoves");
        return possibleMoves;
    }
    #region NearCorners
    private static bool isNearLeftUpCorner(OthelloPiece brick)
    {
        return (
           brick.x == 0 && brick.y == GameBoard.BoardWidth - 2 ||
           brick.x == 1 && brick.y == GameBoard.BoardWidth - 2 ||
           brick.x == 1 && brick.y == GameBoard.BoardWidth - 1
            );
    }

    private static bool isNearRightUpCorner(OthelloPiece brick)
    {
        return (
           brick.x == GameBoard.BoardWidth - 2 && brick.y == GameBoard.BoardWidth - 2 ||
           brick.x == GameBoard.BoardWidth - 1 && brick.y == GameBoard.BoardWidth - 2 ||
           brick.x == GameBoard.BoardWidth - 2 && brick.y == GameBoard.BoardWidth - 1
            );
    }

    private static bool isNearRightDownCorner(OthelloPiece brick)
    {
        return (
           brick.x == GameBoard.BoardWidth - 2 && brick.y == 0 ||
           brick.x == GameBoard.BoardWidth - 1 && brick.y == 1 ||
           brick.x == GameBoard.BoardWidth - 2 && brick.y == 1
            );
    }

    private static bool isNearLeftDownCorner(OthelloPiece brick)
    {
        return (
           brick.x == 1 && brick.y == 0 ||
           brick.x == 1 && brick.y == 1 ||
           brick.x == 0 && brick.y == 1
            );
    }

    #endregion
    private static OthelloPiece GetRandomMove(OthelloPiece[,] bricks)
    {
        var validMoves = OthelloRules.GetAllValidMoves(bricks, Othello.CURRENT_PLAYER);
        if (validMoves.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, validMoves.Count - 1);
            return validMoves[index] as OthelloPiece;
        }
        return null;
    }
    private static OthelloPiece GetRandomMove(List<OthelloPiece> possibleMoves)
    {
        if (possibleMoves.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, possibleMoves.Count - 1);
            return possibleMoves[index] as OthelloPiece;
        }
        return null;
    }
    //Highly value taking corner fields
    static List<OthelloPiece> GetValidCorners(OthelloPiece[,] bricks)
    {
        List<OthelloPiece> validCorners = new List<OthelloPiece>();
        var validMoves = OthelloRules.GetAllValidMoves(bricks, Othello.CURRENT_PLAYER);
        if (validMoves.Count < 1) return null;

        foreach (OthelloPiece piece in validMoves)
        {
            if (piece.x == 0)
            {
                if (piece.y == 0 || piece.y == GameBoard.BoardWidth - 1)
                {
                    validCorners.Add(piece);
                }

            }
            else if (piece.x == GameBoard.BoardWidth - 1)
            {
                if (piece.y == 0 || piece.y == GameBoard.BoardWidth - 1)
                {
                    validCorners.Add(piece);
                }
            }
        }
        return validCorners;
    }
    static OthelloPiece GetRandomValidCorner(OthelloPiece[,] bricks)
    {
        var validCorners = GetValidCorners(bricks);
        return validCorners.Count > 0 ? validCorners[UnityEngine.Random.Range(0, validCorners.Count - 1)] : null;
    }

    //Highly penalize taking the fields next to the corners
    //Value other border tiles higher than remaining tiles
    //Try to minimize the number of moves the opponent can make
}
