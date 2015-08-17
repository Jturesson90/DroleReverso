﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CantMove : MonoBehaviour {

	Animator anim;
	Othello othello;


	private const string TRIGGER_SWIPE_OUT = "SwipeOut";
	private const string TRIGGER_SWIPE_IN = "SwipeIn";
	Text cantMoveText;
	RectTransform parentRectTransform;

	Vector3 forWhite = new Vector3 (0f,0f,0f);
	Vector3 forBlack = new Vector3 (0f,0f,180f);

	void Awake(){
		anim = GetComponent<Animator> ();
		othello = GameObject.Find ("Othello").GetComponent<Othello> ();
		cantMoveText = GameObject.Find ("CantMoveText").GetComponent<Text> ();
		parentRectTransform = GameObject.Find("CantMovePanelParent").GetComponent<RectTransform> ();


	}

	void Start(){
        
	}

	public void WhiteCantMove(){
		print ("WhiteCantMove");
		othello.canMove = false;
		othello.WaitingForSpeedModeCallback = true;
		cantMoveText.text = "White cant move!\nBlack's turn";
		RotateToWhite ();
		anim.SetTrigger (TRIGGER_SWIPE_IN);

	}
	public void BlackCantMove(){
		print ("BlackCantMove");
		othello.canMove = false;
		RotateToBlack ();
		othello.WaitingForSpeedModeCallback = true;
		cantMoveText.text = "Black cant move!\nWhite's turn"; 
		anim.SetTrigger (TRIGGER_SWIPE_IN);
	}

	public void BlackOutOfTime(){
		othello.canMove = false;
		RotateToBlack ();
		othello.WaitingForSpeedModeCallback = true;
		cantMoveText.text = "Out of time!\nWhite's turn"; 
		anim.SetTrigger (TRIGGER_SWIPE_IN);
	}

	public void WhiteOutOfTime(){
		othello.canMove = false;
		cantMoveText.text = "Out of time!\nBlack's turn";
		RotateToWhite ();
		othello.WaitingForSpeedModeCallback = true;
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
			RotateToWhite();
		} else if (blacks > whites) {
			winText += "Black!";
			RotateToBlack ();
		} else {
			RotateToWhite();
			winText = "DRAW!";
		}

		cantMoveText.text = winText; 
		anim.SetTrigger (TRIGGER_SWIPE_IN);
	}
	public void OKPressed(){

		anim.SetTrigger (TRIGGER_SWIPE_OUT);
	}
	public void AnimationSwipeOutCallback(){
		othello.canMove = true;
		if (othello.hasWinner) {
			Application.LoadLevel (Application.loadedLevel);
		} else if (othello.WaitingForSpeedModeCallback) {
			othello.WaitingForSpeedModeCallback = false;
		}
	}

	private void RotateToWhite(){
		parentRectTransform.eulerAngles = forWhite;
	}
	private void RotateToBlack(){
		parentRectTransform.eulerAngles = forBlack;
	}
}
