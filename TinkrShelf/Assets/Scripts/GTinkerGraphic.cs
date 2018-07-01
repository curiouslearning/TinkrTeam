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
	public Color highlightColor = GGameManager.yellow;
	private Coroutine destroyObject;

	private void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
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
		//DataCollection.AddInTouchData (("Graphic_"+dataTinkerGraphic.label),  time.ToString());

		FirebaseHelper.LogInAppTouch(("Graphic_"+dataTinkerGraphic.label) ,  time.ToString());
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
	}

	public Vector2 GetCoordinates(){
		return transform.position;
	}

	void OnTriggerEnter (Collider col)
	{
		Debug.Log (""+col.name);
		if(col.gameObject.name == dataTinkerGraphic.destroyOnCollision)
		{
			destroyObject = StartCoroutine(DestroyCollisionObject (col.gameObject));
		}
	}

	public IEnumerator DestroyCollisionObject (GameObject go)
	{
			yield return new WaitForSeconds (secPerFrame[currentframe]+secPerFrame[currentframe+1]+secPerFrame[currentframe+2]+secPerFrame[currentframe + 3]);
			go.SetActive (false);
	}

	/// <summary>
	/// Loads the animation assets/frames and triggers PlayAnimation().
	/// </summary>
	public void LoadAndPlayAnimation(){

		if (dataTinkerGraphic.anim.Length > 0) {

			if (dataTinkerGraphic.anim [0].onTouch) {
				LoadAssetFromJSON.LoadAssetImages(this, dataTinkerGraphic.anim[0].animName, dataTinkerGraphic.anim[0].numberOfImages);
				secPerFrame = dataTinkerGraphic.anim [0].secPerFrame;
				sequences = dataTinkerGraphic.anim [0].sequences;
				PlayAnimation();

			} 

		}

	}

	/// <summary>
	/// Resets the graphic object and triggers the animation play.
	/// </summary>
	public void PlayAnimation(){
		//StopAllCoroutines();
		StopCoroutine("Animate");
		if (destroyObject != null) {
			StopCoroutine (destroyObject);
		}
		transform.position = new Vector3 (dataTinkerGraphic.posX, dataTinkerGraphic.posY);
		StartCoroutine("Animate");
	}

	/// <summary>
	/// Animate this instance with loaded animation frames.
	/// </summary>
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
			//animate for moving sequences of PNGs towards right.
			else if(transform.position.x < sequences [seqIterator].movable.finalx) {
				currentframe = sequences [seqIterator].startFrame;
				while (transform.position.x < sequences [seqIterator].movable.finalx) {
					spr.sprite = sprites[currentframe];
					yield return new WaitForSeconds(secPerFrame[currentframe]);
					currentframe++;
					var posx = transform.position.x;
					posx += sequences [seqIterator].movable.speed;
					transform.position = new Vector3(posx, this.transform.position.y, 0);

					//loop if we reached the end frame but not the final position!
					if (currentframe > sequences [seqIterator].endFrame)
					{
						currentframe = sequences [seqIterator].startFrame;
					} 

				}

				spr.sprite = sprites[sequences [seqIterator].endFrame];
			}
			//animate for moving sequences of PNGs towards left.
			else if(transform.position.x > sequences [seqIterator].movable.finalx) 
			{
				currentframe = sequences [seqIterator].startFrame;
				while (transform.position.x > sequences [seqIterator].movable.finalx) {
					spr.sprite = sprites[currentframe];
					yield return new WaitForSeconds(secPerFrame[currentframe]);
					currentframe++;
					var posx = transform.position.x;
					posx += sequences [seqIterator].movable.speed;
					transform.position = new Vector3(posx, this.transform.position.y, 0);

					//loop if we reached the end frame but not the final position!
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
}
