using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager0 : GSManager {
	List<string> options;
	GameObject cat1, cat2, cat3;

	public override void Start ()
	{
		base.Start ();
		options = new List<string> ();
		cat1 = GameObject.Find ("red");
		cat2 = GameObject.Find ("yellow");
		cat3 = GameObject.Find ("blue");
		options.Add (cat2.name);
		options.Add (cat3.name);
	}

	public override void OnMouseDown(GameObject go)
	{    
		base.OnMouseDown (go);      

		if (go == cat1) {
			//sending data directly to firebase using "72 hours rule"! (removed local data storage)
			//DataCollection.AddInResponseData (cat1.name, cat1.name, options, true, System.DateTime.Now.ToString ());

			FirebaseHelper.LogInAppResponse (cat1.name, cat1.name, options.ToString(), "yes", System.DateTime.Now.ToString ());

		} else if(go == cat2 || go == cat3){
			//sending data directly to firebase using "72 hours rule"! (removed local data storage)
			//DataCollection.AddInResponseData (go.name, cat1.name, options, false, System.DateTime.Now.ToString ());
			FirebaseHelper.LogInAppResponse(go.name,cat1.name, options.ToString(), "no", System.DateTime.Now.ToString ());
			Debug.Log ("not red");
		}


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

	}

}

