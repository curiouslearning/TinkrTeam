using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager16 : SManager {

	public TinkerGraphic worm1;
	public TinkerGraphic worm2;
	public TinkerGraphic worm3;
	public TinkerGraphic worm4;
	public TinkerGraphic worm5;
	public TinkerGraphic worm6;
	public TinkerGraphic worm7;
	public TinkerGraphic worm8;

	public GameObject duckParent;
	public GameObject duckIdle;
	public GameObject duckChew;
	public GameObject duckMouthOpen;

	private Vector2 currentPos;

	private Text t;
	Vector2 pos;

	// Use this for initialization
	public override void Start () {
        base.Start();
		if (worm1 != null ) {
			worm1.SetDraggable (true);
			currentPos = worm1.GetCoordinates ();
		}
		if (worm2 != null ) 
			worm2.SetDraggable (true);
		if (worm3 != null ) 
			worm3.SetDraggable (true);
		if (worm4 != null ) 
			worm4.SetDraggable (true);
		if (worm5 != null ) 
			worm5.SetDraggable (true);
		if (worm6 != null ) 
			worm6.SetDraggable (true);
		if (worm7 != null ) 
			worm7.SetDraggable (true);
		if (worm8 != null ) 
			worm8.SetDraggable (true);
	}

	public override void OnMouseDown(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseDown (tinkerGraphic);

		if (tinkerGraphic.GetDraggable ()) {
			dragActive = true;
			t= tinkerGraphic.gameObject.GetComponentInChildren<Text>();
			t.text= ""+DuckBugFeedSwitching.bugCounter;
		}


	}

	public override void OnMouseCurrentlyDown(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseCurrentlyDown(tinkerGraphic);
		if (dragActive)
		{
			if (tinkerGraphic.GetDraggable ()) {
				tinkerGraphic.MoveObject ();
				t.text= ""+DuckBugFeedSwitching.bugCounter;
			}

		}

	}

	public override void OnMouseUp(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseUp (tinkerGraphic);

		if (dragActive && tinkerGraphic.GetDraggable ()) {
			dragActive = false;
			t.text = "";
			duckIdle.SetActive (true);
			duckMouthOpen.SetActive (false);
			duckChew.SetActive (false);

		}
		if(tinkerGraphic.gameObject.name=="duck_parent")
		duckParent.GetComponent<DuckBugFeedSwitching> ().FeedDuckOnMouseUp();

	}


		
}
