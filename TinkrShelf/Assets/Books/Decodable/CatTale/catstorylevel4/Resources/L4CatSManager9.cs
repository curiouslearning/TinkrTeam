using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L4CatSManager9 : GSManager {

	GameObject jam;

	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "CatJam") {
			if (jam == null) {
				jam = GameObject.Find ("Jam");
			}

			jam.SetActive (true);
		}
		else if ( go.name == "Text_jam."){

			if (jam == null) {
				jam = GameObject.Find ("Jam");
			}

			jam.SetActive (true);
			GTinkerGraphic catHam = GameObject.Find("CatJam").GetComponent<GTinkerGraphic>();
			if (catHam != null)
			{ 
				catHam.MyOnMouseDown();
			}

		}
		else if (go.name == "Jam") 
		{   
			jam = go;
			GTinkerGraphic catJam = GameObject.Find("CatJam").GetComponent<GTinkerGraphic>();
			if (catJam != null)
			{ 
				catJam.MyOnMouseDown();
			}
		}

		base.OnMouseDown (go);
	}


}
