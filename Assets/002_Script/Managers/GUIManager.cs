using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIManager : UnitySingletonPersistent<GUIManager> {
	GameObject go;
	public Text txtQuestionLetter;
	public Text txtQuestionPrep;
	
	public Animator[] livesAnim;
	public Text txtLives;

	public GameObject pnlWin;
	public Transform pnlWinStars;

	public GameObject pnlLost;

	public GameObject btnBack;
	public GameObject tut;

	public Text[] letterToShow;

	public GameObject pnlReward;
	public Image imgMedal;
	public Text txtTotalStars;
	public Sprite[] medalSprite;
	public Text txtStarEarned;

	void OnEnable(){
		go = transform.GetChild (0).gameObject;
		Messenger.AddListener <string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
		Messenger.AddListener (MyEvents.Game.TARGETLOST, HandleTargetLost);
		Messenger.AddListener <int> (MyEvents.Game.FINISH, HandleFinishGame);
		HandleTargetLost ();
	}

	void OnDisable(){
		Messenger.RemoveListener <string> (MyEvents.Game.TARGETFOUND, HandleTargetFound);
		Messenger.RemoveListener (MyEvents.Game.TARGETLOST, HandleTargetLost);
		Messenger.RemoveListener <int> (MyEvents.Game.FINISH, HandleFinishGame);
	}

	void HandleTargetFound(string letter){
		SetGUIActive (true);
		ResetUI ();

		if (letter.Equals ("X") || letter.Equals ("Y")) {
			txtQuestionPrep.text = "that have";
		} else {
			txtQuestionPrep.text = "beginning with";
		}
		txtQuestionLetter.text = letter;

		foreach(Text t in letterToShow){
			t.text = "Letter " + letter;
		}

		if(tut.activeSelf){
			tut.SetActive (false);
		}
	}

	void HandleTargetLost(){
		SetGUIActive (false);
	}

	void HandleFinishGame(int totalStars){
		GameManager.medals medal = GameManager.Instance.CalculateMedal (totalStars);
		if(medal != GameManager.medals.none){
			pnlReward.SetActive (true);
			txtTotalStars.text = totalStars + "";
			imgMedal.sprite = medalSprite [(int)medal];
		}
	}

	public void SetGUIActive(bool state){
		go.SetActive (state);
	}

	public void SetActiveBtnback(bool state){
		StartCoroutine (ActiveBtnBackAndTut (state));
		pnlReward.SetActive (false);
	}

	IEnumerator ActiveBtnBackAndTut(bool state){
		if (!state) {
			yield return null;	
		} else {
			yield return new WaitForSeconds(1.0f);
		}
		btnBack.SetActive (state);
		tut.SetActive (state);
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

	public void UpdateStarEarned(int currentStar){
		txtStarEarned.text = currentStar + "/78 star earned";
	}
}
