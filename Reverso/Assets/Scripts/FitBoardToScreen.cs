using UnityEngine;
using System.Collections;

public class FitBoardToScreen : MonoBehaviour
{
	public float widthToBeSeen = 8f;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Camera.main.orthographicSize = widthToBeSeen * Screen.height / Screen.width * 0.5f;
	}
}
