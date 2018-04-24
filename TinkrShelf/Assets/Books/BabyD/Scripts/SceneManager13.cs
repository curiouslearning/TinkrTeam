using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager13 : SManager {

	public TinkerGraphic duckParent;
	public GameObject pond_and_grass;
	public GameObject runAnim;
	public GameObject dive;

	// Use this for initialization
	public override void Start () {
        base.Start();
		if (duckParent != null) {
			duckParent.GetComponent<TinkerGraphic>().SetDraggable (true);
		}
	}

	public override void OnMouseDown(TinkerGraphic tinkerGraphic)
	{
        
		Vector2 pos;
		base.OnMouseDown(tinkerGraphic);

		if (tinkerGraphic.GetDraggable ()) {
			if (Input.GetAxis ("Mouse X") < 0)
			{
				tinkerGraphic.transform.rotation = Quaternion.Euler (0, 180, 0);
			}
			if (Input.GetAxis ("Mouse X") > 0)
			{
				tinkerGraphic.transform.rotation = Quaternion.Euler (0, 0, 0);
			}
			dragActive = true;
		}

		if (tinkerGraphic == duckParent) {
			duckParent.GetComponent<DuckAnimSwitching> ().DuckOnMouseDown ();
		}

		if (tinkerGraphic.name == "pond_and_grass") {
			
			RectTransformUtility.ScreenPointToLocalPointInRectangle(tinkerGraphic.myCanvas.transform as RectTransform, Input.mousePosition, tinkerGraphic.myCanvas.worldCamera, out pos);
			pos = tinkerGraphic.myCanvas.transform.TransformPoint(pos);
			//if in water
			if ( (duckParent.gameObject.GetComponent<DuckAnimSwitching>().insideWater) && (pos.x < 0.6f)) {
				//splash
				duckParent.transform.GetChild (0).gameObject.SetActive (false); 
				duckParent.transform.GetChild (1).gameObject.SetActive (false);   
				duckParent.transform.GetChild (2).gameObject.SetActive (false);
                duckParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);  //splash active
                duckParent.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(false); //splash active
                duckParent.transform.GetChild(3).gameObject.SetActive(true);
				duckParent.transform.GetChild (4).gameObject.SetActive (false);  
				dive.gameObject.SetActive (false);  
				duckParent.transform.GetChild (3).gameObject.GetComponent<TinkerGraphic> ().MyOnMouseDown ();
                StartCoroutine(PlayNonLoopSound(1));
                //StartCoroutine (SetIdleAfterSplash());
			} else {
			 //bad dive
			}
		}

	}


	public override void OnMouseCurrentlyDown(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseCurrentlyDown(tinkerGraphic);
		if (dragActive)
		{
			if (tinkerGraphic.GetDraggable ()) {
				
				if (Input.GetAxis ("Mouse X") < 0)
				{
					tinkerGraphic.transform.rotation = Quaternion.Euler (0, 180, 0);
				}
				if (Input.GetAxis ("Mouse X") > 0)
				{
					tinkerGraphic.transform.rotation = Quaternion.Euler (0, 0, 0);
				}
				duckParent.GetComponent<TinkerGraphic>().MoveObject ();
			}
            Vector2 pos;
            if (tinkerGraphic.name == "pond_and_grass")
            {

                RectTransformUtility.ScreenPointToLocalPointInRectangle(tinkerGraphic.myCanvas.transform as RectTransform, Input.mousePosition, tinkerGraphic.myCanvas.worldCamera, out pos);
                pos = tinkerGraphic.myCanvas.transform.TransformPoint(pos);
                if ((duckParent.gameObject.GetComponent<DuckAnimSwitching>().insideWater) && (pos.x < 0.6f))
                {
                    //splash
                    duckParent.transform.GetChild(0).gameObject.SetActive(false);
                    duckParent.transform.GetChild(1).gameObject.SetActive(false);
                    duckParent.transform.GetChild(2).gameObject.SetActive(false);
                    duckParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);  //splash active
                    duckParent.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(false); //splash active
                    //duckParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponentInChildren<Animation>().
                    duckParent.transform.GetChild(3).gameObject.SetActive(true);
                    duckParent.transform.GetChild(4).gameObject.SetActive(false);
                    dive.gameObject.SetActive(false);
                    duckParent.transform.GetChild(3).gameObject.GetComponent<TinkerGraphic>().MyOnMouseDown();
                    StartCoroutine(SetIdleAfterSplash());
                }
            }
            if (tinkerGraphic == duckParent) {
				
				duckParent.GetComponent<DuckAnimSwitching> ().DuckOnMouseCurrentlyDown ();

			}
		}
	}

	public override void OnMouseUp(TinkerGraphic tinkerGraphic)
	{
		Debug.Log ("called");
		base.OnMouseUp(tinkerGraphic);
		if (dragActive && tinkerGraphic.GetDraggable() )
		{
			dragActive = false;
		}
		if (tinkerGraphic == duckParent) {

			duckParent.GetComponent<DuckAnimSwitching> ().DuckOnMouseUp ();
		}
        Vector2 pos;
        if (tinkerGraphic.name == "pond_and_grass")
        {

            RectTransformUtility.ScreenPointToLocalPointInRectangle(tinkerGraphic.myCanvas.transform as RectTransform, Input.mousePosition, tinkerGraphic.myCanvas.worldCamera, out pos);
            pos = tinkerGraphic.myCanvas.transform.TransformPoint(pos);
            if ((duckParent.gameObject.GetComponent<DuckAnimSwitching>().insideWater) && (pos.x < 0.6f))
            {
                //splash
                duckParent.transform.GetChild(0).gameObject.SetActive(false);
                duckParent.transform.GetChild(1).gameObject.SetActive(false);
                duckParent.transform.GetChild(2).gameObject.SetActive(false);
                
                duckParent.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);  //splash active
                duckParent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false); //splash active
                duckParent.transform.GetChild(3).gameObject.SetActive(true);
                duckParent.transform.GetChild(4).gameObject.SetActive(false);
                dive.gameObject.SetActive(false);
                //duckParent.transform.GetChild(3).gameObject.GetComponent<TinkerGraphic>().OnMouseDown();
                StartCoroutine(PlayNonLoopSound(2));
                StartCoroutine(SetIdleAfterSplash());
            }
        }
    }

	public IEnumerator SetIdleAfterSplash(){

		yield return new WaitForSeconds (1.0f);
		duckParent.transform.GetChild (3).gameObject.SetActive (false);
		duckParent.transform.GetChild (2).gameObject.SetActive (true);
		yield break;
	}
		
}
