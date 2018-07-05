using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script responsible for controling scenes,touch events and dropdown menu.
/// </summary>
public class GGameManager : MonoBehaviour
{
	// Reference to GSManager
	public GSManager sceneManager;
	public GStanzaManager stanzaManager;
	public static bool mousepressed = false;
	public Canvas myCanvas;
	static public Color yellow = new Color (237.0f / 255.0f, 245.0f / 255.0f, 84.0f / 255.0f, 1.0f);

	//Mouse touch event references
	[HideInInspector]
	public enum MouseEvents
	{
		MouseDown,
		MouseCurrentlyDown,
		MouseUp
	}

	// Specific colors that TinkerBook uses
	
	static public Color yellow = new Color(237.0f / 255.0f, 245.0f / 255.0f, 84.0f / 255.0f, 1.0f);
	

	public static AudioSource[] sounds;


	public static GGameManager Instance
	{
		get { return GGameManager.instance; }
	}
	// access to the singleton
	private static GGameManager instance;
    
    // Update is called once per frame
    public void Start()
	{
        
			if (gameObject != null)
				sounds = gameObject.GetComponents<AudioSource>();


	}

	/// <summary>
	/// Checks the mouse events and calls the respective scenemanager event.
	/// </summary>
	void Update ()
	{
		// Check for mouse input
		if (Input.GetMouseButtonDown (0)) {
			// Check what was under mouse down (if anything)
			List<GameObject> gos = PickGameObjects (Input.mousePosition);

			// Pass the game object along to the current scene manager (if any) to let it respond
			if (sceneManager != null && gos.Count != 0) {
				sceneManager.OnMouseDown (gos [0]);
			}
		} else if (Input.GetMouseButton (0)) {
			// Check what was under mouse down (if anything)
			List<GameObject> gos = PickGameObjects (Input.mousePosition);
			// Pass the game object along to the current scene manager (if any) to let it respond
			if (sceneManager != null && gos.Count != 0) {
				sceneManager.OnMouseCurrentlyDown (gos [0]);
			}
			if (gos.Count == 0) {
				// Anytime a mouse currently down event misses any gameobject, update applicable lists in scene manager
				sceneManager.ResetInputStates (MouseEvents.MouseCurrentlyDown);
			}
		} else if (Input.GetMouseButtonUp (0)) {
			// Check what was under mouse down (if anything)
			List<GameObject> gos = PickGameObjects (Input.mousePosition);
			// Pass the game object along to the current scene manager (if any) to let it respond
			if (sceneManager != null && gos.Count != 0) {
				sceneManager.OnMouseUp (gos [0]);
			}
			// Anytime there is a mouse up event, update applicable lists in scene manager
			sceneManager.ResetInputStates (MouseEvents.MouseUp);
		}

		// quit game on exit
		else if (Input.GetKeyDown (KeyCode.Escape)) {
			System.Diagnostics.Process.GetCurrentProcess ().Kill ();
		}
	}

	// this is called after Awake() OR after the script is recompiled (Recompile > Disable > Enable)
	protected virtual void OnEnable ()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		if (instance == null) {
			Debug.Log ("enable");
			instance = this;
		} else if (instance != this) {
			Debug.LogWarning ("GAME MANAGER: WARNING - THERE IS ALREADY AN INSTANCE OF GAME MANAGER RUNNING - DESTROYING THIS ONE.");
			Destroy (this.gameObject);
			return;
		}
	}

	void OnDisable ()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// Called each time a new scene is loaded
	void OnSceneLoaded (Scene scene, LoadSceneMode mode)
	{
		Debug.Log ("LEVEL WAS LOADED: " + SceneManager.GetActiveScene ().name);
		//AndroidBroadcastIntentHandler.BroadcastJSONData("scene", SceneManager.GetActiveScene().name);
		LoadSceneManager ();
	}


	private void LoadSceneManager ()
	{
		sceneManager.Start ();
	}

	/// <summary>
	///Down button is clicked.
	/// </summary>
	public void DownClick ()
	{
		DateTime time = DateTime.Now;

		//sending data directly to firebase using "72 hours rule"! (removed local data storage)

	}

	/// <summary>
	///Up button is clicked.
	/// </summary>
	public void UpArrowClick ()
	{
		DateTime time = DateTime.Now;
		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//DataCollection.AddInTouchData ("Button_UpMenu", time.ToString());
	}

	/// <summary>
	///When home button is clicked,it goes to the shelf.
	/// </summary>
	public void MenuClick ()
	{
		DateTime time = DateTime.Now;
		//sending data directly to firebase using "72 hours rule"! (removed local data storage)
		//DataCollection.AddInTouchData ("Button_Home", time.ToString());
		FirebaseHelper.LogInAppTouch("Button_Home",  time.ToString());

        SceneManager.LoadScene ("shelf");

	}
    public void AutoNarrate()
	{
		DateTime time = DateTime.Now;
    
        if(ShelfManager.autoNarrate)
        stanzaManager.RequestAutoPlay(stanzaManager.stanzas[0], stanzaManager.stanzas[0].tinkerTexts[0]);
	}     


	/// <summary>
	/// Raycast to find the gameobject positon
	/// </summary>
	/// <param name="screenPos">Mouse click screen position.</param>
	private List<GameObject> PickGameObjects (Vector3 screenPos)
	{
		List<GameObject> gameObjects = new List<GameObject> ();
		Vector3 localPos = Camera.main.ScreenToViewportPoint (screenPos);
		Ray ray = Camera.main.ViewportPointToRay (localPos);
		RaycastHit[] hits;
		hits = Physics.RaycastAll (ray, Mathf.Infinity);
		foreach (RaycastHit hit in hits) {

			gameObjects.Add (hit.collider.gameObject);
		}

		// Now sort all GameObjects by Z pos ascending
		gameObjects.Sort (CompareZPosition);
		return gameObjects;
	}

	/// <summary>
	/// Used for gameobject z-sorting ascending.
	/// </summary>
	/// <param name="a">first gameobject.</param>
	/// <param name="b">second gameobject.</param>
	private static int CompareZPosition (GameObject a, GameObject b)
	{
		if (a.transform.localPosition.z < b.transform.localPosition.z)
			return -1;
		else if (a.transform.localPosition.z > b.transform.localPosition.z)
			return 1;
		else
			return 0;
	}


}
