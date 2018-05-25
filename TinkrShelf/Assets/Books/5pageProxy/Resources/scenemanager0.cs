using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenemanager0 : GSManager {
	


	public override void Start() 
	{ base.Start ();
		
	}

	public override void OnMouseDown(GameObject go)

	{    base.OnMouseDown (go);      
		
		if (go.name == "blue") {
				Debug.Log (go.name);
					GameObject changecolour = GameObject.Find ("cat");
					changecolour.GetComponent<SpriteRenderer> ().color = blue;
				} else if (go.name == "yellow") {
				Debug.Log (go.name);
					GameObject changecolour = GameObject.Find ("cat");
					changecolour.GetComponent<SpriteRenderer> ().color = yellow;
				} else if (go.name == "red") {
					GameObject changecolour = GameObject.Find ("cat");
					changecolour.GetComponent<SpriteRenderer> ().color = red;
				}
				GTinkerGraphic tinkerGraphic = go.GetComponent<GTinkerGraphic> ();
				Debug.Log ("clicked 3");
				if (tinkerGraphic != null) {
					Debug.Log ("clicked 4");
					tinkerGraphic.MyOnMouseDown ();
				}
			}


	//	}

	}


