using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager9 : GSManager {
//
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "Jam") 
		{   StartCoroutine (SetGameObject());
			GameObject seqAnim = GameObject.Find ("CatJam");
			GTinkerGraphic tinkerGraphic1 = seqAnim.GetComponent<GTinkerGraphic>();
			if (tinkerGraphic1 != null)
			{
				tinkerGraphic1.MyOnMouseDown();

			}
		}


	}
	public IEnumerator SetGameObject ()
	{
		yield return new WaitForSeconds (2.0f);
		GameObject.Find ("Jam").SetActive (false);
	}

}
