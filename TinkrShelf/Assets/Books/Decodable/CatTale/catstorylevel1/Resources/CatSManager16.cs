using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager16 : GSManager {

	GTinkerGraphic sleep; 

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "Cat" || go.name == "Text_cat") {
			if (sleep == null) {
				sleep = GameObject.Find ("Sleep").GetComponent<GTinkerGraphic> ();
			}
			sleep.MyOnMouseDown ();
		} else if (go.name == "Sleep") {
			GameObject.Find ("Cat").GetComponent<GTinkerGraphic> ().MyOnMouseDown();
		}

		base.OnMouseDown (go);

	}

}
