using UnityEngine;
using System.Collections;
using Lean;
using UnityEngine.UI;

public class ModelsManager : MonoBehaviour {
	int currentIncorrect;
	int currentCorrect;
	int totalCorrect;

	public GameManager.letters letter;

	public Models[] models;

	void OnEnable(){
		Init ();
		Messenger.Broadcast<string> (MyEvents.Game.TARGETFOUND, letter.ToString().ToUpper());
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
			if(models[i].Name == name && models[i].isCorrect){
				return true;
			}
		}
		return false;
	}

	void HandleModelTap(GameObject go){
		go.GetComponent<Collider> ().enabled = false;
		if (CompareAnswer(go.name)) {
			AudioClip clip = Resources.Load<AudioClip> ("M_Sound/" + go.name);
			if(clip != null){
				SoundManager.Instance.PlaySound (clip);
			}

			go.GetComponentInChildren<Animator> ().SetTrigger ("tap");

			currentCorrect++;
			if(currentCorrect == totalCorrect){
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

	void CalculateTotalCorrect(){
		totalCorrect = 0;
		for(int i = 0; i < models.Length; i++){
			if (models [i].isCorrect) {
				totalCorrect++;
			}
		}
	}

	void Init(){
		currentCorrect = 0;
		currentIncorrect = 0;
		CalculateTotalCorrect ();

		AudioClip clip = Resources.Load<AudioClip> ("Q_Sound/" + letter.ToString().ToUpper());
		if(clip != null){
			SoundManager.Instance.PlaySound (clip);
		}

		for (int i = 0; i < models.Length; i++) {
			GameObject origin = Resources.Load<GameObject> ("Models/" + models [i].Name);
			if(origin != null){
				GameObject go = Instantiate (origin);
				go.name = models [i].Name;
				go.transform.localPosition = GameManager.Instance.GetPos (i);
				go.transform.SetParent (transform, false);
			}
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
		Init ();
	}
}
