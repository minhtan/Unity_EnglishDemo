using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Models{
	public GameManager.modelsName name;
	public bool isCorrect;
}

public class GameManager : UnitySingletonPersistent<GameManager> {
	public GameObject particle;
	public Vector3[] posVector;

	public enum modelsName{
		ant,apple,baby,banana,bear,car,cat,croc
	}

	public Vector3 GetPos(int pos){
		return posVector[pos];
	}
}
