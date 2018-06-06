using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager3 : GSManager {

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "Cat") {
			GTinkerGraphic tinkerGraphic = GameObject.Find ("Ran").GetComponent<GTinkerGraphic> ();
			if (tinkerGraphic != null) {
				tinkerGraphic.MyOnMouseDown ();
			}
		}
		else if (go.name == "Ran") {
			GTinkerGraphic tinkerGraphic = GameObject.Find ("Cat").GetComponent<GTinkerGraphic> ();
			if (tinkerGraphic != null ) {
				tinkerGraphic.MyOnMouseDown ();
			} 
			
		}
		base.OnMouseDown (go);
}
}
