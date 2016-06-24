using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : UnitySingletonPersistent<GUIManager> {
	GameObject go;
	public Text txtQuestionLetter;
	
	public Animator[] livesAnim;
	public Text txtLives;

	public GameObject pnlWin;
	public Transform pnlWinStars;

	public GameObject pnlLost;

	public GameObject btnBack;

	public Text[] letterToShow;

	void OnEnable(){
		go = transform.GetChild (0).gameObject;
		Messenger.AddListener <string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
		Messenger.AddListener (MyEvents.Game.TARGETLOST, HandleTargetLost);
		HandleTargetLost ();
	}

	void OnDisable(){
		Messenger.RemoveListener <string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
		Messenger.RemoveListener (MyEvents.Game.TARGETLOST, HandleTargetLost);
	}

	void HandleTargetFound(string letter){
		SetGUIActive (true);
		ResetUI ();
		txtQuestionLetter.text = letter;
		foreach(Text t in letterToShow){
			t.text = "Letter " + letter;
		}
	}

	void HandleTargetLost(){
		SetGUIActive (false);
	}

	public void SetGUIActive(bool state){
		go.SetActive (state);
	}

	public void SetActiveBtnback(bool state){
		btnBack.SetActive (state);
	}

	public void ShowLostLive(int index){
		foreach(Animator a in livesAnim){
			a.SetTrigger ("lostLive");
		}
		txtLives.text = index + "";
	}

	public void ResetUI(){
		pnlWin.SetActive (false);
		pnlLost.SetActive (false);
		txtLives.text = "3";
		foreach(Animator a in livesAnim){
			a.SetTrigger ("reset");
		}
		foreach(Transform t in pnlWinStars){
			t.gameObject.SetActive (false);
		}
	}

	public bool IsResultActive(){
		if (pnlWin.activeSelf || pnlLost.activeSelf) {
			return true;
		} else {
			return false;
		}
	}

	public void ShowWinPnl(int numOfCorrect){
		pnlWin.SetActive (true);
		for (int i = 0; i < numOfCorrect; i++) {
			pnlWinStars.GetChild (i).gameObject.SetActive (true);
		}
	}

	public void ShowLostPnl(){
		pnlLost.SetActive (true);
	}

	public void _Reset(){
		ResetUI ();
		Messenger.Broadcast (MyEvents.Game.RESET);
	}
}
