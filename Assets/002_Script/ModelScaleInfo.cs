using UnityEngine;
using System.Collections;

public class ModelScaleInfo : MonoBehaviour {

	[HideInInspector]
	public float scale;
	// Use this for initialization
	void Awake () {
		scale = transform.localScale.x;
	}

}
