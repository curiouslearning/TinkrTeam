using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	// Use this for  initialization

	public SManager sceneManager;                       // Reference to SManager for each scene
	public static bool mousepressed = false;
	public Canvas myCanvas;
	[HideInInspector]
	public enum Scenes                                      // Place all scene names here in order
	{
		Init,
		Scene00,
		Scene01,
		Scene02,
		Scene03,
		Scene04,
		Scene05,
		Scene06,
		Scene07,
		Scene08,
		Scene09,
		Scene10,
		Scene11,
		Scene12,
		Scene13,
		Scene14,
		Scene15,
		Scene16,
		Scene17,
		Scene18,
		Scene19,
		Scene20,
		Scene21,
		Scene24,
		END
	}

	[HideInInspector]
	public enum MouseEvents
	{
		MouseDown,
		MouseCurrentlyDown,
		MouseUp
	}

	// Specific colors that TinkerBook uses
	static public Color red = new Color(238.0f / 255.0f, 35.0f / 255.0f, 39.0f / 255.0f, 1.0f);
	static public Color white = Color.white;
	static public Color orange = new Color(255.0f / 255.0f, 166.0f / 255.0f, 50.0f / 255.0f, 1.0f);
	static public Color purple = new Color(103.0f / 255.0f, 1.0f / 255.0f, 207.0f / 255.0f, 1.0f);
	static public Color brown = new Color(153.0f / 255.0f, 102.0f / 255.0f, 0 / 255.0f, 1.0f);
	static public Color yellow = new Color(237.0f / 255.0f, 245.0f / 255.0f, 84.0f / 255.0f, 1.0f);
	static public Color green = new Color(64.0f / 255.0f, 218.0f / 255.0f, 42.0f / 255.0f, 1.0f);
	static public Color blue = new Color(33.0f / 255.0f, 60.0f / 255.0f, 201.0f / 255.0f, 1.0f);
    static public Color navblue = new Color(33.0f / 255.0f, 60.0f / 255.0f, 221.0f / 255.0f, 0.9f);
    static public Color duckColor = white;
    static public Color frogColor = white;
    public Scenes currentScene;
	public static GameManager Instance
	{
		get { return GameManager.instance; }
	}
	// access to the singleton
	private static GameManager instance;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
		} 

		// Check for mouse input
		if (Input.GetMouseButtonDown(0))
		{
			// Check what was under mouse down (if anything)
			List<GameObject> gos = PickGameObjects(Input.mousePosition);

				// Pass the game object along to the current scene manager (if any) to let it respond
			if (sceneManager != null && gos.Count!=0) {
				sceneManager.OnMouseDown (gos[0]);
			}
		} 
		else if (Input.GetMouseButton(0))
		{
			// Check what was under mouse down (if anything)
			List<GameObject> gos = PickGameObjects(Input.mousePosition);
				// Pass the game object along to the current scene manager (if any) to let it respond
			if (sceneManager != null && gos.Count!=0)
				{
					sceneManager.OnMouseCurrentlyDown(gos[0]);
				}

			if (gos.Count == 0)
			{
				// Anytime a mouse currently down event misses any gameobject, update applicable lists in scene manager
				sceneManager.ResetInputStates(MouseEvents.MouseCurrentlyDown);
			}
		} 
		else if (Input.GetMouseButtonUp(0))
		{
			// Check what was under mouse down (if anything)
			List<GameObject> gos = PickGameObjects(Input.mousePosition);

				// Pass the game object along to the current scene manager (if any) to let it respond
			if (sceneManager != null && gos.Count!=0) {
					sceneManager.OnMouseUp (gos[0]);
				}

			// Anytime there is a mouse up event, update applicable lists in scene manager
			sceneManager.ResetInputStates(MouseEvents.MouseUp);
		} 

		else if (Input.GetKeyDown(KeyCode.Escape)) // quit game on exit
		{
			System.Diagnostics.Process.GetCurrentProcess().Kill();

		}
	}




	// this is called after Awake() OR after the script is recompiled (Recompile > Disable > Enable)
	private void Init()
	{
		// Assign our current scene on one-time init so we can support starting game from any scene during testing
		currentScene = (Scenes)Enum.Parse(typeof(Scenes), SceneManager.GetActiveScene().name);
	}

	protected virtual void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		if (instance == null)
		{
			Debug.Log("enable");
			instance = this;

			Init();
		}
		else if (instance != this)
		{
			Debug.LogWarning("GAME MANAGER: WARNING - THERE IS ALREADY AN INSTANCE OF GAME MANAGER RUNNING - DESTROYING THIS ONE.");
			Destroy(this.gameObject);
			return;
		}
	}
	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// Called each time a new scene is loaded
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("LEVEL WAS LOADED: " + SceneManager.GetActiveScene().name);
		//AndroidBroadcastIntentHandler.BroadcastJSONData("scene", SceneManager.GetActiveScene().name);
		LoadSceneManager();
	}


	private void LoadSceneManager()
	{
		// Grab the current SManager GameObject (if it exists)
		GameObject sceneManagerGO = GameObject.Find("SceneManager");


		if (sceneManagerGO != null)
		{
			sceneManager = sceneManagerGO.GetComponent<SManager>();

			if (sceneManager != null)
			{
				sceneManager.Init(this);

			}
		}
	}
		


	public void LoadPreviousScene()
	{
		if (currentScene > Scenes.Init + 1)
		{
			currentScene--;
			SceneManager.LoadScene(currentScene.ToString());
		}
	}
	public void LoadNextScene()
	{
		if (currentScene < Scenes.END)
		{
			currentScene++;
			SceneManager.LoadScene(currentScene.ToString());
			Debug.Log(currentScene);
		}
	}


	private List<GameObject> PickGameObjects( Vector3 screenPos )
	{
		List<GameObject> gameObjects = new List<GameObject>();
		Vector3 localPos = Camera.main.ScreenToViewportPoint (screenPos);
		Ray ray = Camera.main.ViewportPointToRay (localPos);

		RaycastHit[] hits;
		hits = Physics.RaycastAll (ray, Mathf.Infinity);

		foreach (RaycastHit hit in hits)
		{
			
			gameObjects.Add(hit.collider.gameObject);
		}

		// Now sort all GameObjects by Z pos ascending
		gameObjects.Sort(CompareZPosition);
		return gameObjects;
	}


	// Used for gameobject z-sorting ascending
	private static int CompareZPosition(GameObject a, GameObject b)
	{
		if (a.transform.localPosition.z < b.transform.localPosition.z)
			return -1;
		else if (a.transform.localPosition.z > b.transform.localPosition.z)
			return 1;
		else
			return 0;
	}

}
