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
	public Sprite[] sprites;
	private int currentframe=0;
	private float SecPerFrame = 0.25f;
	private bool isLooping = false;
	public Movable movable; 
	public SpriteRenderer spr;

	// Reset and highlight colors defaults (change from scene manager subclasses)
	public Color resetColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public Color highlightColor = GameManager.yellow;

	private void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
	}

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
	
	public void MyOnMouseDown()
	{
		System.DateTime time=  System.DateTime.Now;
		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//DataCollection.AddInTouchData (dataTinkerGraphic.label, "graphic", time.ToString());
		FirebaseHelper.LogInAppTouch(dataTinkerGraphic.label, "graphic", time.ToString());

		if (dataTinkerGraphic.anim.Length > 0) {
			Anim anim = dataTinkerGraphic.anim[0];
			LoadAssetExample.LoadAssetImages(this, anim.animName, anim.numberOfImages);

			if (dataTinkerGraphic.anim [0].movable.speed != 0 && dataTinkerGraphic.anim [0].onTouch){
				movable = dataTinkerGraphic.anim [0].movable;
				PlayAnimation ( 0.25f, anim.isLooping);
			} else if (dataTinkerGraphic.anim [0].onTouch) {
				movable = null;
				PlayAnimation ( 0.25f, anim.isLooping);
			} 
		
		}

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
		

	public void MoveObject(){
		Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
        //transform.position = Input.mousePosition;
    }

	public Vector2 GetCoordinates(){
		return transform.position;
	}

	public void PlayAnimation( float secPerFrame, bool isLooping)
	{
		SecPerFrame = secPerFrame;
		this.isLooping = isLooping;
		StopCoroutine("AnimateandMove");
		StopCoroutine("AnimateSprite");
		currentframe = 0;
		if (movable != null) {
			StartCoroutine ("AnimateandMove");
		} else {
			StartCoroutine ("AnimateSprite");
		}
	}

	IEnumerator AnimateandMove()
	{   
		if(transform.position.x>=movable.finalx)
		{
			spr.sprite = sprites[sprites.Length -1];
			yield break;
		}
		yield return new WaitForSeconds(SecPerFrame);
		Debug.Log (spr.sprite);
		spr.sprite = sprites[currentframe];
		currentframe++;
		var posx = transform.position.x;
		posx += movable.speed;
		transform.position = new Vector3(posx, this.transform.position.y, 0);
		if (currentframe >= sprites.Length)
		{
			if (isLooping)
			{
				currentframe = 0;
			}
		}
		StartCoroutine("AnimateandMove");
	}

	IEnumerator AnimateSprite()
	{
			yield return new WaitForSeconds(SecPerFrame);
			spr.sprite = sprites[currentframe];
			currentframe++;
			if (currentframe >= sprites.Length)
			{
				if (isLooping)
				{
					currentframe = 0;
				}
				else
				{
				yield break;
				}
			}
			StartCoroutine("AnimateSprite");
		}
}
