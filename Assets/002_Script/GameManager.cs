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

	public enum modelsName{
		ant,apple,baby,banana,bear,car,cat,
		croc,axe,bee,cookie,dino,dog,duck, 
		egg,elephant,fish,fox,gorila,guitar,
		hippo,ninja,pig,pirate,rabbit,lion, 
		train,truck,violin,ear,flower,girl,
		hat,house,iguana,insect,igloo,
		jellyfish,juice,jump
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
