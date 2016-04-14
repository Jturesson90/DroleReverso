using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class ComputerAI
{

    public static OthelloPiece GetMove(OthelloPiece[,] bricks)
    {
        switch (OthelloManager.Instance.ComputerLevel)
        {
            case OthelloManager.ComputerLevelEnum.One:
                return LevelOneMove(bricks);
            case OthelloManager.ComputerLevelEnum.Two:
                return LevelTwoMove(bricks);
            case OthelloManager.ComputerLevelEnum.Three:
                return LevelThreeMove(bricks);
            case OthelloManager.ComputerLevelEnum.Four:
                return LevelFourMove(bricks);
            case OthelloManager.ComputerLevelEnum.Five:
                return LevelFiveMove(bricks);
        }
        return null;
    }

    private static OthelloPiece LevelOneMove(OthelloPiece[,] bricks)
    {
        Debug.Log("Computer Level 1 Move");
        return GetRandomMove(bricks);
    }


    private static OthelloPiece LevelTwoMove(OthelloPiece[,] bricks)
    {
        Debug.Log("Computer Level 2 Move");
        var corner = GetRandomValidCorner(bricks);
        if (corner != null) return corner;
        return GetRandomMove(bricks);
    }

    private static OthelloPiece LevelThreeMove(OthelloPiece[,] bricks)
    {
        Debug.Log("Computer Level 3 Move");
        var corner = GetRandomValidCorner(bricks);
        if (corner != null) return corner;
        return GetRandomMove(bricks);
    }
    private static OthelloPiece LevelFourMove(OthelloPiece[,] bricks)
    {
        throw new NotImplementedException();
    }
    private static OthelloPiece LevelFiveMove(OthelloPiece[,] bricks)
    {
        throw new NotImplementedException();
    }
    private static OthelloPiece GetRandomMove(OthelloPiece[,] bricks)
    {
        var validMoves = OthelloRules.GetAllValidMoves(bricks);
        if (validMoves.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, validMoves.Count - 1);
            return validMoves[index] as OthelloPiece;
        }
        return null;
    }
    //Highly value taking corner fields
    static List<OthelloPiece> GetValidCorners(OthelloPiece[,] bricks)
    {
        List<OthelloPiece> validCorners = new List<OthelloPiece>();
        var validMoves = OthelloRules.GetAllValidMoves(bricks);
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
        if (validCorners.Count > 0)
        {
            return validCorners[UnityEngine.Random.Range(0, validCorners.Count - 1)];
        }
        else
        {
            return null;
        }

    }

    //Highly penalize taking the fields next to the corners
    //Value other border tiles higher than remaining tiles
    //Try to minimize the number of moves the opponent can make
}
