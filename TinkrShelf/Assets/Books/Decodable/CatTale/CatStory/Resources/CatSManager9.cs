﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager9 : GSManager {

	GameObject ham;

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "CatJam") {
			if (ham == null) {
				ham = GameObject.Find ("Jam");
			}

			ham.SetActive (true);
		}
		else if ( go.name == "Text_Jam"){

			if (ham == null) {
				ham = GameObject.Find ("Jam");
			}

			ham.SetActive (true);
			GTinkerGraphic catHam = GameObject.Find("CatJam").GetComponent<GTinkerGraphic>();
			if (catHam != null)
			{ 
				catHam.MyOnMouseDown();
			}

		}
		else if (go.name == "Jam") 
		{  
			ham = go;
			GTinkerGraphic catHam = GameObject.Find("CatJam").GetComponent<GTinkerGraphic>();
			if (catHam != null)
			{ 
				catHam.MyOnMouseDown();
			}
		}

		base.OnMouseDown (go);
	}

}