using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CantMove : MonoBehaviour {

	Animator anim;
	Othello othello;
	
	private const string TRIGGER_SWIPE_OUT = "SwipeOut";
	private const string TRIGGER_SWIPE_IN = "SwipeIn";
	Text cantMoveText;

	void Awake(){
		anim = GetComponent<Animator> ();
		othello = GameObject.Find ("Othello").GetComponent<Othello> ();
		cantMoveText = GameObject.Find ("CantMoveText").GetComponent<Text> ();

	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void WhiteCantMove(){
		print ("WhiteCantMove");
		othello.canMove = false;
		cantMoveText.text = "White cant move!\nBlack's turn"; 
		anim.SetTrigger (TRIGGER_SWIPE_IN);

	}
	public void BlackCantMove(){
		print ("BlackCantMove");
		othello.canMove = false;
		cantMoveText.text = "Black cant move!\nWhite's turn"; 
		anim.SetTrigger (TRIGGER_SWIPE_IN);
	}

	public void NoOneCanMove(OthelloPiece[,] bricks){
		print ("NoOneCanMove");
		othello.canMove = false;
		int whites = 0;
		int blacks = 0;
		string winText = "Congratulations ";
		foreach (OthelloPiece brick in bricks) {
			if(brick.brickColor == BrickColor.White){
				whites++;
			}else if(brick.brickColor == BrickColor.Black){
				blacks++;
			}
		}
		if (whites > blacks) {
			winText += "White!";
		} else if (blacks > whites) {
			winText += "Black!";
		} else {
			winText = "DRAW!";
		}

		cantMoveText.text = winText; 
		anim.SetTrigger (TRIGGER_SWIPE_IN);
	}
	public void OKPressed(){
		othello.canMove = true;
		anim.SetTrigger (TRIGGER_SWIPE_OUT);
	}
	public void AnimationSwipeOutCallback(){
		if (othello.hasWinner) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
