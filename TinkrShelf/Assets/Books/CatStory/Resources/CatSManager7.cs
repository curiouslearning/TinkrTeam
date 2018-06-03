using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSManager7 : GSManager {

	// Use this for initialization
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
		if (go.name == "Ham") 
			{ GameObject seqAnim = GameObject.Find ("CatHam");
				GTinkerGraphic tinkerGraphic1 = seqAnim.GetComponent<GTinkerGraphic>();
				if (tinkerGraphic1 != null)
				{
					tinkerGraphic1.MyOnMouseDown();
				    StartCoroutine (SetGameObject());
				}
			}

		
	}
	public IEnumerator SetGameObject ()
	{
		yield return new WaitForSeconds (2.5f);
		GameObject.Find ("Ham").SetActive (false);
	}
}
