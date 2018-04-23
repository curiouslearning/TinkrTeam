using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager20 : SManager {


	public TinkerGraphic sheet;
	public GameObject target;
	private Vector2 currentPos, targetPos;
	float distance;


	public override void Start() {
		base.Start ();
		distance = 0.1f;   //change according to your need
		if (sheet != null) {
			sheet.SetDraggable(true);
			currentPos = sheet.GetCoordinates ();
			targetPos = target.transform.position;
		}
	}

	// Scene specific override (when play clicks on top shell, make it stick to wherever they move)
	public override void OnMouseDown(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseDown(tinkerGraphic);
		if (tinkerGraphic.GetDraggable ()) {
			dragActive = true;
		}
	}

	// Scene specific override (when play clicks on top shell, make it stick to wherever they move)
	public override void OnMouseCurrentlyDown(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseCurrentlyDown(tinkerGraphic);
		if (dragActive)
		{
			if (tinkerGraphic.GetDraggable ()) {
				sheet.MoveObject ();
			}
		}
	}

	// Scene specific override (when play clicks on top shell, make it stick to wherever they move)
	public override void OnMouseUp(TinkerGraphic tinkerGraphic)
	{
		base.OnMouseUp(tinkerGraphic);

		if (dragActive && tinkerGraphic.GetDraggable() )
		{
			dragActive = false;
			// See if player has moved shell far away enough to advance the scene
			currentPos = sheet.GetCoordinates();
			bool navigate= CheckNear (targetPos, currentPos, distance);
			if (navigate) {
				sheet.transform.position = targetPos;   
				NextScene ();
			}
		}
	}
}
