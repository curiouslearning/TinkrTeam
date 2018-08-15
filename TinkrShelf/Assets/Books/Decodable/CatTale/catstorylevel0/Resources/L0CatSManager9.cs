﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L0CatSManager9 : GSManager {

	GameObject jam;

	public override void Start ()
	{   jam = GameObject.Find ("Jam");
		base.Start ();
	}

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "CatJam") {
			jam.SetActive (true);
		} else if (go.name == "Text_jam") {
			jam.SetActive (true);
			GTinkerGraphic catJam = GameObject.Find ("CatJam").GetComponent<GTinkerGraphic> ();
			if (catJam != null) {
				//catJam.reset ();
				catJam.MyOnMouseDown ();
			}


		}
		//		else if (go.name == "Jam") 
		//		{   
		//			jam = go;
		//			GTinkerGraphic catJam = GameObject.Find("CatJam").GetComponent<GTinkerGraphic>();
		//			if (catJam != null)
		//			{ 
		//				catJam.MyOnMouseDown();
		//			}
		//		}

		base.OnMouseDown (go);
	}
}