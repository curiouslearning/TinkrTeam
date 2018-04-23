using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class StanzaManager : MonoBehaviour {

    public SManager sceneManager;
    public List<Stanza> stanzas;


    private bool autoPlaying = false;
    private bool cancelAutoPlay = false;

    public TextAsset xmlStanzaData;

    void Start()
    {
        GetComponent<AudioSource>().GetComponent<AudioSource>().volume = 1.0f;
    }
    // Load up the scene defined xml timing data
    public void LoadStanzaXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlStanzaData.text);
        XmlNodeList wordsList = xmlDoc.GetElementsByTagName("word");
        SetupWordTimings(wordsList);
    }

    // Loads up a custom stanza audio mp3 and xml timing data
    public void LoadStanzaAudioAndXML(string audioFilename, string xmlFilename)
    {
        // Load our audio
        AudioClip stanzaClip = (AudioClip)Resources.Load(audioFilename) as AudioClip;
        GetComponent<AudioSource>().clip = stanzaClip;

        // Load our xml
        xmlStanzaData = (TextAsset)Resources.Load(xmlFilename);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlStanzaData.text);
        XmlNodeList wordsList = xmlDoc.GetElementsByTagName("word");

        SetupWordTimings(wordsList);
    }

    // Goes through all stanzas and tinkertexts assigning the word timings
    private void SetupWordTimings(XmlNodeList wordsList)
    {
        int stanzaIndex = 0;
        int wordIndex = 0;
        int relativeWordIndex = 0;

        while (wordIndex < wordsList.Count)
        {
            stanzas[stanzaIndex].tinkerTexts[relativeWordIndex].SetupWordTiming(wordsList[wordIndex]);

            wordIndex++;
            relativeWordIndex++;

            // Hit end of stanza yet?
            if (relativeWordIndex > stanzas[stanzaIndex].tinkerTexts.Count - 1)
            {
                relativeWordIndex = 0;
                stanzaIndex++;

                // If we are in a new valid stanza
                if (stanzaIndex < stanzas.Count)
                {
                    // Calculate and set our end delay based on when last word in stanza ends and when first word of next stanza begins
                    float firstWordStartTime = float.Parse(wordsList[wordIndex].Attributes["msStart"].Value) / 1000.0f;
                    float lastWordEndTime = float.Parse(wordsList[wordIndex - 1].Attributes["msEnd"].Value) / 1000.0f;
                    stanzas[stanzaIndex - 1].endDelay = firstWordStartTime - lastWordEndTime;
                }
            }
        }
    }



    // Whether we are currently autoplaying stanzas
    public bool IsAutoPlaying()
    {
        return autoPlaying;
    }


    // Method to request an auto play starting w/ a stanza
    public void RequestAutoPlay(Stanza startingStanza, TinkerText startingTinkerText = null)
    {
        if (!autoPlaying)  // && !sceneManager.disableAutoplay)
        {
            autoPlaying = true;
            cancelAutoPlay = false; // reset our cancel flag
            StartCoroutine(StartAutoPlay(startingStanza, startingTinkerText));
        }
    }

    // Method to request cancelling an autoplay
    public void RequestCancelAutoPlay()
    {
        if (autoPlaying)
        {
            cancelAutoPlay = true;
        }
    }

    // Whether there is a cancel request in progress
    public bool CancelAutoPlay()
    {
        return cancelAutoPlay;
    }


    // Begins an auto play starting w/ a stanza
    private IEnumerator StartAutoPlay(Stanza startingStanza, TinkerText startingTinkerText)
    {
        // If we aren't starting from the beginning, read the audio progress from the startingTinkerText
        GetComponent<AudioSource>().time = startingTinkerText.GetStartTime();
        // Start playing the full stanza audio
        GetComponent<AudioSource>().Play();

        int startingStanzaIndex = stanzas.IndexOf(startingStanza);
        for (int i = startingStanzaIndex; i < stanzas.Count; i++)
        {
            if (i == startingStanzaIndex)
            {
                yield return StartCoroutine(stanzas[i].AutoPlay(startingTinkerText));
            }
            else
            {
                yield return StartCoroutine(stanzas[i].AutoPlay());
            }

            // Abort early?
            if (CancelAutoPlay())
            {
                autoPlaying = false;
                GetComponent<AudioSource>().Stop();
                yield break;
            }
        }

        autoPlaying = false;
        yield break;
    }

	public void OnMouseDown(TinkerText tinkerText, bool suppressAnim = false)
	{
		if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
		{
			tinkerText.stanza.OnMouseDown(tinkerText, suppressAnim);
		}
			
	}

	public void OnPairedMouseDown(TinkerText tinkerText)
	{
		if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
		{
			tinkerText.stanza.OnPairedMouseDown(tinkerText);
		}
	}

	public void OnMouseCurrentlyDown(TinkerText tinkerText)
	{
		if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
		{
			tinkerText.stanza.OnMouseCurrentlyDown(tinkerText);
		}
	}

	public void OnPairedMouseCurrentlyDown(TinkerText tinkerText)
	{
		if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
		{
			tinkerText.stanza.OnPairedMouseCurrentlyDown(tinkerText);
		}
	}

	public void OnMouseUp(TinkerText tinkerText)
	{
		if (tinkerText.stanza != null && stanzas.Contains(tinkerText.stanza))
		{
			tinkerText.stanza.OnMouseUp(tinkerText);
		}
			
	}
		

	public void ResetInputStates(GameManager.MouseEvents mouseEvent)
	{
		foreach (Stanza stanza in stanzas)
		{
			stanza.ResetInputStates(mouseEvent);
		}
	}


    public void OnMouseDown(TinkerGraphic tinkerGraphic)
    {
        if (tinkerGraphic != null)
        {
            tinkerGraphic.MyOnMouseDown();
        }

    }

    public void OnMouseUp(TinkerGraphic tinkerGraphic)
    {
        tinkerGraphic.MyOnMouseUp();
    }

}
