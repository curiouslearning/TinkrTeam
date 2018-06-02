using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager3 : GSManager {

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
		if (go.name == "Cat") 
		{
			GameObject seqAnim = GameObject.Find ("Ran");
			GTinkerGraphic tinkerGraphic1 = seqAnim.GetComponent<GTinkerGraphic> ();
			GTinkerGraphic tinkerGraphic2 = go.GetComponent<GTinkerGraphic> ();
			if (tinkerGraphic1 != null && tinkerGraphic2 != null) 
			{
				tinkerGraphic1.MyOnMouseDown ();
				tinkerGraphic2.MyOnMouseDown ();
			}
		
	}
}
}
