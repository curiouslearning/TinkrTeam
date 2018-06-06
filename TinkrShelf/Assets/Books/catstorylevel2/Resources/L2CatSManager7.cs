using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2CatSManager7 : GSManager {

	GameObject ham;

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "CatHam" || go.name == "Text_Ham") {
			if (ham == null) {
				ham = GameObject.Find ("Ham");
			}
			ham.SetActive (true);
			StartCoroutine (SetGameObject());
		}

		if (go.name == "Ham") 
		{   
			StartCoroutine (SetGameObject());
			GTinkerGraphic catHam = GameObject.Find("CatHam").GetComponent<GTinkerGraphic>();
			if (catHam != null)
			{ 
				catHam.MyOnMouseDown();
			}
		}
			
		base.OnMouseDown (go);
	}
	public IEnumerator SetGameObject ()
	{
		yield return new WaitForSeconds (2.5f);
		ham = GameObject.Find ("Ham"); 
		ham.SetActive (false);
	}
		
}