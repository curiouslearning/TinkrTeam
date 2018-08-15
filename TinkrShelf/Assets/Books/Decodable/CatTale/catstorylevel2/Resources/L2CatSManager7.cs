using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2CatSManager7 : GSManager {

	GameObject ham;

	public override void OnMouseDown(GameObject go)
	{   Debug.Log ("scene spf entered");
		if (go.name == "CatHam") {
			if (ham == null) {
				ham = GameObject.Find ("Ham");
			}

			ham.SetActive (true);
		}
		else if ( go.name == "Text_ham"){
			Debug.Log ("ham if enterred");
			if (ham == null) {
				ham = GameObject.Find ("Ham");
			}

			ham.SetActive (true);
			GTinkerGraphic catHam = GameObject.Find("CatHam").GetComponent<GTinkerGraphic>();
			if (catHam != null)
			{ 
				catHam.MyOnMouseDown();
			}

		}
		else if (go.name == "Ham") 
		{   
			ham = go;
			GTinkerGraphic catHam = GameObject.Find("CatHam").GetComponent<GTinkerGraphic>();
			if (catHam != null)
			{ 
				catHam.MyOnMouseDown();
			}
		}

		base.OnMouseDown (go);
	}

}