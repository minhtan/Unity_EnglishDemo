using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessengerPopUp : MonoBehaviour {
	void Start () {
		if(PlayerPrefs.HasKey("Prize")){
			int medal = PlayerPrefs.GetInt ("Prize");
			if (medal <= 0)
				return;
			
			string text = "";
			switch (medal) {
			case 1:
				text = "You won a CEC gold medal, please contact us to redeem\nyour prize!";
				break;
			case 2:
				text = "You won a CEC silver medal, please contact us to redeem\nyour prize!";
				break;
			case 3:
				text = "You won a CEC bronze medal, please contact us to redeem\nyour prize!";
				break;
			default:
				break;
			}
			GameObject go = transform.Find ("Messages/ScrollView/Content/Reward").gameObject;
			go.SetActive (true);
			go.transform.Find ("RewardText").gameObject.GetComponent<Text>().text = text;
		}	
	}

}
