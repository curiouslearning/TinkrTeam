using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GTinkerGraphic : MonoBehaviour{
    public GameObjectClass dataTinkerGraphic;
   // private Animator anim;
    public GTinkerText pairedText1;
    public GTinkerText pairedText2;
	public GSManager sceneManager;
	public Canvas myCanvas;

	public Sprite[] sprites;
	private int currentframe=0;
	public SpriteRenderer spr;

	public Sequence[] sequences;
	private int seqIterator;
	public float[] secPerFrame;

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
		dataTinkerGraphic.draggable = value;
	}

	public bool GetDraggable(){
		return dataTinkerGraphic.draggable;
	}
	
	public void MyOnMouseDown()
	{
		System.DateTime time=  System.DateTime.Now;

		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//DataCollection.AddInTouchData (dataTinkerGraphic.label, "graphic", time.ToString());

		FirebaseHelper.LogInAppTouch(dataTinkerGraphic.label, "graphic", time.ToString());
		LoadAndPlayAnimation ();
		sceneManager.OnMouseDown(this);
    }



	// Paired TinkerText Mouse Down Event
	public void OnPairedMouseDown(GTinkerText tinkerText)
	{
		sceneManager.OnPairedMouseDown(tinkerText);
		LoadAndPlayAnimation ();
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


	public void LoadAndPlayAnimation(){

		if (dataTinkerGraphic.anim.Length > 0) {

			if (dataTinkerGraphic.anim [0].onTouch) {

				LoadAssetExample.LoadAssetImages(this, dataTinkerGraphic.anim[0].animName, dataTinkerGraphic.anim[0].numberOfImages);
				secPerFrame = dataTinkerGraphic.anim [0].secPerFrame;
				sequences = dataTinkerGraphic.anim [0].sequences;
				PlayAnimation();

			} 

		}
	
	}

	public void PlayAnimation(){
		
		StopCoroutine ("Animate");
		StartCoroutine("Animate");
	}


	IEnumerator Animate()
	{
		currentframe = 0;
		int i = 1;

		for (seqIterator = 0; seqIterator < sequences.Length; seqIterator++) {
			
			//animate for non moving sequences of PNGs
			if (sequences [seqIterator].movable.speed == 0 ) {
				i = 1;       //count the number of loops from start for every sequence!
				while (i <= sequences [seqIterator].noOfLoops) {
					for (currentframe = sequences [seqIterator].startFrame; currentframe <= sequences [seqIterator].endFrame; currentframe++) {
						spr.sprite = sprites [currentframe];
						yield return new WaitForSeconds (secPerFrame [currentframe]);
					}
					i++;

				}
			}
			//animate for nmoving sequences of PNGs
			else {
				currentframe = sequences [seqIterator].startFrame;
				while (transform.position.x < sequences [seqIterator].movable.finalx) {
					spr.sprite = sprites[currentframe];
					yield return new WaitForSeconds(secPerFrame[currentframe]);
					currentframe++;
					var posx = transform.position.x;
					posx += sequences [seqIterator].movable.speed;
					transform.position = new Vector3(posx, this.transform.position.y, 0);

					//loop if we reached the end frame but not the final posiiton!
					if (currentframe > sequences [seqIterator].endFrame)
					{
						currentframe = sequences [seqIterator].startFrame;
					} 
				
				}

				spr.sprite = sprites[sequences [seqIterator].endFrame];
			}


		}
		yield break;
	}

	/*IEnumerator AnimateAndMove()
	{   
		if(transform.position.x>=movable.finalx)
		{
			spr.sprite = sprites[sprites.Length -1];
			yield break;
		}
		spr.sprite = sprites[currentframe];
		Debug.Log ("waited for move:"+secPerFrame[currentframe]);
		yield return new WaitForSeconds(secPerFrame[currentframe]);
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
		StartCoroutine("AnimateAndMove");
	}

	IEnumerator AnimateSprite()
	{
		    spr.sprite = sprites[currentframe];
	    	Debug.Log ("waited for sprite:"+secPerFrame[currentframe]);
		    yield return new WaitForSeconds(secPerFrame[currentframe]);
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
		}*/
}
