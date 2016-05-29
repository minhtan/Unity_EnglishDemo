using UnityEngine;
using System.Collections;

public class SoundManager : UnitySingletonPersistent<SoundManager>
{
	private AudioSource audioSource;
	public AudioClip wrongSound;

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

	public void Stop(){
		audioSource.Stop();
	}
}