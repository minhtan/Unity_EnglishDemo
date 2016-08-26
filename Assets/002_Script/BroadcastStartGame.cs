using UnityEngine;
using System.Collections;

public class BroadcastStartGame : MonoBehaviour {

	public void Broadcast(){
		Messenger.Broadcast (MyEvents.Game.START);
	}
}
