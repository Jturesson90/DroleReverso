using UnityEngine;
using System.Collections;

public class GameBackButton : MonoBehaviour {

	// Use this for initialization
	void Awake () {
#if UNITY_ANDROID
        gameObject.SetActive(false);
#endif

    }
	

}
