using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAnimSwitching:MonoBehaviour {
    public GameObject sceneManager;
	public static float pondStartX=-1.1f;
	public static float pondStartY= -0.7f;
	public static float pondEndX=1.3f;
	public static float pondEndY=-0.3f;
	public static float diveStartX=0.6f;
	public static float diveEndX=1.1f;
	public GameObject dive;
	public GameObject sideLeaf;
	private Vector2 pos;
	public bool insideWater = false;
	bool running =false;
	// Use this for initialization
	public void Start () {
       // base.Start();
        //idle on land
		this.transform.GetChild (0).gameObject.SetActive (true); 
		this.transform.GetChild (1).gameObject.SetActive (false); 
		this.transform.GetChild (2).gameObject.SetActive (false);   
		this.transform.GetChild (3).gameObject.SetActive (false); 
		this.transform.GetChild (4).gameObject.SetActive (false); 
		dive.gameObject.SetActive (false); 
	}
	
	public void DuckOnMouseDown(){
		pos = this.transform.position;
		if ((pos.x >= pondStartX && pos.x <= pondEndX) && (pos.y >= pondStartY && pos.y <= pondEndY)) {

			// swim active, others inactive
			this.transform.GetChild (0).gameObject.SetActive (false);   //idle
			this.transform.GetChild (1).gameObject.SetActive (false);   //run
			this.transform.GetChild (2).gameObject.SetActive (false);   //idle pond
			this.transform.GetChild (3).gameObject.SetActive (false);   //splash
			this.transform.GetChild (4).gameObject.SetActive (true);    //swim
			this.transform.GetChild (4).gameObject.GetComponent<TinkerGraphic> ().MyOnMouseDown ();
			dive.gameObject.SetActive (false);  //dive
			insideWater = true;
		} else {
			//run active, other inactive
			this.transform.GetChild (0).gameObject.SetActive (false);
			this.transform.GetChild (1).gameObject.SetActive (true);   
			this.transform.GetChild (2).gameObject.SetActive (false);
			this.transform.GetChild (3).gameObject.SetActive (false);
			this.transform.GetChild (4).gameObject.SetActive (false);
			this.transform.GetChild (1).gameObject.GetComponent<TinkerGraphic> ().MyOnMouseDown ();
			dive.gameObject.SetActive (false); 
			running = true;
		}
	}

	public void DuckOnMouseCurrentlyDown(){
		pos = this.transform.position;
		if ((pos.x >= pondStartX && pos.x <= pondEndX) && (pos.y >= pondStartY && pos.y <= pondEndY)) {
			//swim
			this.transform.GetChild (0).gameObject.SetActive (false); 
			this.transform.GetChild (1).gameObject.SetActive (false);   
			this.transform.GetChild (2).gameObject.SetActive (false);   
			this.transform.GetChild (3).gameObject.SetActive (false);  
			this.transform.GetChild (4).gameObject.SetActive (true);   
			dive.gameObject.SetActive (false);   
			insideWater = true;
			if (running) {
				running = false;
				this.transform.GetChild (4).gameObject.GetComponent<TinkerGraphic> ().MyOnMouseDown ();
			}
		} else {
			//run active, other inactive
			this.transform.GetChild (0).gameObject.SetActive (false);
			this.transform.GetChild (1).gameObject.SetActive (true);   
			this.transform.GetChild (2).gameObject.SetActive (false);
			this.transform.GetChild (3).gameObject.SetActive (false);
			this.transform.GetChild (4).gameObject.SetActive (false);
			dive.gameObject.SetActive (false); 
			if (!running) {
				running = true;
				this.transform.GetChild (1).gameObject.GetComponent<TinkerGraphic> ().MyOnMouseDown ();
			}
		}
	}
		
	public void DuckOnMouseUp(){
		pos = this.transform.position;
		if ((pos.x >= diveStartX && pos.x <= diveEndX) && (pos.y >= pondStartY && pos.y <= pondEndY)) {

            //dive once
            Debug.Log("dive");
            StartCoroutine(sceneManager.GetComponent<SManager>().PlayNonLoopSound(0,1f));
			this.transform.GetChild (0).gameObject.SetActive (false); 
			this.transform.GetChild (1).gameObject.SetActive (false);
			this.transform.GetChild (2).gameObject.SetActive (false);
			this.transform.GetChild (3).gameObject.SetActive (false);  
			this.transform.GetChild (4).gameObject.SetActive (false);   
			dive.gameObject.SetActive (true); 
			sideLeaf.SetActive (false);
			dive.gameObject.GetComponent<TinkerGraphic> ().MyOnMouseDown ();
			StartCoroutine (SetDiveFalse());
		} else if ((pos.x >= pondStartX && pos.x <= pondEndX) && (pos.y >= pondStartY && pos.y <= pondEndY)) {

			//idle in pond
			this.transform.GetChild (0).gameObject.SetActive (false); 
			this.transform.GetChild (1).gameObject.SetActive (false); 
			this.transform.GetChild (2).gameObject.SetActive (true);   
			this.transform.GetChild (3).gameObject.SetActive (false); 
			this.transform.GetChild (4).gameObject.SetActive (false); 
			dive.gameObject.SetActive (false); 
		} else {
			//idle on land
			this.transform.GetChild (0).gameObject.SetActive (true); 
			this.transform.GetChild (1).gameObject.SetActive (false); 
			this.transform.GetChild (2).gameObject.SetActive (false);   
			this.transform.GetChild (3).gameObject.SetActive (false); 
			this.transform.GetChild (4).gameObject.SetActive (false); 
			dive.gameObject.SetActive (false); 
			insideWater = false;
		}
	}

	public IEnumerator SetDiveFalse(){

		yield return new WaitForSeconds (3.5f);
		dive.gameObject.SetActive (false);
		sideLeaf.SetActive (true);
		this.transform.position = new Vector2(0.0f, -0.5f);
		this.transform.GetChild (2).gameObject.SetActive (true);
		yield break;

	}
}
