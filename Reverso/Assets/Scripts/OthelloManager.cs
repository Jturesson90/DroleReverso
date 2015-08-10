using UnityEngine;
using System.Collections;

public class OthelloManager : MonoBehaviour {

	private static OthelloManager othelloManager;

	public bool SpeedMode = false;
	public bool PlayAgainstComputer = false;
	public enum ComputerLevel
	{
		Easy,Normal,Hard
	}

	public bool UseHints = false;



	void Awake(){
		if (!othelloManager) {
			othelloManager = this;
		} else {
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartGame(){
		Application.LoadLevel("GameScene");
	}

	public void StartVersus(){
		PlayAgainstComputer = false;
		StartGame ();
	}
	
	public void StartComputer(){
		PlayAgainstComputer = true;
		StartGame ();
	}
}
