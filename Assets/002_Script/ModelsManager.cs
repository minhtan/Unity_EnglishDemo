using UnityEngine;
using System.Collections;
using Lean;
using UnityEngine.UI;

public class ModelsManager : MonoBehaviour {
	
	int currentIncorrect;
	int currentCorrect;
	int totalCorrect;
	bool gameover;
	public bool isLastAnimRunning;
	int currentAnim;

	public GameManager.letters letter;

	public Models[] models;

	void OnEnable(){
		StartCoroutine( Init () );
		Messenger.Broadcast<string> (MyEvents.Game.TARGETFOUND, letter.ToString().ToUpper());
		Messenger.AddListener (MyEvents.Game.RESET, HandleReset);
		Messenger.AddListener <GameObject> (MyEvents.Game.MODEL_TAP, HandleModelTap);
		Messenger.AddListener (MyEvents.Game.ANIM_END, OnAnimEnd);
	}

	void OnDisable(){
		End ();
		Messenger.Broadcast (MyEvents.Game.TARGETLOST);
		Messenger.RemoveListener (MyEvents.Game.RESET, HandleReset);
		Messenger.RemoveListener <GameObject> (MyEvents.Game.MODEL_TAP, HandleModelTap);
		Messenger.RemoveListener (MyEvents.Game.ANIM_END, OnAnimEnd);
	}

	bool CompareAnswer(string name){
		for(int i = 0; i < models.Length; i++){
			if(models[i].Name == name && models[i].isCorrect){
				return true;
			}
		}
		return false;
	}

	void HandleModelTap(GameObject go){
		if(gameover){
			return;
		}

		go.GetComponent<Collider> ().enabled = false;
		if (CompareAnswer(go.name)) {
			AudioClip clip = Resources.Load<AudioClip> ("M_Sound/" + go.name);
			if(clip != null){
				SoundManager.Instance.PlaySound (clip);
			}

			go.GetComponentInChildren<Animator> ().SetTrigger ("tap");

			currentCorrect++;
			if(currentCorrect == totalCorrect){
				StartCoroutine(Win ());
			}
		} else {
			SoundManager.Instance.PlayWrongSound ();

			Renderer[] rd = go.GetComponentsInChildren<Renderer> ();
			for (int i = 0; i < rd.Length; i++) {
				Material[] mtrl = rd [i].GetComponent<Renderer> ().materials;
				for(int j = 0; j < mtrl.Length; j ++){
					mtrl [j].color = Color.white;
					mtrl [j].mainTexture = null;
				}
			}

			currentIncorrect++;
			GUIManager.Instance.ShowLostLive (3-currentIncorrect);
			if(currentIncorrect == 3){
				GameOver ();
			}
		}
	}
		
	void GameOver(){
		gameover = true;
		GUIManager.Instance.ShowLostPnl ();
	}

	void OnAnimEnd(){
		currentAnim++;
		if(currentAnim == totalCorrect){
			isLastAnimRunning = false;
		}
	}

	IEnumerator Win(){
		gameover = true;
		do {
			yield return null;
		} while(isLastAnimRunning);
		GUIManager.Instance.ShowWinPnl (3-currentIncorrect);
	}

	void CalculateTotalCorrect(){
		totalCorrect = 0;
		for(int i = 0; i < models.Length; i++){
			if (models [i].isCorrect) {
				totalCorrect++;
			}
		}
	}

	IEnumerator Init(){
		currentCorrect = 0;
		currentIncorrect = 0;
		currentAnim = 0;
		isLastAnimRunning = true;
		gameover = false;
		CalculateTotalCorrect ();

		AudioClip clip = Resources.Load<AudioClip> ("Q_Sound/" + letter.ToString().ToUpper());
		if(clip != null){
			SoundManager.Instance.PlaySound (clip);
		}

		for (int i = 0; i < models.Length; i++) {
			ResourceRequest rr = Resources.LoadAsync<GameObject> ("Models/" + models [i].Name);
			while(!rr.isDone){
				yield return null;
			}
			GameObject origin = rr.asset as GameObject;
			if(origin != null){
				GameObject go = Instantiate (origin);
				go.name = models [i].Name;
				go.transform.localPosition = GameManager.Instance.GetPos (i);
				go.transform.SetParent (transform, false);
				go.transform.localScale = Vector3.zero;
			}
		}

		StartCoroutine (ScaleModelsUp());
	}

	IEnumerator ScaleModelsUp(){
		for (int i = 0; i < models.Length;) {
			try{
				LeanTween.scale (
					transform.GetChild (i).gameObject, 
					transform.GetChild (i).GetComponent<ModelScaleInfo> ().scale.ToVector3 (),
					0.5f).setEase (LeanTweenType.easeOutBounce);
			}catch{
				Debug.Log ("some thing went wrong :D");
			}
			yield return new WaitForSeconds (0.1f);
			i++;
		}
	}

	void End(){
		foreach(Transform t in gameObject.transform){
			Destroy (t.gameObject);
		}
		SoundManager.Instance.Stop ();
		Resources.UnloadUnusedAssets ();
	}

	public void HandleReset(){
		End ();
		StartCoroutine( Init () );
	}
}
