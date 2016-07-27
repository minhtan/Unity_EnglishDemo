﻿// Copyright (C) 2015 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;

// This class is responsible for loading the next scene in a transition (the core of
// this work is performed in the Transition class, though).
using System.Collections;


public class SceneTransition : MonoBehaviour
{
    public string scene = "<Insert scene name>";
    public float duration = 1.0f;
    public Color color = Color.black;

    public void PerformTransition()
    {
        Transition.LoadLevel(scene, duration, color);
		StartCoroutine (Wait ());
    }

	IEnumerator Wait(){
		yield return new WaitForEndOfFrame ();
		if (GUIManager.Instance != null) {
			if(scene == "full"){
				GUIManager.Instance.SetActiveBtnback (true);
			}else{
				GUIManager.Instance.SetActiveBtnback (false);
			}
		}
	}
}