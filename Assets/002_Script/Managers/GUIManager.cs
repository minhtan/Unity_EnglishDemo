using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : UnitySingletonPersistent<GUIManager> {
	public const string WIN_TEXT = "Correct";
	public const string LOSE_TEXT = "Try again";

	GameObject go;
	public Text txtQuestionLetter;
	public Transform pnlLivesImg;
	public GameObject pnlResult;

	void OnEnable(){
		go = transform.GetChild (0).gameObject;
		Messenger.AddListener <string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
		Messenger.AddListener (MyEvents.Game.TARGETLOST, HandleTargetLost);

	}

	void OnDisable(){
		Messenger.RemoveListener <string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
		Messenger.RemoveListener (MyEvents.Game.TARGETLOST, HandleTargetLost);
	}

	void HandleTargetFound(string letter){
		SetGUIActive (true);
		ResetUI ();
		txtQuestionLetter.text = letter;
	}

	void HandleTargetLost(){
		SetGUIActive (false);
	}

	public void SetGUIActive(bool state){
		go.SetActive (state);
	}

	public void ShowLostLive(int index){
		pnlLivesImg.GetChild (index - 1).gameObject.GetComponent<Image> ().color = Color.black;
	}

	public void ResetUI(){
		foreach(Transform t in pnlLivesImg){
			t.gameObject.GetComponent<Image> ().color = Color.white;
		}
		pnlResult.SetActive (false);
	}

	public bool IsResultActive(){
		return pnlResult.activeSelf;
	}

	public void ShowResult(string msg){
		pnlResult.SetActive (true);
		pnlResult.GetComponentInChildren<Text> ().text = msg;
	}

	public void _Reset(){
		ResetUI ();
		Messenger.Broadcast (MyEvents.Game.RESET);
	}
}
