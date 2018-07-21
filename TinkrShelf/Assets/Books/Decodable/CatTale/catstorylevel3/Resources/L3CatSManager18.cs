using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L3CatSManager18 : GSManager {
    GameObject Jam;
    GTinkerGraphic Cat;
    public override void Start()
    {
        Jam = GameObject.Find("Jam");
        Cat=GameObject.Find("Cat").GetComponent<GTinkerGraphic>();
        base.Start();
    }

    public override void OnMouseDown(GameObject go)
	{
		if (go.name == "Text_jam.") 
		{
            Jam.SetActive(true);
            Cat.reset();
		}
		base.OnMouseDown(go);

	}
}
