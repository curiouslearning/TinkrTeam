using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager10 : GSManager {

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
		if (go.name == "Fat") 
		{  go.SetActive (false);
			GameObject seqAnim = GameObject.Find ("FatCatRun");
			GTinkerGraphic tinkerGraphic1 = seqAnim.GetComponent<GTinkerGraphic>();
			if (tinkerGraphic1 != null)
			{    Debug.Log("hehehehe");
				tinkerGraphic1.MyOnMouseDown();
			}
		}


	}
}
