using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager9 : GSManager {

	GameObject jam;

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "CatJam" || go.name == "Text_Jam") {
			if (jam == null) {
				jam = GameObject.Find ("Jam");
			}
			jam.SetActive (true);
			StartCoroutine (SetGameObject());
		}
		if (go.name == "Jam") 
		{   
			StartCoroutine (SetGameObject());
			GTinkerGraphic catJam = GameObject.Find("CatJam").GetComponent<GTinkerGraphic>();
			if (catJam != null)
			{ 
				catJam.MyOnMouseDown();
			}
		}
		base.OnMouseDown (go);

	}

	public IEnumerator SetGameObject ()
	{
		yield return new WaitForSeconds (2.0f);
		jam = GameObject.Find ("Jam"); 
		jam.SetActive (false);
	}

}
