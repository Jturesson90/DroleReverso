using UnityEngine;
using System.Collections;
using System;
public class Board : ICloneable
{

    private OthelloPiece[,] _bricks;

    public OthelloPiece[,] Bricks { get { return _bricks; } set { _bricks = value; } }

    public object Clone()
    {

        return MemberwiseClone();
    }
}
