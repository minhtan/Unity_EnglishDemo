using UnityEngine;
using System.Collections;

public class MyEvents : MonoBehaviour {

	public enum Game{
		RESET,
		TARGETFOUND,
		MODEL_TAP,
		TARGETLOST,
		ANIM_END,
		START,
		WIN,
		FINISH
	}
}
