// Copyright (C) 2015 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;

// This class manages the audio source used to play the looping background song
// in the demo. The player can choose to mute the music, and this preference is
// persisted via Unity's PlayerPrefs.
public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;
	public const float maxVolume = 0.1f;
    private AudioSource m_audioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            m_audioSource = GetComponent<AudioSource>();
            m_audioSource.ignoreListenerVolume = true;
			m_audioSource.volume = PlayerPrefs.GetInt("music_on") >= BackgroundMusic.maxVolume ? BackgroundMusic.maxVolume : 0;
			AudioListener.volume = 1f;
        }
    }

    public void FadeIn()
    {
		if (PlayerPrefs.GetInt("music_on") == maxVolume)
        {
			StartCoroutine(FadeAudio(maxVolume, Fade.In));
        }
    }

    public void FadeOut()
    {
		if (PlayerPrefs.GetInt("music_on") == maxVolume)
        {
			StartCoroutine(FadeAudio(maxVolume, Fade.Out));
        }
    }

    private enum Fade
    {
        In,
        Out
    }

    private IEnumerator FadeAudio(float time, Fade fadeType)
    {
		var start = fadeType == Fade.In ? 0.0f : maxVolume;
		var end = fadeType == Fade.In ? maxVolume : 0.0f;
        var i = 0.0f;
		var step = maxVolume / time;

		while (i <= maxVolume)
        {
            i += step * Time.deltaTime;
            m_audioSource.volume = Mathf.Lerp(start, end, i);
            yield return new WaitForSeconds(step * Time.deltaTime);
        }
    }
}