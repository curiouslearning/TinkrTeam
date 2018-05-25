using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenemanager1 : GSManager {
	
	// Use this for initialization
	public override void Start() {
		base.Start ();

	}
	IEnumerator WaitTimeAndLoadNextScene()
	{
		yield return new WaitForSeconds(2.33f);
		NextScene();
	}
		
	public override void OnMouseDown(GameObject go)
	{   base.OnMouseDown (go);
		if (go.name == "redcat") {
			Destroy (go);
			StartCoroutine (WaitTimeAndLoadNextScene ());
		} else if (go.name == "yellowcat" || go.name == "bluecat") {
			
			Destroy (go);
		}

	
	}}



