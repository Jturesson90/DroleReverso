using UnityEngine;
using System.Collections;

public class FitBoardToScreen : MonoBehaviour
{
	public float minWidthToBeSeen = 8f;
    public float MaxWidthToBeSeen = 9f;
	// Use this for initialization
	void Start ()
	{
   
        if ((float)Screen.width / (float)Screen.height > 0.7f) {
            minWidthToBeSeen = MaxWidthToBeSeen;
        }
        Camera.main.orthographicSize = minWidthToBeSeen * Screen.height / Screen.width * 0.5f;
    }
	
}
