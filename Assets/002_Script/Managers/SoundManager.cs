using UnityEngine;
using System.Collections;

public class SoundManager : UnitySingletonPersistent<SoundManager>
{
	private AudioSource audioSource;
	public AudioClip wrongSound;
	public AudioClip[] correctSound;

	public override void Awake()
	{
		base.Awake();
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void PlaySound(AudioClip clipToPlay) {
		audioSource.Stop();
		audioSource.clip = clipToPlay;
		audioSource.Play();
	}

	public void PlayWrongSound(){
		PlaySound (wrongSound);
	}

	public IEnumerator PlayCorrectSound(System.Action callback){
		PlaySound (correctSound [UnityEngine.Random.Range(0, correctSound.Length - 1)] );

		while (audioSource.isPlaying) {
			yield return null;
		}

		callback ();
	}

	public void Stop(){
		audioSource.Stop();
	}
}