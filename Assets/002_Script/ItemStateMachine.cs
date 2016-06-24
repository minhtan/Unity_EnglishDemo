using UnityEngine;
using System.Collections;

public class ItemStateMachine : StateMachineBehaviour {

	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit (animator, stateInfo, layerIndex);
		LeanTween.scale (animator.transform.parent.gameObject, Vector3.zero, 1.0f).setEase (LeanTweenType.easeOutCubic);
		Messenger.Broadcast(MyEvents.Game.ANIM_END);
	}

}
