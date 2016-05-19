using UnityEngine;
using System.Collections;
using Lean;
using UnityEngine.UI;

public class ModelController : MonoBehaviour {
	int currentIncorrect;
	int currentCorrect;

	public string letter;
	public GameObject particle;
	public AudioClip questionSound;
	public AudioClip wrongSound;

	void OnEnable(){
		Messenger.Broadcast<string> (MyEvents.Game.TARGETFOUND, letter);
		Init ();
		LeanTouch.OnFingerTap += OnFingerTap;
		Messenger.AddListener (MyEvents.Game.RESET, _Reset);
	}

	void OnDisable(){
		Remove ();
		LeanTouch.OnFingerTap -= OnFingerTap;
		Messenger.RemoveListener (MyEvents.Game.RESET, _Reset);
	}

	void OnFingerTap(LeanFinger fg){
		if(GUIManager.Instance.IsResultActive()){
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();

		if(Physics.Raycast(ray, out hitInfo)){
			GameObject go = hitInfo.collider.gameObject;
			go.GetComponent<Collider> ().enabled = false;
			Mark m = go.GetComponent<Mark> ();
			if (m.correct) {
				SoundManager.Instance.PlaySound (m.clip);

				GameObject par = Instantiate (particle);
				par.transform.SetParent (go.transform.parent, false);
				LeanTween.scale (go, Vector3.zero, 1.0f).setEase (LeanTweenType.easeOutCubic);

				currentCorrect++;
				if(currentCorrect == 3){
					Win ();
				}
			} else {
				SoundManager.Instance.PlaySound (wrongSound);

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

		GUIManager.Instance.ResetUI ();

		foreach(Transform t in gameObject.transform){
			GameObject pref = Resources.Load (t.gameObject.name) as GameObject;
			GameObject go = Instantiate (pref);
			go.transform.SetParent (t, false);
		}
	}

	void Remove(){
		foreach(Transform t in gameObject.transform){
			Destroy (t.GetChild (0).gameObject);
		}
	}

	public void _Reset(){
		Remove ();
		Init ();
	}
}
