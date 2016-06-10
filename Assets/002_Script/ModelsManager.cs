using UnityEngine;
using System.Collections;
using Lean;
using UnityEngine.UI;

public class ModelsManager : MonoBehaviour {
	int currentIncorrect;
	int currentCorrect;

	public string letter;

	public Models[] models;

	void OnEnable(){
		Init ();
		Messenger.Broadcast<string> (MyEvents.Game.TARGETFOUND, letter);
		Messenger.AddListener (MyEvents.Game.RESET, HandleReset);
		Messenger.AddListener <GameObject> (MyEvents.Game.MODEL_TAP, HandleModelTap);
	}

	void OnDisable(){
		End ();
		Messenger.Broadcast (MyEvents.Game.TARGETLOST);
		Messenger.RemoveListener (MyEvents.Game.RESET, HandleReset);
		Messenger.RemoveListener <GameObject> (MyEvents.Game.MODEL_TAP, HandleModelTap);
	}

	bool CompareAnswer(string name){
		for(int i = 0; i < models.Length; i++){
			if(models[i].Name == name){
				return true;
			}
		}
		return false;
	}

	void HandleModelTap(GameObject go){
		go.GetComponent<Collider> ().enabled = false;
		if (CompareAnswer(go.name)) {
			SoundManager.Instance.PlaySound (go.GetComponent<Mark> ().clip);

			GameObject par = Instantiate (GameManager.Instance.particle);
			par.transform.SetParent (go.transform.parent, false);
			LeanTween.scale (go, Vector3.zero, 1.0f).setEase (LeanTweenType.easeOutCubic);

			currentCorrect++;
			if(currentCorrect == 3){
				Win ();
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
			GUIManager.Instance.ShowLostLive (currentIncorrect);
			if(currentIncorrect == 3){
				GameOver ();
			}
		}
	}
		
	void GameOver(){
		GUIManager.Instance.ShowResult (GUIManager.LOSE_TEXT);
	}

	void Win(){
		GUIManager.Instance.ShowResult (GUIManager.WIN_TEXT);
	}

	void Init(){
		currentCorrect = 0;
		currentIncorrect = 0;

		SoundManager.Instance.PlaySound (Resources.Load<AudioClip>("Q_Sound/" + letter));

		for (int i = 0; i < models.Length; i++) {
			GameObject go = Instantiate (Resources.Load<GameObject> ("Models/" + models[i].Name));
			go.transform.localPosition = GameManager.Instance.GetPos (i);
			go.transform.SetParent (transform, false);
		}
	}

	void End(){
		foreach(Transform t in gameObject.transform){
			Destroy (t.gameObject);
		}
		SoundManager.Instance.Stop ();
	}

	public void HandleReset(){
		End ();
		Init ();
	}
}
