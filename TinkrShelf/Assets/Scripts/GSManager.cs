using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GSManager :  MonoBehaviour {


	[HideInInspector]
	public GGameManager gameManager;
	public Canvas myCanvas;

	// Manager for all TinkerTexts and stanza
	public GStanzaManager stanzaManager;
	//public List<StanzaObject> stanzas;

    // Whether to allow input on text/graphics during autoplay
    public bool inputAllowedDuringAutoplay = true;

	// Whether to interrupt auto play if a single word is hit
	public bool inputInterruptsAutoplay = true;

	// Disable auto play?
	[HideInInspector]
	public bool disableAutoplay = false;

	// Disable sounds?
	[HideInInspector]
	public bool disableSounds = false;

	// Drag event active?
	[HideInInspector]
	public bool dragActive = false;

    private int countDownEvent = 0;
    public static AudioSource[] sounds;


	//for menubar drop down

	public int i = 1;
	public static int j = 1; 
	static public Color blue = new Color(6.0f / 255.0f, 7.0f / 255.0f, 253.0f / 255.0f, 81.0f);
	static public Color red = new Color(253.0f / 255.0f, 6.0f / 255.0f, 52.0f / 255.0f, 255.0f);
	static public Color yellow = new Color(237.0f / 255.0f, 243.0f / 255.0f, 0.0f / 255.0f, 249.0f);


	public GameObject Lbutton;
	public GameObject Rbutton;


 

	public virtual void Start() //GGameManager _gameManager
	{
		//gameManager = _gameManager;

		// Reset flags
		dragActive = false;
		disableAutoplay = false;
		disableSounds = false;

		// If we have a stanza manager
		if (stanzaManager != null)
		{
			// And it has an audio clip and xml defined already in the scene
			if (LoadAssetExample.storyBookJson.pages[LoadAssetExample.pageNumber].timestamps.Length >0) //&& stanzaManager.GetComponent<AudioSource>().clip != null)
			{
				// Then have it set the xml up
				stanzaManager.LoadStanzaJSON();
			}
		}
		Lbutton.gameObject.GetComponent<Image>().color = GGameManager.navblue;

		Color c = Rbutton.gameObject.GetComponent<Image>().color;
		c.a = 0.8f;
		Rbutton.gameObject.GetComponent<Image>().color = c;

		//Lbutton.GetComponent<Button>().interactable = false;
		Rbutton.GetComponent<Button>().interactable = false;

	}

	
	public bool IsInputAllowed()
	{
		if (inputAllowedDuringAutoplay)
		{
			return true;
		}
		else if (stanzaManager != null)
		{
			return !stanzaManager.IsAutoPlaying();   
		}

		return true; // stanza manager must be null
	}

	// Here we have a superclass intercept for catching global GameObject mouse down events
	public virtual void OnMouseDown(GameObject go)
	{   
		countDownEvent++;
		Debug.Log (countDownEvent);
		if (countDownEvent == 2)
			EnableButtons();
            // Lock out other input during auto play?
            if (IsInputAllowed())
            {
                // TinkerText object 
                if (go.GetComponent<GTinkerText>() != null)
                {
                
                    GTinkerText tinkerText = go.GetComponent<GTinkerText>();

                    if (tinkerText != null)
                    {
                        if (stanzaManager != null)
                        {
                            // Is an autoplay in progress? If so, see if we should interrupt
                            if (stanzaManager.IsAutoPlaying() && inputInterruptsAutoplay)
              
						{
                                stanzaManager.RequestCancelAutoPlay();
                            }

                            stanzaManager.OnMouseDown(tinkerText);
                        }
                    }
                }
                // TinkerGraphic object
                else if (go.GetComponent<GTinkerGraphic>() != null)
			{

                    GTinkerGraphic tinkerGraphic = go.GetComponent<GTinkerGraphic>();
                Debug.Log("clicked 3");
                    if (tinkerGraphic != null)
                    {
                    Debug.Log("clicked 4");
                        tinkerGraphic.MyOnMouseDown();
                    }
                }

				
            }

		
        

	}
	public void MoveObject(){
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
		transform.position = myCanvas.transform.TransformPoint(pos);
	}

	public Vector2 GetCoordinates(){
		return transform.position;
	}

	// Here we have a superclass intercept for catching global TinkerGraphic mouse down events
	public virtual void OnMouseDown(GTinkerGraphic tinkerGraphic)
	{
		
		if (tinkerGraphic.pairedText1 != null)
		{
			stanzaManager.OnPairedMouseDown(tinkerGraphic.pairedText1);
		}
	}

	// Here we have a superclass intercept for catching global TinkerText paired mouse down events
	public virtual void OnPairedMouseDown(GTinkerText tinkerText)
	{
		Renderer[] list;
		list = tinkerText.pairedGraphic.gameObject.GetComponentsInChildren<Renderer>();
		foreach(Renderer item in list){
			if (item.name == "ripple") //don't color ripple in scene 13 attached to BabyD.
				continue;
			item.material.color = tinkerText.pairedGraphic.highlightColor;
		 }
       
	}

	// Here we have a superclass intercept for catching global GameObject mouse currently down events
	public virtual void OnMouseCurrentlyDown(GameObject go)
	{
        Debug.Log("current down");
		// Lock out other input during auto play?
		if (IsInputAllowed())
		{
			// TinkerGraphic object
			if (go.GetComponent<GTinkerGraphic>() != null)
			{
				GTinkerGraphic tinkerGraphic = go.GetComponent<GTinkerGraphic>();

				if (tinkerGraphic != null)
				{
					tinkerGraphic.OnMouseCurrentlyDown();
				}
			}
			// TinkerText object
			else if (!dragActive && go.GetComponent<GTinkerText>() != null)
			{
				GTinkerText tinkerText = go.GetComponent<GTinkerText>();

				if (tinkerText != null)
				{
					if (stanzaManager != null)
					{
						// Only allow further drag events if we aren't autoplaying
						if (!stanzaManager.IsAutoPlaying())
						{
							stanzaManager.OnMouseCurrentlyDown(tinkerText);
						}
					}
				}
			}
		}
	}

	// Here we have a superclass intercept for catching global TinkerGraphic mouse currently down events
	public virtual void OnMouseCurrentlyDown(GTinkerGraphic tinkerGraphic)
	{
        if (tinkerGraphic.GetDraggable())
        {
            tinkerGraphic.MoveObject();
        }
        // override me
    }

	// Here we have a superclass intercept for catching global TinkerText paired mouse currently down events
	public virtual void OnPairedMouseCurrentlyDown(GTinkerText tinkerText)
	{
		// override me
	}

	// Here we have a superclass intercept for catching global GameObject mouse up events
	public virtual void OnMouseUp(GameObject go)
	{
		// Got a TinkerText object? (Also, make sure dragActive is false)
		if (!dragActive && go.GetComponent<GTinkerText>() != null)
		{
			GTinkerText tinkerText = go.GetComponent<GTinkerText>();

			if (tinkerText != null)
			{
				if (stanzaManager != null)
				{
					stanzaManager.OnMouseUp(tinkerText);
				}
			}
		}
		// TinkerGraphic object
		else if (go.GetComponent<GTinkerGraphic>() != null)
		{
			GTinkerGraphic tinkerGraphic = go.GetComponent<GTinkerGraphic>();

			if (tinkerGraphic != null)
			{
				tinkerGraphic.MyOnMouseUp();
			}
		}
	}

	// Here we have a superclass intercept for catching global TinkerGraphic mouse up events
	public virtual void OnMouseUp(GTinkerGraphic tinkerGraphic)
	{
		// override me
	}

	// Here we have a superclass intercept for catching global TinkerText paired mouse up events
	public virtual void OnPairedMouseUp(GTinkerText tinkerText)
	{
		Renderer[] list;
		list = tinkerText.pairedGraphic.gameObject.GetComponentsInChildren<Renderer>();
		foreach(Renderer item in list){   //color all the components
			item.material.color = tinkerText.pairedGraphic.resetColor;
		}
	}
		

	// Override if a scene manager subclass needs a hint manager
	public virtual IEnumerator StartHintManager()
	{
		yield break;
	}

	// Override if a scene manager subclass needs a graphic hint
	public virtual IEnumerator PlayHintAnimation()
	{
		yield break;
	}
		

	//UI Right Button 
	public void NextScene()
	{
		//gameManager.LoadNextScene();
	}

	//UI Left Button
	public void PreviousScene()
	{
		//gameManager.LoadPreviousScene();
	}

	public virtual void ResetInputStates(GGameManager.MouseEvents mouseEvent)
	{
		if (stanzaManager != null)
		{
			stanzaManager.ResetInputStates(mouseEvent);
		}

		GTinkerGraphic[] list;
		list = this.GetComponentsInChildren<GTinkerGraphic> ();
		foreach (GTinkerGraphic tinkerGraphic in list) {
			tinkerGraphic.MyOnMouseUp ();
		}
	}

	public bool CheckFar(Vector2 start, Vector2 end, float requiredDistance){
		if (requiredDistance <= Vector2.Distance (start, end)) {
			return true;
		}
		return false;
	}

	public bool CheckNear(Vector2 start, Vector2 end, float requiredDistance){
		if (requiredDistance >= Vector2.Distance (start, end)) {
			return true;
		}
		return false;
	}
	private void EnableButtons()
	{
		//Color c = Lbutton.gameObject.GetComponent<Image>().color;
		//c.a = 1.0f;
		//Lbutton.gameObject.GetComponent<Image>().color = c;

		//c = Rbutton.gameObject.GetComponent<Image>().color;
		//c.a = 1.0f;
		//Rbutton.gameObject.GetComponent<Image>().color = c;
		Lbutton.gameObject.GetComponent<Image>().color = GGameManager.navblue;

		Rbutton.gameObject.GetComponent<Image>().color = GGameManager.navblue;

		//Lbutton.GetComponent<Button>().interactable = true;
		Rbutton.GetComponent<Button>().interactable = true;
	}


	public float getAudioLength(int i)
	{

		return sounds[i].clip.length;
	}

	public IEnumerator PlayLoopingSound(int index,float startdelay=0f, float enddelay=0f)
	{
		while (true)
		{
			yield return new WaitForSeconds(startdelay);
			if (!sounds[index].isPlaying)
			{
				sounds[index].Play();
			}
			yield return new WaitForSeconds(enddelay);
		}
	}
	public IEnumerator PlayNonLoopSound(int index,float startdelay=0f, float enddelay=0f)
	{ 
		yield return new WaitForSeconds(startdelay);
		if (!sounds[index].isPlaying)
		{
			sounds[index].Play();
			//Debug.Log("abcd   "+sounds[index].name);
		}
		yield return new WaitForSeconds(enddelay);
	}

   


}