using UnityEngine;
using System.Collections;
using Lean;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct Models{
	public GameManager.modelsName name;
	public string Name {
		get {
			return name.ToString();
		}
	}

	public bool isCorrect;
}

public class GameManager : UnitySingletonPersistent<GameManager> {
	public Vector3[] posVector;

	public GameObject winPrtcl;
	public GameObject losePrtcl;

	int totalStars;
	List<string> playableLetters;

	public enum letters{
		a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z
	}

	public enum modelsName{
		ant,apple,baby,banana,bear,car,cat,
		aligator,axe,bee,cookie,dino,dog,duck, 
		egg,elephant,fish,fox,gorila,guitar,
		hippo,ninja,pig,pirate,rabbit,lion, 
		train,van,violin,ear,flower,girl,
		hat,house,iguana,insect,igloo,
		jellyfish,juice,jump,key,kite,kangaroo,
		lemon,leaf,monkey,monster,mouse,nose,
		nap,orange,octopus,ostrich,panda,
		quack,quite,queen,robot,rainbow,
		spider,superman,snake,ten,tiger,
		up,unicorn,umbrella,vampire,witch,whale,
		watch,box,yellow,yeti,yoyo,zebra,
		zombie,zip
	}

	public enum medals{
		gold, silver, bronze, none
	}

	public Vector3 GetPos(int pos){
		return posVector[pos];
	}

	void OnEnable(){
		playableLetters = new List<string> ();
		LeanTouch.OnFingerTap += OnFingerTap;
		Messenger.AddListener (MyEvents.Game.START, StartGameHandle);
		Messenger.AddListener<int, string> (MyEvents.Game.WIN, WinHandle);
	}

	void OnDisable(){
		LeanTouch.OnFingerTap -= OnFingerTap;
		Messenger.RemoveListener (MyEvents.Game.START, StartGameHandle);
		Messenger.RemoveListener<int, string> (MyEvents.Game.WIN, WinHandle);
	}

	void OnFingerTap(LeanFinger fg){
//		Instantiate (winPrtcl, fg.GetWorldPosition(1.0f), Quaternion.identity);

		if(GUIManager.Instance.IsResultActive() || LeanTouch.GuiInUse){
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();

		if(Physics.Raycast(ray, out hitInfo)){
			Messenger.Broadcast<GameObject, Vector3> (MyEvents.Game.MODEL_TAP, hitInfo.collider.gameObject, fg.GetWorldPosition(1.0f));
		}
	}

	void StartGameHandle(){
		totalStars = 0;
		playableLetters.Clear();
		foreach (letters letter in Enum.GetValues(typeof(letters))){
			playableLetters.Add (letter.ToString ());
		}
	}

	void WinHandle(int earnedStar, string letter){
		if(playableLetters.Contains (letter)){
			playableLetters.Remove (letter);
			totalStars = totalStars + earnedStar;

			if (playableLetters.Count <= 0) {
				Messenger.Broadcast<int> (MyEvents.Game.FINISH, totalStars);
			} else {
				GUIManager.Instance.ShowWinPnl (earnedStar);
			}
		}
	}

	public medals CalculateMedal(int totalStars){
		medals medal;
		int medalInPref;
		if (totalStars >= 78) {
			medal = medals.gold;
			medalInPref = 1;
		} else if (totalStars >= 72) {
			medal = medals.silver;
			medalInPref = 2;
		} else if (totalStars >= 66) {
			medal = medals.bronze;
			medalInPref = 3;
		} else {
			medal = medals.none;
			medalInPref = 0;
		}
		PlayerPrefs.SetInt ("Prize", medalInPref);
		return medal;
	}
}
