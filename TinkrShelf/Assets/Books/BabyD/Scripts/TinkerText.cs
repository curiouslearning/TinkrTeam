using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
//20.3,-18.8 (yellow), 0,-17.4 (white),blue (22.9,-0.4)
public class TinkerText : MonoBehaviour {
    //private static bool check=false;
    public TinkerGraphic pairedGraphic;
    public TinkerText pairedText1;
	public Stanza stanza;
    private float startTime;
    private float endTime;
    public float delayTime;
    private Animator wordanimator;
    private Animator graphicanimator;
    public GameObject anim;
    public GameObject anim2;

    void Start()
    {
		AddCollider ();
        wordanimator = GetComponent<Animator>();
        if (anim2 != null)
            graphicanimator = anim2.GetComponent<Animator>();
    }


    // Takes an xml word element and reads and sets the timing data
    public void SetupWordTiming(XmlNode wordNode)
    {
        startTime = float.Parse(wordNode.Attributes["msStart"].Value) / 1000.0f;
        endTime = float.Parse(wordNode.Attributes["msEnd"].Value) / 1000.0f;
        delayTime = endTime - startTime;
    }

    // Returns the absolute start time
    public float GetStartTime()
    {
        return startTime;
    }

    // Returns the absolute end time
    public float GetEndTime()
    {
        return endTime;
    }

	// Adds a box collider based on initial text mesh size (and makes sure it is large enough)
	private void AddCollider()
	{
		// Setup a trigger collider at runtime so it is the same bounds as the text
		BoxCollider col = gameObject.AddComponent<BoxCollider>();
		col.isTrigger = true;
		col.size = new Vector2(gameObject.GetComponent<RectTransform>().sizeDelta.x , gameObject.GetComponent<RectTransform>().sizeDelta.y);
		// Check against a collider width that is too small (tough to tap on "I" or "1")
		if (col.size.x <= 0.055f)
		{
			// increase size x4
			Vector2 newSize = new Vector2(col.size.x * 4.0f, col.size.y);
			col.size = newSize;
		}
	}

   
    public void clipResume()
    {
        wordanimator.Play("textzoomin");
		wordanimator.ResetTrigger("tapme");    
	}
    public void clipPlay()
	{
            AudioSource source = gameObject.GetComponent<AudioSource>();

            delayTime = 0.21f;
            wordanimator.speed = 1 / (delayTime);

        source.Play();
            wordanimator.SetTrigger("tapme");
    

    }
    public void iconanimPlay()

	{
		if (anim != null)
		{
			anim.SetActive(true);
		}
	}


    public void iconanimResume()
    {
        if (anim != null)
        {
            anim.SetActive(false);
		}
	}
    

    public void graphicPlay()
    {
        if (anim2 != null)
            anim2.SetActive(true);
	}

    void graphicResume()
	{if (anim2 != null)
		anim2.SetActive(false);
        
    }


	// Mouse Down Event
	public void MyMouseDown(bool suppressAnim = false)
	{

        if (!stanza.stanzaManager.sceneManager.disableSounds)
		{
			PlaySound();
		}

		clipPlay();
		iconanimPlay();

		if (!suppressAnim)
		{
			graphicPlay();
		}

		// Is there a TinkerGraphic paired with this TinkerText?
		if (pairedGraphic)
		{
			// Then send the event along!
			pairedGraphic.OnPairedMouseDown(this);
		}
	}

	// Paired Mouse Down Event
	public void OnPairedMouseDown()
	{
		if (!stanza.stanzaManager.sceneManager.disableSounds)
		{
			PlaySound();
		}

		clipPlay();
		iconanimPlay();
	}

	// Mouse Currently Down Event
	public void OnMouseCurrentlyDown()
	{
		if (!stanza.stanzaManager.sceneManager.disableSounds)
		{
			PlaySound();
		}

		clipPlay();
		iconanimPlay();

		// Is there a TinkerGraphic paired with this TinkerText?
		if (pairedGraphic)
		{
			// Then send the event along!
			pairedGraphic.OnPairedMouseCurrentlyDown(this);
		}
	}

	// Paired Mouse Currently Down Event
	public void OnPairedMouseCurrentlyDown()
	{
		if (!stanza.stanzaManager.sceneManager.disableSounds)
		{
			PlaySound();
		} 

		clipPlay();
		iconanimPlay();
	}

	// Mouse Up Event
	public void MyOnMouseUp()
	{
		// Is there a TinkerGraphic paired with this TinkerText?
		if (pairedGraphic)
		{
			// Then send the event along!
			pairedGraphic.OnPairedMouseUp(this);
		}

        clipResume();
		iconanimResume();
		graphicResume();
	}
		
	// Plays any sound that is attached 
	public void PlaySound()
	{
		if (!GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().Play();
		}
	}

	// Stops any sound that is attached 
	public void StopSound()
	{
		if (GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().Stop();
		}
	}


	// Resets the state
	public void Reset()
	{

        // If there is an anim attached, stop it from playing and hide it
        clipResume();
		iconanimResume();

		if (pairedGraphic != null)
		{

			Renderer[] list;
			list = this.pairedGraphic.gameObject.GetComponentsInChildren<Renderer>();
			foreach(Renderer item in list){   //color all the components
				item.material.color = this.pairedGraphic.resetColor;
			}
		}
	}

}