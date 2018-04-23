using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stanza : MonoBehaviour {
    public StanzaManager stanzaManager;
    public List<TinkerText> tinkerTexts;
    // Time delay at end of stanza during autoplay
	public float endDelay; 

	private TinkerText mouseDownTinkerText;
	private TinkerText mouseCurrentlyDownTinkerText;

	// used as tracking to detect stanza auto play
	private int lastTinkerTextIndex = -9999;
  
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public IEnumerator AutoPlay(TinkerText startingTinkerText = null)
    {
        int startingTinkerTextIndex = 0;

        if (startingTinkerText != null)
        {
            startingTinkerTextIndex = tinkerTexts.IndexOf(startingTinkerText);
        }

        for (int i = startingTinkerTextIndex; i < tinkerTexts.Count; i++)
        {
            // delay according to timing data
            //animation not integrated
            //yield return new WaitForSeconds(tinkerTexts[i].GetAnimationDelay());
            TinkerText t = tinkerTexts[i];
            Animator anim = t.GetComponent<Animator>();
            anim.speed = 1 / t.delayTime;
            
            // If we aren't on last word, delay before playing next word
            if (i < tinkerTexts.Count - 1)
            {
                float pauseDelay = tinkerTexts[i + 1].GetStartTime() - tinkerTexts[i].GetEndTime();

                anim.Play("textzoomout");
                yield return new WaitForSeconds(t.delayTime/2);

                //anim.SetTrigger("tapme");
                anim.Play("textzoomin");
                yield return new WaitForSeconds(t.delayTime / 2);
				if (pauseDelay != 0) 
				{
					anim.speed = 1 / pauseDelay;
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
                        anim.Play ("pausedelay");
					//anim.SetTrigger("resume");
					yield return new WaitForSeconds (pauseDelay);
				}
            }
            else // Delay before next stanza
            {
                anim.Play("textzoomout");
                yield return new WaitForSeconds(t.delayTime / 2);

                anim.Play("textzoomin");
                yield return new WaitForSeconds(t.delayTime / 2);
                if (endDelay != 0)
                {
                    anim.speed = 1 / endDelay;
                    anim.Play("enddelay");
                }

                yield return new WaitForSeconds(endDelay);

            }

            // Abort early?
            if (stanzaManager.CancelAutoPlay())
            {
                yield break;
            }
        }

        // Stop the coroutine
        yield break;
    }

	public void OnMouseDown(TinkerText tinkerText, bool suppressAnim = false)
	{
		// if we aren't already mouse down on this text
		if (mouseDownTinkerText != null && mouseDownTinkerText != tinkerText)
		{
			// Then reset the old one
			mouseDownTinkerText.Reset();
		}

		// Assign this new one
		mouseDownTinkerText = tinkerText;

		// And signal the tinkerText 
		tinkerText.MyMouseDown(suppressAnim);
	}

	public void OnPairedMouseDown(TinkerText tinkerText)
	{
		// if we aren't already mouse down on this text
		if (mouseDownTinkerText != null && mouseDownTinkerText != tinkerText)
		{
			// Then reset the old one
			mouseDownTinkerText.Reset();
		}

		// Assign this new one
		mouseDownTinkerText = tinkerText;

		// And signal the tinkerText 
		tinkerText.OnPairedMouseDown();
	}

	public void OnMouseCurrentlyDown(TinkerText tinkerText)
	{
		// If this text is already marked as mouse down, clear that
		if (mouseDownTinkerText != null && mouseDownTinkerText == tinkerText)
		{
			mouseDownTinkerText = null;

			// and reassign it to currently down 
			mouseCurrentlyDownTinkerText = tinkerText;

			DetectStanzaAutoPlay(tinkerText);
		}
		else if (mouseCurrentlyDownTinkerText != null)
		{
			// If this text isn't already marked as currently down
			if (mouseCurrentlyDownTinkerText != tinkerText)
			{
				// Then reset the old one
				mouseCurrentlyDownTinkerText.Reset();

				// Assign this new one
				mouseCurrentlyDownTinkerText = tinkerText;

				// Signal tinkerText
				tinkerText.OnMouseCurrentlyDown();

				DetectStanzaAutoPlay(tinkerText);
			}
		}
		else
		{
			// Assign this new one
			mouseCurrentlyDownTinkerText = tinkerText;

			// Signal tinkerText
			tinkerText.OnMouseCurrentlyDown();

			DetectStanzaAutoPlay(tinkerText);
		}
	}

	public void OnPairedMouseCurrentlyDown(TinkerText tinkerText)
	{
		// If this text is already marked as mouse down, clear that
		if (mouseDownTinkerText != null && mouseDownTinkerText == tinkerText)
		{
			mouseDownTinkerText = null;

			// and reassign it to currently down 
			mouseCurrentlyDownTinkerText = tinkerText;

			DetectStanzaAutoPlay(tinkerText);
		}
		else if (mouseCurrentlyDownTinkerText != null)
		{
			// If this text isn't already marked as currently down
			if (mouseCurrentlyDownTinkerText != tinkerText)
			{
				// Then reset the old one
				mouseCurrentlyDownTinkerText.Reset();

				// Assign this new one
				mouseCurrentlyDownTinkerText = tinkerText;

				// Signal tinkerText
				tinkerText.OnPairedMouseCurrentlyDown();

				DetectStanzaAutoPlay(tinkerText);
			}
		}
		else
		{
			// Assign this new one
			mouseCurrentlyDownTinkerText = tinkerText;

			// Signal tinkerText
			tinkerText.OnPairedMouseCurrentlyDown();

			DetectStanzaAutoPlay(tinkerText);
		}
	}
		

	public void StopAllIndividualSounds()
	{
		foreach (TinkerText tinkerText in tinkerTexts)
		{
			tinkerText.StopSound();
		}
	}

	public void OnMouseUp(TinkerText tinkerText)
	{
		// Assign this new one
		mouseDownTinkerText = tinkerText;
		// And signal the tinkerText 
		tinkerText.MyOnMouseUp();
	}
		

	public void ResetInputStates(GameManager.MouseEvents mouseEvent)
	{
		ResetMouseDownStates();
		ResetMouseCurrentlyDownStates();

		if (mouseEvent == GameManager.MouseEvents.MouseUp)
		{
			lastTinkerTextIndex = -9999;
		}
	}

	private void ResetMouseDownStates()
	{
		if (mouseDownTinkerText != null)
		{
			mouseDownTinkerText.Reset();
		}
		mouseDownTinkerText = null;
	}

	private void ResetMouseCurrentlyDownStates()
	{
		if (mouseCurrentlyDownTinkerText != null)
		{
			mouseCurrentlyDownTinkerText.Reset();
		}
		mouseCurrentlyDownTinkerText = null;
	}

	// Attempts to detect when player has swiped across two words left to right in a stanza to begin autoplay
	private void DetectStanzaAutoPlay(TinkerText tinkerText)
	{
		int currentTinkerTextIndex = tinkerTexts.IndexOf(tinkerText);

		if (lastTinkerTextIndex == -9999)
		{
			lastTinkerTextIndex = currentTinkerTextIndex;
		}
		else
		{

			if (currentTinkerTextIndex == lastTinkerTextIndex + 1)
			{
				
				if (currentTinkerTextIndex < tinkerTexts.Count - 1)     //less than last index of this stanza.
				{
					float pauseDelay = tinkerTexts[currentTinkerTextIndex].GetStartTime() - tinkerTexts[lastTinkerTextIndex].GetEndTime();
					StartCoroutine (RequestToPlay (pauseDelay,this,currentTinkerTextIndex+1 ));    //start from next TinkerText of this stanza (avoid overlap)
    
				}
				else {
					int nextStanzaIndex = stanzaManager.stanzas.IndexOf (this) + 1;
					//start from next stanza's first TinkerText (avoid overlap)
					StartCoroutine (RequestToPlay (endDelay,stanzaManager.stanzas[nextStanzaIndex],0 ));
 
				     }

				//stanzaManager.RequestAutoPlay (this, tinkerTexts [lastTinkerTextIndex]);
				lastTinkerTextIndex = -9999;
			}
			else if (currentTinkerTextIndex < lastTinkerTextIndex)
			{
				lastTinkerTextIndex = -9999;
			}
			else
			{
				lastTinkerTextIndex = currentTinkerTextIndex;
			}
		}

	}
  
	private IEnumerator RequestToPlay(float delay, Stanza stanza, int tinkerTextIndex){
		yield return new WaitForSeconds (delay+.5f);
		stanzaManager.RequestAutoPlay (stanza, stanza.tinkerTexts [tinkerTextIndex]);
	}
}
