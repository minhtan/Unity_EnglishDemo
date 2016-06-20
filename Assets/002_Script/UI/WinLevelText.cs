using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinLevelText : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		Messenger.AddListener<string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
	}

	void HandleTargetFound(string letterFound){
		GetComponent<Text> ().text = "Letter " + letterFound;
	}
}
