using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour {


	OthelloManager manager;

	Toggle hintToggle;
	Toggle speedModeToggle;

	void Awake(){
		manager = GameObject.Find ("OthelloManager").GetComponent<OthelloManager> ();
		hintToggle = GameObject.Find("HintsToggle").GetComponent<Toggle> ();
		speedModeToggle = GameObject.Find("SpeedModeToggle").GetComponent<Toggle> ();
	}

	public void StartVersus(){
		manager.StartVersus ();
		manager.UseHints = hintToggle.isOn;
		manager.SpeedMode = speedModeToggle.isOn;
	}
	
	public void StartComputer(){
		manager.StartComputer ();
		manager.UseHints = hintToggle.isOn;
		manager.SpeedMode = speedModeToggle.isOn;
	}


}
