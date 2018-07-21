using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class L3CatSManager3 : GSManager {
    GTinkerGraphic ranman;
    GTinkerGraphic cat;
    public override void Start()
    {
       ranman = GameObject.Find("Ran").GetComponent<GTinkerGraphic>();
       cat = GameObject.Find("Cat").GetComponent<GTinkerGraphic>();
    }
	public override void OnMouseDown(GameObject go)
	{
		if (go.name == "Cat") {
			if (ranman != null) {
				ranman.MyOnMouseDown();
			}
		} else if (go.name == "Ran") {
			if (cat != null) {
				cat.MyOnMouseDown();
			} 

		} else if (go.name == "Text_ran.") {
			if (ranman != null && cat!=null) {
				ranman.MyOnMouseDown ();
				cat.MyOnMouseDown ();
			}

		}
        else if(go.name=="Text_cat")
        {
            ranman.reset();
        }
		base.OnMouseDown (go);
	}
}