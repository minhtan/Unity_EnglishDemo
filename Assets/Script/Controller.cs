using UnityEngine;
using System.Collections;
using Lean;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	int currentIncorrect;
	int currentCorrect;
	public Transform pnlLivesImg;
	public GameObject particle;
	public GameObject pnlResult;
	public AudioClip wrongSound;

	void OnEnable(){
		Init ();
		LeanTouch.OnFingerTap += OnFingerTap;
	}

	void OnDisable(){
		Remove ();
		LeanTouch.OnFingerTap -= OnFingerTap;
	}

	void OnFingerTap(LeanFinger fg){
		if(pnlResult.activeSelf){
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
				pnlLivesImg.GetChild (currentIncorrect - 1).gameObject.GetComponent<Image> ().color = Color.black;
				if(currentIncorrect == 3){
					GameOver ();
				}
			}
		}
	}
		
	void GameOver(){
		pnlResult.SetActive (true);
		pnlResult.GetComponentInChildren<Text> ().text = "Try again.";
	}

	void Win(){
		pnlResult.SetActive (true);
		pnlResult.GetComponentInChildren<Text> ().text = "Correct!";
	}

	void Init(){
		currentCorrect = 0;
		currentIncorrect = 0;
		foreach(Transform t in pnlLivesImg){
			t.gameObject.GetComponent<Image> ().color = Color.white;
		}
		pnlResult.SetActive (false);
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
		pnlResult.SetActive (false);
	}
}
