using UnityEngine;
using System.Collections;

public class ExitApplication : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{ 
			if(Application.loadedLevelName.Equals("GameScene")){
				Application.LoadLevel(Application.loadedLevel-1);
			}else{
				Application.Quit(); 
			}
		}
	}
}
