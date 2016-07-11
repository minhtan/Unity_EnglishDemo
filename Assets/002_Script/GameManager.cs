using UnityEngine;
using System.Collections;
using Lean;

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
	public GameObject particle;
	public Vector3[] posVector;

	public GameObject winPrtcl;
	public GameObject losePrtcl;

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

	public Vector3 GetPos(int pos){
		return posVector[pos];
	}

	void OnEnable(){
		LeanTouch.OnFingerTap += OnFingerTap;
	}

	void OnDisable(){
		LeanTouch.OnFingerTap -= OnFingerTap;
	}

	void OnFingerTap(LeanFinger fg){
		Debug.Log ("tapped");

		if(GUIManager.Instance.IsResultActive() || LeanTouch.GuiInUse){
			return;
		}

		RaycastHit hitInfo;
		Ray ray = fg.GetRay ();

		if(Physics.Raycast(ray, out hitInfo)){
			Messenger.Broadcast<GameObject> (MyEvents.Game.MODEL_TAP, hitInfo.collider.gameObject);
		}
	}
}
