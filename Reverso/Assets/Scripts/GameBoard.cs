using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{



	public GameObject brick;




	private Text blackText, whiteText, blackText2, whiteText2;

	public GameObject whiteArrow;
	public GameObject blackArrow;




	void Awake ()
	{
		GameObject[] whiteTexts = GameObject.FindGameObjectsWithTag ("WhiteText");
		GameObject[] blackTexts = GameObject.FindGameObjectsWithTag ("BlackText");

		whiteText = whiteTexts [0].GetComponent<Text> ();
		whiteText2 = whiteTexts [1].GetComponent<Text> ();

		blackText = blackTexts [0].GetComponent<Text> ();
		blackText2 = blackTexts [1].GetComponent<Text> ();


	}
	void Start ()
	{

	}
	
	Transform selected;
	
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) { // if left button pressed...
			CastDownRay ();
		
		}
		if (Input.GetMouseButtonUp (0)) {
			CastUpRay ();
		}

	}
	void CastUpRay ()
	{
		#if UNITY_EDITOR
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		#elif UNITY_ANDROID || UNITY_IOS 
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		#else
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		#endif

		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (selected == hit.transform) {
				OthelloPiece op = hit.transform.GetComponent<OthelloPiece> ();
				if (op && op.brickColor == BrickColor.Empty) {
					GetComponent<Othello> ().Pressed (op.x, op.y);
				}
			} else {
			
			}
		}
	} 
	void CastDownRay ()
	{
		#if UNITY_EDITOR
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		#elif UNITY_ANDROID || UNITY_IOS
		Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
		#else
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		#endif
		if (!GetComponent<Othello> ().canMove) {
			return;
		}
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.tag == "GamePiece") {
				selected = hit.transform;

			}

		
		}
	} 

	public OthelloPiece[,] SetupBoard (OthelloPiece[,] bricks)
	{
		CheckArrows (Othello.CURRENT_PLAYER);

		bricks = new OthelloPiece[8, 8];
		for (int y = 0; y < 8; y++) {
			for (int x = 0; x < 8; x++) {
				GameObject piece = (GameObject)Instantiate (brick, new Vector3 (x, y, -0.4f), Quaternion.identity);
				piece.transform.parent = transform;
				OthelloPiece othello = piece.GetComponent<OthelloPiece> ();
				othello.brickColor = BrickColor.Empty;
				othello.x = x;
				othello.y = y;
				bricks [x, y] = othello;
			}
		}

		bricks [3, 4].brickColor = BrickColor.White;
		bricks [4, 3].brickColor = BrickColor.White;
		bricks [3, 3].brickColor = BrickColor.Black;
		bricks [4, 4].brickColor = BrickColor.Black;
		UpdateBoard (bricks);
		return bricks;
	}



	public void CheckArrows (Othello.PlayerColor playerColor)
	{	

		if (playerColor == Othello.PlayerColor.White) {
			whiteArrow.SetActive (true);
			blackArrow.SetActive (false);
		} else if (playerColor == Othello.PlayerColor.Black) {
			whiteArrow.SetActive (false);
			blackArrow.SetActive (true);
		} else {
			whiteArrow.SetActive (false);
			blackArrow.SetActive (false);
		}
	}
	public void UpdateBoard (OthelloPiece[,] bricks)
	{
		int blacks = 0;
		int whites = 0;
		int sumLeft = 64;
		foreach (OthelloPiece i in bricks) {
			if (i.brickColor == BrickColor.Black) {
				blacks++;
				sumLeft--;
			} else if (i.brickColor == BrickColor.White) {
				whites++;
				sumLeft--;
			}
		}
		UpdateLabels (blacks, whites);
		if (sumLeft <= 0) {
			//Application.LoadLevel (Application.loadedLevel);
		}


	}
	public void UpdateLabels (int blacks, int whites)
	{

		whiteText.text = "Whites - " + whites;
		whiteText2.text = "Whites - " + whites;
		blackText.text = blacks + " - Blacks";
		blackText2.text = blacks + " - Blacks";
	}
}
