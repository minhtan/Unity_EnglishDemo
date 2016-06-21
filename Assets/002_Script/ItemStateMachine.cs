using UnityEngine;
using System.Collections;

public class ItemStateMachine : StateMachineBehaviour {

	override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		LeanTween.scale (animator.transform.parent.gameObject, Vector3.zero, 1.0f).setEase (LeanTweenType.easeOutCubic);
	}

}
