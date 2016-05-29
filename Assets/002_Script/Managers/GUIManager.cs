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
		ResetUI ();
	}

	void OnDisable(){
		
	}

	void Start(){
		go = transform.GetChild (0).gameObject;
		Messenger.AddListener<string> (MyEvents.Game.TARGETFOUND, TargetFound);
	}

	void TargetFound(string letter){
		txtQuestionLetter.text = letter;
	}

	public void SetActive(bool state){
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
		Messenger.Broadcast (MyEvents.Game.RESET);
	}
}
