using UnityEngine;
using System.Collections;

public static class OthelloPieceExtension
{

    public static byte[] ToByteArray(this OthelloPiece[,] bricks)
    {
        byte[] returnValue = new byte[64];
        int listItem = 0;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                returnValue[listItem] = (byte)bricks[x, y].brickColor;
                listItem++;
            }
        }
        return returnValue;
    }
    public static OthelloPiece[,] ToOthelloPieceArray(this byte[] byteArray, OthelloPiece[,] bricks)
    {

        int length = (int)Mathf.Sqrt(byteArray.Length);
        var index = 0;
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
            {
                bricks[x, y].brickColor = (BrickColor)byteArray[index];
                index++;
            }
        }
        return bricks;
    }
    public static string ToMinutesAndSeconds(this float time)
    {
        float minutes = Mathf.Floor(time / 60f);
        float seconds = Mathf.Floor(time - minutes * 60);

        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

}
