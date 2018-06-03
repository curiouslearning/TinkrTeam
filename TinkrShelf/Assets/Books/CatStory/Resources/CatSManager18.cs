using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager18 : GSManager {

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
		if (go.name =="Jam") 
		{   
			GameObject seqAnim = GameObject.Find ("Cat");
			GTinkerGraphic tinkerGraphic1 = seqAnim.GetComponent<GTinkerGraphic>();
			if (tinkerGraphic1 != null)
			{   Debug.Log("hehehehe");
				tinkerGraphic1.MyOnMouseDown();

			}
		}


	}
}
