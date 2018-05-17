using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GTinkerGraphic : MonoBehaviour {
    public GameObjectClass dataTinkerGraphic;
   // private Animator anim;
    public GTinkerText pairedText1;
    public GTinkerText pairedText2;
	public GSManager sceneManager;
	private bool  draggable;
	public Canvas myCanvas;

	// Reset and highlight colors defaults (change from scene manager subclasses)
	public Color resetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color highlightColor = GameManager.yellow;

    // Use this for initialization
    void Start () {
        //anim = GetComponent<Animator>();
    }

   
	public void SetDraggable(bool value){
		draggable = value;
	}

	public bool GetDraggable(){
		return draggable;
	}
	/*
	public void MyOnMouseDown()
	{

        Debug.Log("clicked graphic" + this);
		sceneManager.OnMouseDown(this);
    }

	// Paired TinkerText Mouse Down Event
	public void OnPairedMouseDown(GTinkerText tinkerText)
	{
		sceneManager.OnPairedMouseDown(tinkerText);
	}

	// Mouse Currently Down Event
	public void OnMouseCurrentlyDown()
	{
		sceneManager.OnMouseCurrentlyDown(this);
	}

	// Paired TinkerText Mouse Down Event
	public void OnPairedMouseCurrentlyDown(GTinkerText tinkerText)
	{
		sceneManager.OnPairedMouseCurrentlyDown(tinkerText);
	}

	// Mouse Up Event
	public void MyOnMouseUp()
	{
		sceneManager.OnMouseUp(this);
	}
    
	// Paired TinkerText Mouse Up Event
	public void OnPairedMouseUp(GTinkerText tinkerText)
	{
		sceneManager.OnPairedMouseUp(tinkerText);
	}
		
*/
	public void MoveObject(){
		Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
        //transform.position = Input.mousePosition;
    }

	public Vector2 GetCoordinates(){
		return transform.position;
	}
}
